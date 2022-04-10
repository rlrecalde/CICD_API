using System.ComponentModel;

namespace CICD.Model
{
    [Description("[Branch]")]
    public class Branch
    {
        [Description("[Id]")]
        public int Id { get; set; }

        [Description("[ProjectId]")]
        public int ProjectId { get; set; }

        [Description("[Name]")]
        public string Name { get; set; }

        [Description("[Deleted]")]
        public bool Deleted { get; set; }
    }
}
