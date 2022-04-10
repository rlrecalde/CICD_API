using System.ComponentModel.DataAnnotations;

namespace CICD.DTO
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Token { get; set; }

        public bool IsDefault { get; set; }
    }
}