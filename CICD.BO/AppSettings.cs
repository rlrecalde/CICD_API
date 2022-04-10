using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.BO
{
    public class AppSettings
    {
        public string GitHubUrl { get; set; }

        public string UserEndpoint { get; set; }

        public string ContentsRoute { get; set; }

        public string ReposRoute { get; set; }

        public string BranchesRoute { get; set; }

        public string CommitsRoute { get; set; }

        public string CommentsRoute { get; set; }

        public string WorkingDirectory { get; set; }

        public string DockerizerFullFileName { get; set; }

        public bool UseWsl { get; set; }
    }
}
