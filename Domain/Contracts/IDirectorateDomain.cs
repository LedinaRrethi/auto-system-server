using DTO.DirectorateDTO;

namespace Domain.Contracts
{
    public interface IDirectorateDomain
    {
        Task<List<DirectorateDTO>> GetAllActiveAsync();
    }
}
