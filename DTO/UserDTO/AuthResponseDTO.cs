
namespace DTO.UserDTO
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }

        public string RefreshToken { get; set; } = null!;
    }
}

