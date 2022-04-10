using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DTO
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string RelativePath { get; set; }

        [Required]
        [MaxLength(5)]
        public string DotnetVersion { get; set; }

        [Required]
        public bool Test { get; set; }

        [Required]
        public bool Deploy { get; set; }

        [Required]
        public int DeployPort { get; set; }
    }
}
