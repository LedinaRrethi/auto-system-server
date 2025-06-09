using DTO.UserDTO;

public interface IAuthDomain
{
    Task RegisterAsync(RegisterDto dto);
    Task<AuthResponseDTO> LoginAsync(LoginDto dto, string ipAddress);
    Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken, string ipAddress);
    Task LogoutAsync(string refreshToken);
}
