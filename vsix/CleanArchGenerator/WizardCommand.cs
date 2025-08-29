using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace CleanArchGenerator
{
    internal sealed class WizardCommand
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("fceadb61-1a69-4607-9c7e-2fd6bfe0dd0b");
        private readonly AsyncPackage package;

        private WizardCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package;
            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            _ = new WizardCommand(package, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            var window = new Wizard.CleanArchWizardWindow();
            window.ShowModal();
        }
    }
}
