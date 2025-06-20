using DTO.InspectionDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IInspectionDomain
    {
        Task<bool> CreateInspectionRequestAsync(InspectionRequestCreateDTO dto, string userId, string ip);
    }


}
