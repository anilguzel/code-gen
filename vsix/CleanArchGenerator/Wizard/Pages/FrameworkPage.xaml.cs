using System.Windows;
using System.Windows.Controls;

namespace CleanArchGenerator.Wizard.Pages
{
    public partial class FrameworkPage : Page, IWizardPage
    {
        private Frame frame;
        private WizardState state;
        public FrameworkPage(WizardState state)
        {
            InitializeComponent();
            this.state = state;
        }

        public void SetFrame(Frame frame) => this.frame = frame;

        private void Back_Click(object sender, RoutedEventArgs e) => frame.GoBack();

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            state.Framework.Source = SourceBox.Text;
            state.Framework.PackageId = PackageBox.Text;
            state.Framework.Version = VersionBox.Text;
            frame.Navigate(new EntitiesPage(state));
        }
    }
}
