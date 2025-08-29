import argparse
from pathlib import Path
import uuid

PROJECTS = ["Domain", "Application", "Infrastructure", "Api"]

FRAMEWORK_PACKAGES = {
    "Domain": ["Company.Framework"],
    "Application": ["Company.Framework"],
    "Infrastructure": ["Company.Framework.Data"],
    "Api": ["Company.Framework.Web"],
}

EF_PACKAGES = {
    "postgresql": "Npgsql.EntityFrameworkCore.PostgreSQL",
    "sqlserver": "Microsoft.EntityFrameworkCore.SqlServer",
    "oracle": "Oracle.EntityFrameworkCore",
}

PROGRAM_CS = """using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Framework integration placeholder
builder.Services.AddCompanyFramework(cfg =>
{{
    cfg.UseDatabaseProvider("{db_provider}", "{connection_string}");
}});

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();
"""

CS_PROJ_TEMPLATE = """<Project Sdk=\"Microsoft.NET.Sdk{sdk_ext}\">
  <PropertyGroup>
    <TargetFramework>{framework}</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
{package_refs}</Project>
"""

# Package references omit versions when Directory.Packages.props is used
PACKAGE_REF_TEMPLATE = "    <PackageReference Include=\"{package}\" />\n"

SLN_HEADER = """Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.0.31903.59
MinimumVisualStudioVersion = 10.0.40219.1
"""

PROJECT_ENTRY_TEMPLATE = "Project(\"{guid}\") = \"{name}\", \"src/{name}/{name}.csproj\", \"{proj_guid}\"\nEndProject\n"
CONFIG_ENTRY_TEMPLATE = (
    "        {proj_guid}.Debug|Any CPU.ActiveCfg = Debug|Any CPU\n"
    "        {proj_guid}.Debug|Any CPU.Build.0 = Debug|Any CPU\n"
    "        {proj_guid}.Release|Any CPU.ActiveCfg = Release|Any CPU\n"
    "        {proj_guid}.Release|Any CPU.Build.0 = Release|Any CPU\n"
)

PROPS_TEMPLATE = """<Project>
  <ItemGroup>
{items}  </ItemGroup>
</Project>
"""

PROPS_ITEM_TEMPLATE = "    <PackageVersion Include=\"{package}\" Version=\"{version}\" />\n"


def create_project(solution: str, project: str, framework: str, db_provider: str):
    project_dir = Path("src") / f"{solution}.{project}"
    project_dir.mkdir(parents=True, exist_ok=True)
    package_refs = ""
    for package in FRAMEWORK_PACKAGES.get(project, []):
        package_refs += PACKAGE_REF_TEMPLATE.format(package=package)
    if project == "Infrastructure":
        ef_package = EF_PACKAGES[db_provider]
        package_refs += PACKAGE_REF_TEMPLATE.format(package=ef_package)
    sdk_ext = ".Web" if project == "Api" else ""
    csproj = CS_PROJ_TEMPLATE.format(
        framework=framework,
        package_refs=f"  <ItemGroup>\n{package_refs}  </ItemGroup>\n" if package_refs else "",
        sdk_ext=sdk_ext,
    )
    (project_dir / f"{solution}.{project}.csproj").write_text(csproj)
    if project == "Api":
        program = PROGRAM_CS.format(db_provider=db_provider, connection_string="<connection>")
        (project_dir / "Program.cs").write_text(program)
    else:
        (project_dir / "Class1.cs").write_text(
            f"namespace {solution}.{project};\n\npublic class Class1 {{ }}\n"
        )
    return project_dir


def create_solution(
    solution: str,
    framework: str,
    db_provider: str,
    framework_version: str,
    ef_version: str,
):
    project_entries = ""
    config_entries = ""
    package_versions = {}
    for proj in PROJECTS:
        create_project(solution, proj, framework, db_provider)
        proj_guid = str(uuid.uuid4()).upper()
        full_name = f"{solution}.{proj}"
        project_entries += PROJECT_ENTRY_TEMPLATE.format(
            guid="{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}",
            name=full_name,
            proj_guid=proj_guid,
        )
        config_entries += CONFIG_ENTRY_TEMPLATE.format(proj_guid=proj_guid)
        for pkg in FRAMEWORK_PACKAGES.get(proj, []):
            package_versions[pkg] = framework_version
        if proj == "Infrastructure":
            package_versions[EF_PACKAGES[db_provider]] = ef_version

    items = "".join(
        PROPS_ITEM_TEMPLATE.format(package=pkg, version=ver)
        for pkg, ver in sorted(package_versions.items())
    )
    Path("Directory.Packages.props").write_text(PROPS_TEMPLATE.format(items=items))

    sln = (
        SLN_HEADER
        + project_entries
        + "Global\n    GlobalSection(SolutionConfigurationPlatforms) = preSolution\n        Debug|Any CPU = Debug|Any CPU\n        Release|Any CPU = Release|Any CPU\n    EndGlobalSection\n    GlobalSection(ProjectConfigurationPlatforms) = postSolution\n"
        + config_entries
        + "    EndGlobalSection\n    GlobalSection(SolutionProperties) = preSolution\n     HideSolutionNode = FALSE\n    EndGlobalSection\nEndGlobal\n"
    )
    Path(f"{solution}.sln").write_text(sln)


def main():
    parser = argparse.ArgumentParser(
        description="Generate Clean Architecture solution skeleton"
    )
    parser.add_argument("name", help="Solution name")
    parser.add_argument("--framework", choices=["net8.0", "net9.0"], default="net8.0")
    parser.add_argument(
        "--db-provider", choices=list(EF_PACKAGES.keys()), default="postgresql"
    )
    parser.add_argument(
        "--framework-version",
        default="1.0.0",
        help="Version for Company.Framework packages",
    )
    parser.add_argument(
        "--ef-version",
        default="1.0.0",
        help="Version for EF Core provider package",
    )
    args = parser.parse_args()
    create_solution(
        args.name,
        args.framework,
        args.db_provider,
        args.framework_version,
        args.ef_version,
    )


if __name__ == "__main__":
    main()

