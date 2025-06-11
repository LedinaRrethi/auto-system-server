using DTO.UserDTO;

namespace Domain.Contracts
{
    public interface IAuthDomain
    {
        Task RegisterAsync(RegisterDTO dto);
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto, string ipAddress);
        Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken, string ipAddress);
        Task LogoutAsync(string refreshToken);
    }
}
