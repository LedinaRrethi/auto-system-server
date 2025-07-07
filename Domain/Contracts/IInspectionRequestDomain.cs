using DTO;
using DTO.InspectionDTO;
using Helpers.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IInspectionRequestDomain
    {
        Task<bool> CreateInspectionRequestAsync(InspectionRequestCreateDTO dto);
        Task<PaginationResult<MyInspectionRequestDTO>> GetCurrentUserPagedInspectionRequestsAsync(PaginationDTO dto);

        Task<Dictionary<string, int>> GetInspectionStatusCountAsync(Guid directoryId);



    }


}
