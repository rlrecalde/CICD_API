using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BO
{
    public class Process
    {
        public string Name { get; set; }

        public string WorkingDirectory { get; set; }

        public string Arguments { get; set; }
    }
}
