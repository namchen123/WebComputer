using System.ComponentModel.DataAnnotations;

namespace WebComputer.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        public String oldPassword {  get; set; }
        [Required]
        public String newPassword { get; set; }
        [Required]
        public String confirmNewPassword { get; set; }
    }
}
