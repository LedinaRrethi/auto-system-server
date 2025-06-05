using DTO.UserDTO;

public interface IAuthDomain
{
    Task RegisterAsync(RegisterDto dto);
    Task<string> LoginAsync(LoginDto dto);
}
