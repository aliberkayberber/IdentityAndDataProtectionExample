using System.ComponentModel.DataAnnotations;

namespace IdentityAndDataProtectionExample.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
