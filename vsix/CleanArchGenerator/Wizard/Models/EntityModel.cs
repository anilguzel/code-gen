using System.Collections.Generic;

namespace CleanArchGenerator.Wizard.Models
{
    public class EntityModel
    {
        public string Name { get; set; } = string.Empty;
        public List<PropertyModel> Properties { get; set; } = new();
    }
}
