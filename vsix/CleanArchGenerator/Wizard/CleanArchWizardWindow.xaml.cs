using System.Windows;
using System.Windows.Navigation;

namespace CleanArchGenerator.Wizard
{
    public partial class CleanArchWizardWindow : Window
    {
        public CleanArchWizardWindow()
        {
            InitializeComponent();
            WizardFrame.Navigated += WizardFrame_Navigated;
            WizardFrame.Navigate(new Pages.ProjectSettingsPage(WizardState.Instance));
        }

        private void WizardFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is Pages.IWizardPage page)
                page.SetFrame(WizardFrame);
        }
    }
}
