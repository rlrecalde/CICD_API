using System.ComponentModel;

namespace CICD.Model
{
    [Description("[Configuration]")]
    public class Configuration
    {
        [Description("[Id]")]
        public int Id { get; set; }

        [Description("[ConfigurationTypeId]")]
        public int ConfigurationTypeId { get; set; }

        [Description("[Value]")]
        public string Value { get; set; }
    }
}
