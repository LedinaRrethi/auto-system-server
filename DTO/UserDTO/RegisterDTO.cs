using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Helpers.Enumerations;

namespace DTO.UserDTO
{
    public class RegisterDTO
    {
        public string FirstName { get; set; } = null!;

        public string FatherName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, DataType(DataType.Password), MinLength(8)]
        public string Password { get; set; } = null!;

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;


        [Required]
        public UserRole Role { get; set; }  

        public string? SpecialistNumber { get; set; }

        public Guid? DirectorateId { get; set; }
        public string? PersonalId { get; set; }

    
    }

}

