using AutoMapper;
using DAL.UoW;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Domain.Concrete
{
    public class DomainBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public DomainBase(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        protected string GetCurrentIp()
        {
            var context = _httpContextAccessor.HttpContext;

            var forwarded = context?.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(forwarded))
                return forwarded.Split(',')[0].Trim(); 

            return context?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        protected string? GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        protected void SetAuditOnCreate(dynamic entity)
        {
            entity.CreatedOn = DateTime.UtcNow;
            entity.CreatedBy = GetCurrentUserId();
            entity.CreatedIp = GetCurrentIp();
        }

        protected void SetAuditOnUpdate(dynamic entity)
        {
            entity.ModifiedOn = DateTime.UtcNow;
            entity.ModifiedBy = GetCurrentUserId();
            entity.ModifiedIp = GetCurrentIp();
        }
    }
}
