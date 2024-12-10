using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Identity.Users
{
    public class UpdateUserRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [MinLength(6)]
        public string CurrentPassword { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }
    }
}