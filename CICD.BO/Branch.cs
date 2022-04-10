using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BO
{
    public class Branch
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Commit LastCommit { get; set; }

        public Project Project { get; set; }
    }
}
