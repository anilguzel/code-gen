using System.IO;
using System.Text;
using Scriban;
using CleanArchGenerator.Wizard;
using CleanArchGenerator.Wizard.Models;

namespace CleanArchGenerator.Generators
{
    public class SolutionGenerator
    {
        public void Generate(string basePath, WizardState state)
        {
            var solutionDir = Path.Combine(basePath, state.Project.SolutionName);
            Directory.CreateDirectory(solutionDir);
            var srcDir = Path.Combine(solutionDir, "src");
            Directory.CreateDirectory(srcDir);

            string[] projects = { "Domain", "Application", "Infrastructure", "Api" };
            foreach (var prj in projects)
            {
                var projectName = state.Project.SolutionName + "." + prj;
                var projectDir = Path.Combine(srcDir, projectName);
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, projectName + ".csproj"), ProjectFileContent(state, prj));
                if (prj == "Api")
                    File.WriteAllText(Path.Combine(projectDir, "Program.cs"), ProgramCs(state));
            }
            File.WriteAllText(Path.Combine(solutionDir, state.Project.SolutionName + ".sln"), "// solution placeholder");
        }

        private string ProjectFileContent(WizardState state, string layer)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk\">");
            sb.AppendLine("  <PropertyGroup>");
            sb.AppendLine($"    <TargetFramework>{state.Project.DotNetVersion}</TargetFramework>");
            sb.AppendLine("  </PropertyGroup>");
            if (layer == "Api")
            {
                sb.AppendLine("  <ItemGroup>");
                sb.AppendLine($"    <PackageReference Include=\"{state.Framework.PackageId}.Web\" Version=\"{state.Framework.Version}\" />");
                sb.AppendLine("  </ItemGroup>");
            }
            else if (layer == "Infrastructure")
            {
                sb.AppendLine("  <ItemGroup>");
                sb.AppendLine($"    <PackageReference Include=\"{state.Framework.PackageId}.Data\" Version=\"{state.Framework.Version}\" />");
                sb.AppendLine("  </ItemGroup>");
            }
            else if (layer == "Application")
            {
                sb.AppendLine("  <ItemGroup>");
                sb.AppendLine($"    <PackageReference Include=\"{state.Framework.PackageId}\" Version=\"{state.Framework.Version}\" />");
                sb.AppendLine("  </ItemGroup>");
            }
            sb.AppendLine("</Project>");
            return sb.ToString();
        }

        private string ProgramCs(WizardState state)
        {
            var template = Template.Parse(@"using Microsoft.AspNetCore.Builder;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCompanyFramework(cfg => {
    cfg.UseDatabaseProvider(\"{{db}}\", \"{{cs}}\");
    cfg.UseDataProtection();
});
var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();");
            var db = state.Database.Provider switch
            {
                "PostgreSQL" => "UseNpgsql",
                "SQL Server" => "UseSqlServer",
                "Oracle" => "UseOracle",
                _ => "UseNpgsql"
            };
            return template.Render(new { db = db, cs = state.Database.ConnectionString });
        }
    }
}
