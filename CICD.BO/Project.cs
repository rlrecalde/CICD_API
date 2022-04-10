using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BO
{
    public class Project
    {
        public int Id { get; set; }

        public User User { get; set; }

        public string Name { get; set; }

        public string RelativePath { get; set; }

        public string DotnetVersion { get; set; }

        public bool Test { get; set; }

        public bool Deploy { get; set; }

        public int DeployPort { get; set; }

        public IEnumerable<Branch> Branches { get; set; }
    }
}
