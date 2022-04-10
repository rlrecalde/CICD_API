using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BO
{
    public class Configuration
    {
        public int Id { get; set; }

        public ConfigurationType ConfigurationType { get; set; }

        public string Value { get; set; }
    }
}
