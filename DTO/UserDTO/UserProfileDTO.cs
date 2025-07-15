
namespace DTO.UserDTO
{
    public class UserProfileDTO
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string FatherName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string? PersonalId { get; set; }
    }

}
