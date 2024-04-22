using System.ComponentModel.DataAnnotations;

namespace Poznamky.Models
{
    public class PoznamkaModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Vlozeno { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public bool Dulezita { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}
