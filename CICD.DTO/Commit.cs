using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DTO
{
    public class Commit
    {
        public string Id { get; set; }

        public int BranchId { get; set; }

        public string? CommitterLogin { get; set; }

        public string? CommitterName { get; set; }

        public DateTime Date { get; set; }

        public string? Message { get; set; }
    }
}
