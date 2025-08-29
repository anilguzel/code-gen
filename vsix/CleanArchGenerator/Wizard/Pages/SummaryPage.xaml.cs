using System.IO;
using System.Windows;
using System.Windows.Controls;
using CleanArchGenerator.Generators;

namespace CleanArchGenerator.Wizard.Pages
{
    public partial class SummaryPage : Page, IWizardPage
    {
        private Frame frame;
        private WizardState state;
        public SummaryPage(WizardState state)
        {
            InitializeComponent();
            this.state = state;
        }

        public void SetFrame(Frame frame) => this.frame = frame;

        private void Back_Click(object sender, RoutedEventArgs e) => frame.GoBack();

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            var generator = new SolutionGenerator();
            var path = Directory.GetCurrentDirectory();
            generator.Generate(path, state);
            StatusText.Text = "Solution generated at " + path;
        }
    }
}
