using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using Domain.Notifications;
using DTO;
using DTO.InspectionDTO;
using DTO.VehicleDTO;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Domain.Concrete
{
    public class InspectionDomain : IInspectionDomain
    {
        private readonly IInspectionRepository _repo;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IHubContext<NotificationHub, INotificationHub> _notificationHubContext;

        private IRepository<Auto_InspectionDocs> _docRepo => _unitOfWork.GetRepository<IRepository<Auto_InspectionDocs>>();
        private IRepository<Auto_Inspections> _inspectionRepo => _unitOfWork.GetRepository<IRepository<Auto_Inspections>>();
        private IRepository<Auto_InspectionRequests> _requestRepo => _unitOfWork.GetRepository<IRepository<Auto_InspectionRequests>>();

        public InspectionDomain(
            IInspectionRepository repo,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            INotificationRepository notificationRepository,
            IHttpContextAccessor httpContextAccessor,
            IHubContext<NotificationHub, INotificationHub> notificationHubContext
        )
        {
            _repo = repo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
            _httpContextAccessor = httpContextAccessor;
            _notificationHubContext = notificationHubContext;
        }

        public async Task<PaginationResult<InspectionRequestListDTO>> GetMyRequestsAsync(string userId, PaginationDTO dto)
        {
            return await _repo.GetRequestsBySpecialistAsync(userId, dto);
        }

        public async Task<bool> ApproveInspectionAsync(InspectionApprovalDTO dto, string? userId, string ip)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                var inspection = await _repo.GetInspectionWithRequestAsync(dto.IDPK_Inspection);

                if (inspection == null)
                    return false;

                var totalFileSize = 0;

             
                foreach (var doc in dto.Documents)
                {
                    if (!doc.DocumentName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                        throw new Exception($"File '{doc.DocumentName}' is not a PDF.");

                    var fileBytes = Convert.FromBase64String(doc.FileBase64);
                    totalFileSize += fileBytes.Length;
                 
                    if (totalFileSize > 5 * 1024 * 1024)
                        throw new Exception($"File '{doc.DocumentName}' is larger than 5MB.");

                }

                // Update Inspection
                inspection.IsPassed = dto.IsPassed;
                inspection.Comment = dto.Comment;
                inspection.ModifiedBy = userId;
                inspection.ModifiedOn = DateTime.UtcNow;
                inspection.ModifiedIp = ip;
                inspection.IDFK_Specialist = userId;

                // Update Request
                inspection.Request.Status = dto.IsPassed ? InspectionStatus.Approved : InspectionStatus.Rejected;
                inspection.Request.ModifiedBy = userId;
                inspection.Request.ModifiedOn = DateTime.UtcNow;
                inspection.Request.ModifiedIp = ip;

                await _inspectionRepo.UpdateAsync(inspection);
                await _requestRepo.UpdateAsync(inspection.Request);

                // Add Documents
                foreach (var doc in dto.Documents)
                {
                    var docEntity = new Auto_InspectionDocs
                    {
                        IDPK_InspectionDoc = Guid.NewGuid(),
                        IDFK_Inspection = dto.IDPK_Inspection,
                        DocumentName = doc.DocumentName,
                        FileBase64 = doc.FileBase64,
                        CreatedBy = userId!,
                        CreatedOn = DateTime.UtcNow,
                        CreatedIp = ip
                    };

                    await _docRepo.AddAsync(docEntity);
                }

                // send notificatin
                var vehicleOwnerId = inspection.Request.Vehicle?.IDFK_Owner;
                if (!string.IsNullOrWhiteSpace(vehicleOwnerId))
                {
                    var notif = new Auto_Notifications
                    {
                        IDPK_Notification = Guid.NewGuid(),
                        IDFK_Receiver = vehicleOwnerId,
                        Title = "Vehicle Inspection Result",
                        Message = inspection.IsPassed
                            ? "Your vehicle has successfully passed the technical inspection.."
                            : "Your vehicle did not pass the technical inspection. Please review the comments and attached documents.",
                        Type = NotificationType.InspectionResult,
                        CreatedBy = userId!,
                        CreatedOn = DateTime.UtcNow,
                        CreatedIp = ip
                    };

                    await _notificationRepository.AddNotificationAsync(notif);

                    await NotificationConnections.SendNotificationToUserAsync(_notificationHubContext, notif, vehicleOwnerId);

                }

                await _repo.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<VehicleDTO>> GetMyVehiclesAsync(string userId)
        {
            var vehicles = await _repo.GetVehiclesByUserIdAsync(userId);
            return _mapper.Map<List<VehicleDTO>>(vehicles);
        }
    }
}
