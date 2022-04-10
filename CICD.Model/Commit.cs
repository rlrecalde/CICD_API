using System.ComponentModel;

namespace CICD.Model
{
    [Description("[Commit]")]
    public class Commit
    {
        [Description("[Id]")]
        public int Id { get; set; }

        [Description("[BranchId]")]
        public int BranchId { get; set; }

        [Description("[Sha]")]
        public string Sha { get; set; }

        [Description("[CommitterLogin]")]
        public string CommitterLogin { get; set; }

        [Description("[CommitterName]")]
        public string CommitterName { get; set; }

        [Description("[Date]")]
        public DateTime Date { get; set; }

        [Description("[Message]")]
        public string Message { get; set; }
    }
}
