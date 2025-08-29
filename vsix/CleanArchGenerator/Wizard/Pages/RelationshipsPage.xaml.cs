using System.Windows;
using System.Windows.Controls;

namespace CleanArchGenerator.Wizard.Pages
{
    public partial class RelationshipsPage : Page, IWizardPage
    {
        private Frame frame;
        private WizardState state;
        public RelationshipsPage(WizardState state)
        {
            InitializeComponent();
            this.state = state;
        }

        public void SetFrame(Frame frame) => this.frame = frame;

        private void Back_Click(object sender, RoutedEventArgs e) => frame.GoBack();

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new SummaryPage(state));
        }
    }
}
