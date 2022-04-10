using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BO
{
    public class ProcessResponse
    {
        public bool Sucess { get; set; }

        public IEnumerable<string> DataLines { get; set; }

        public IEnumerable<string> ErrorLines { get; set; }
    }
}
