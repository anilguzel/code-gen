using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace CleanArchGenerator
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("Clean Arch Generator", "Scaffold Clean Architecture solutions", "0.1")]
    [Guid(PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class CleanArchGeneratorPackage : AsyncPackage
    {
        public const string PackageGuidString = "d29a2e3c-c92a-4e75-8efc-4a23a5d0a112";

        protected override async Task InitializeAsync(System.Threading.CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);
            await WizardCommand.InitializeAsync(this);
        }
    }
}
