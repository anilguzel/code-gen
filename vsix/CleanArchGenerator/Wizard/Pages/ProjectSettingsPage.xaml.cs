using System.Windows;
using System.Windows.Controls;

namespace CleanArchGenerator.Wizard.Pages
{
    public partial class ProjectSettingsPage : Page, IWizardPage
    {
        private Frame frame;
        private WizardState state;
        public ProjectSettingsPage(WizardState state)
        {
            InitializeComponent();
            this.state = state;
        }

        public void SetFrame(Frame frame) => this.frame = frame;

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            state.Project.SolutionName = SolutionNameBox.Text;
            state.Project.DotNetVersion = FrameworkCombo.SelectedIndex == 0 ? "net8.0" : "net9.0";
            frame.Navigate(new DatabasePage(state));
        }
    }
}
