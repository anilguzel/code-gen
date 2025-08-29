# Clean Architecture Solution Generator

This repository contains a minimal Python-based generator that scaffolds a Clean Architecture solution structure and a proof-of-concept Visual Studio extension skeleton.

## Usage

```bash
python3 generate_solution.py MySolution --framework net9.0 --db-provider postgresql
```

The command creates a `MySolution.sln` and the following project layout under `src/`:

- `MySolution.Domain`
- `MySolution.Application`
- `MySolution.Infrastructure`
- `MySolution.Api`

Each project targets the selected .NET version and includes placeholder package references such as `Company.Framework`. The Infrastructure project also references the chosen EF Core provider package.

## Visual Studio Extension (Preview)

Under `vsix/` there is a basic VSIX/WPF wizard skeleton (`CleanArchGenerator`). The wizard collects project, database, framework and entity settings and invokes a C# `SolutionGenerator` to scaffold the same structure as the Python script. The extension is not fully functional and is provided for reference only; building the VSIX requires the Visual Studio SDK on Windows.

> **Note:** Both the Python CLI and the VSIX wizard are simplified prototypes and do not implement the full PRD.
