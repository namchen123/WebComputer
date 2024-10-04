using System.ComponentModel.DataAnnotations;

namespace WebComputer.Models
{
    public class LoginViewModel
    {
        [Required]
        public String Username { get; set; }
        [Required]
        public String Password { get; set; }

    }
}
