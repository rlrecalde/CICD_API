using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DTO
{
    public class Branch
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Commit LastCommit { get; set; }

        [Required]
        public Project Project { get; set; }
    }
}
