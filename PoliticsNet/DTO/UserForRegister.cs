using System.ComponentModel.DataAnnotations;

namespace PoliticsNet.DTO
{
    public class UserForRegister
    {
        //TODO: add stuff
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        // [Required]
        // [StringLength(100, MinimumLength = 3)]
        // public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Code { get; set; }

    }
}