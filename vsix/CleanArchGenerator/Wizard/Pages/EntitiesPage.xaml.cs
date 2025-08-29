using System.Windows;
using System.Windows.Controls;

namespace CleanArchGenerator.Wizard.Pages
{
    public partial class EntitiesPage : Page, IWizardPage
    {
        private Frame frame;
        private WizardState state;
        public EntitiesPage(WizardState state)
        {
            InitializeComponent();
            this.state = state;
            EntityList.ItemsSource = state.Entities;
        }

        public void SetFrame(Frame frame) => this.frame = frame;

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(EntityNameBox.Text))
            {
                state.Entities.Add(new Models.EntityModel { Name = EntityNameBox.Text });
                EntityList.Items.Refresh();
                EntityNameBox.Text = string.Empty;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) => frame.GoBack();

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new RelationshipsPage(state));
        }
    }
}
