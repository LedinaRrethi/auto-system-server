using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using DTO.DirectorateDTO;
using Microsoft.AspNetCore.Http;

namespace Domain.Concrete
{
    public class DirectorateDomain : DomainBase , IDirectorateDomain
    {
        public DirectorateDomain(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, mapper, httpContextAccessor)
        {
        }

        private IDirectorateRepository DirectorateRepository => _unitOfWork.GetRepository<IDirectorateRepository>();

        public async Task<List<DirectorateDTO>> GetAllActiveAsync()
        {
            var list = await DirectorateRepository.GetAllActiveAsync();
            return _mapper.Map<List<DirectorateDTO>>(list);
        }

    }
}
