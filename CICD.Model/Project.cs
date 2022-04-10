using System.ComponentModel;

namespace CICD.Model
{
    [Description("[Project]")]
    public class Project
    {
        [Description("[Id]")]
        public int Id { get; set; }

        [Description("[UserId]")]
        public int UserId { get; set; }

        [Description("[Name]")]
        public string Name { get; set; }

        [Description("[RelativePath]")]
        public string RelativePath { get; set; }

        [Description("[DotnetVersion]")]
        public string DotnetVersion { get; set; }

        [Description("[Test]")]
        public bool Test { get; set; }

        [Description("[Deploy]")]
        public bool Deploy { get; set; }

        [Description("[DeployPort]")]
        public int DeployPort { get; set; }
    }
}
