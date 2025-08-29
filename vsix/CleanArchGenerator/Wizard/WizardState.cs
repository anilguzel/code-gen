namespace CleanArchGenerator.Wizard
{
    public class WizardState
    {
        public static WizardState Instance { get; } = new WizardState();
        public Models.ProjectSettings Project { get; set; } = new Models.ProjectSettings();
        public Models.DatabaseSettings Database { get; set; } = new Models.DatabaseSettings();
        public Models.FrameworkSettings Framework { get; set; } = new Models.FrameworkSettings();
        public System.Collections.Generic.List<Models.EntityModel> Entities { get; set; } = new();
    }
}
