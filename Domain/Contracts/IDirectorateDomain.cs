using DTO.DirectorateDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IDirectorateDomain
    {
        Task<List<DirectorateDTO>> GetAllActiveAsync();
    }
}
