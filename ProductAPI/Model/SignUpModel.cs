using ProductAPI.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Model
{
    public class SignUpModel
    {
        
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }

        public Guid RoleId { get; set; }
    }
}
