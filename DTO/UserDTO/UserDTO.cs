
namespace DTO.UserDTO
{
    public class UserDTO
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;
        public string FatherName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;
        public DateTime BirthDate { get; set; }

        public string Role { get; set; } = null!; 
        public string Status { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string? SpecialistNumber { get; set; }
        public string? DirectorateName { get; set; } 
    }
}
