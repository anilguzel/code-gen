using System.Windows;
using System.Windows.Controls;

namespace CleanArchGenerator.Wizard.Pages
{
    public partial class DatabasePage : Page, IWizardPage
    {
        private Frame frame;
        private WizardState state;
        public DatabasePage(WizardState state)
        {
            InitializeComponent();
            this.state = state;
        }

        public void SetFrame(Frame frame) => this.frame = frame;

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            frame.GoBack();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            state.Database.Provider = ((ComboBoxItem)ProviderCombo.SelectedItem).Content.ToString();
            state.Database.ConnectionString = ConnectionStringBox.Text;
            frame.Navigate(new FrameworkPage(state));
        }
    }
}
