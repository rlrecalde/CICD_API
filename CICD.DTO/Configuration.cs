using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICD.DTO
{
    public class Configuration
    {
        public int Id { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int ConfigurationTypeId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Value { get; set; }
    }
}
