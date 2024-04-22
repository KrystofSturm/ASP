using System.ComponentModel.DataAnnotations;

namespace Poznamky.Models
{
    public class UzivatelModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public bool Agreed { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
