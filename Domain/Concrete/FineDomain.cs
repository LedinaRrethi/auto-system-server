using AutoMapper;
using DAL.Contracts;
using DAL.UoW;
using Domain.Contracts;
using Domain.Notifications;
using DTO.FineDTO;
using Entities.Models;
using Helpers.Enumerations;
using Helpers.Pagination;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Domain.Concrete
{
    public class FineDomain : IFineDomain
    {
        private readonly IFineRepository _repo;
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager<Auto_Users> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IHubContext<NotificationHub, INotificationHub> _notificationHubContext;


        public FineDomain(
            IFineRepository repo, 
            IMapper mapper, 
            UserManager<Auto_Users> userManager, 
            IUnitOfWork unitOfWork , 
            INotificationRepository notificationRepository ,
            IHubContext<NotificationHub, INotificationHub> notificationHubContext
        )        
        {
            _repo = repo;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
            _notificationHubContext = notificationHubContext;
        }

        public async Task<bool> CreateFineAsync(FineCreateDTO dto, string policeId, string ip)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                // kerkoj automjetin ose pronarin nese ekziston
                var vehicle = await _repo.GetVehicleByPlateAsync(dto.PlateNumber);
                Auto_Users? owner = vehicle?.IDFK_Owner != null
                    ? await _repo.GetUserByIdAsync(vehicle.IDFK_Owner)
                    : null;

                Auto_FineRecipients? recipient = null;
                Auto_Users? matchedUser = null;

                // gjej FineRecipient ekzistues me ane te id se pronarit apo personal id
                if (owner != null)
                {
                    recipient = await _repo.GetFineRecipientByUserIdAsync(owner.Id);
                }
                else if (!string.IsNullOrWhiteSpace(dto.PersonalId))
                {
                    recipient = await _repo.GetFineRecipientByPersonalIdAsync(dto.PersonalId.Trim());
                }

               
                // nese recipient nuk ekzisotn fr krijoj te ri
                if (recipient == null)
                {

                    if (!string.IsNullOrWhiteSpace(dto.PersonalId))
                    {
                        var normalizedPersonalId = dto.PersonalId.Trim().ToLower();

                        matchedUser = await _userManager.Users
                            .FirstOrDefaultAsync(u =>
                                u.PersonalId != null &&
                                u.PersonalId.Trim().ToLower() == normalizedPersonalId &&
                                u.Invalidated == 0);
                    }

                    recipient = new Auto_FineRecipients
                    {
                        IDPK_FineRecipient = Guid.NewGuid(),
                        IDFK_User = owner?.Id ?? matchedUser?.Id,
                        FirstName = owner?.FirstName ?? dto.FirstName!,
                        LastName = owner?.LastName ?? dto.LastName!,
                        FatherName = owner?.FatherName ?? dto.FatherName,
                        PersonalId = owner?.PersonalId ?? dto.PersonalId!,
                        PlateNumber = dto.PlateNumber, //TO DO 
                        CreatedBy = policeId,
                        CreatedOn = DateTime.UtcNow,
                        CreatedIp = ip
                    };

                    await _repo.AddFineRecipientAsync(recipient);
                    await _repo.SaveChangesAsync();
                }


                var fine = new Auto_Fines
                {
                    IDPK_Fine = Guid.NewGuid(),
                    IDFK_Vehicle = vehicle?.IDPK_Vehicle,
                    IDFK_FineRecipient = recipient.IDPK_FineRecipient,
                    FineAmount = dto.FineAmount,
                    FineDate = dto.FineDate ?? DateTime.UtcNow,
                    FineReason = dto.FineReason,
                    PlateNumber = dto.PlateNumber, 
                    CreatedBy = policeId,
                    CreatedOn = DateTime.UtcNow,
                    CreatedIp = ip
                };

                await _repo.AddFineAsync(fine);
                await _repo.SaveChangesAsync();

                // dergoj njoftimin
                var notificationUserId = owner?.Id ?? recipient.IDFK_User;

                if (!string.IsNullOrEmpty(notificationUserId))
                {
                    var notification = new Auto_Notifications
                    {
                        IDPK_Notification = Guid.NewGuid(),
                        IDFK_Receiver = notificationUserId,
                        Title = "Fine",
                        Message = $"You have received a fine for the vehicle with plate: {dto.PlateNumber}.",
                        Type = NotificationType.FineIssued,
                        CreatedBy = policeId,
                        CreatedOn = DateTime.UtcNow,
                        CreatedIp = ip
                    };

                    await _notificationRepository.AddNotificationAsync(notification);
                    await _repo.SaveChangesAsync();

                    await NotificationConnections.SendNotificationToUserAsync(_notificationHubContext, notification, notificationUserId);
                }

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<PaginationResult<FineResponseDTO>> GetMyFinesAsync(string userId, FineFilterDTO filter)
        {
            var fines = await _repo.GetFinesForUserAsync(userId);

            DateTime? fromDateUtc = null;
            DateTime? toDateUtc = null;

            if (filter.FromDate.HasValue)
                fromDateUtc = DateTime.SpecifyKind(filter.FromDate.Value, DateTimeKind.Local).ToUniversalTime();

            if (filter.ToDate.HasValue)
                toDateUtc = DateTime.SpecifyKind(filter.ToDate.Value, DateTimeKind.Local).ToUniversalTime().AddDays(1).AddTicks(-1);

            bool validDateRange = true;
            if (fromDateUtc.HasValue && toDateUtc.HasValue)
                validDateRange = fromDateUtc <= toDateUtc;

            var filteredFines = fines
                .Where(f =>
                    (string.IsNullOrEmpty(filter.PlateNumber) || (f.Vehicle?.PlateNumber != null && f.Vehicle.PlateNumber.Contains(filter.PlateNumber))) &&
                    (!fromDateUtc.HasValue || f.FineDate >= fromDateUtc.Value) &&
                    (!toDateUtc.HasValue || f.FineDate <= toDateUtc.Value) &&
                    validDateRange
                )
                .ToList();

            var fineDtos = filteredFines.Select(f => new FineResponseDTO
            {
                IDPK_Fine = f.IDPK_Fine,
                FineAmount = f.FineAmount,
                FineReason = f.FineReason,
                FineDate = f.FineDate.ToLocalTime(),
                PlateNumber = f.PlateNumber,
                PoliceFullName = f.PoliceOfficer?.PersonalId ?? "-",
                RecipientFullName = f.FineRecipient != null ? $"{f.FineRecipient.FirstName} {f.FineRecipient.LastName}" : null
            }).ToList();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.ToLower();
                fineDtos = fineDtos.Where(f =>
                    (!string.IsNullOrEmpty(f.PlateNumber) && f.PlateNumber.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(f.RecipientFullName) && f.RecipientFullName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(f.PoliceFullName) && f.PoliceFullName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(f.FineReason) && f.FineReason.ToLower().Contains(searchLower)) ||
                    f.FineAmount.ToString().Contains(searchLower)
                ).ToList();
            }

            var helper = new PaginationHelper<FineResponseDTO>();
            return helper.GetPaginatedData(
                fineDtos,
                filter.Page,
                filter.PageSize,
                filter.SortField,
                filter.SortOrder
            );
        }


        public async Task<PaginationResult<FineResponseDTO>> GetFinesCreatedByPoliceAsync(string policeId, FineFilterDTO filter)
        {
            var fines = await _repo.GetFinesCreatedByPoliceAsync(policeId);

            DateTime? fromDateUtc = null;
            DateTime? toDateUtc = null;

            if (filter.FromDate.HasValue)
                fromDateUtc = DateTime.SpecifyKind(filter.FromDate.Value, DateTimeKind.Local).ToUniversalTime();

            if (filter.ToDate.HasValue)
                toDateUtc = DateTime.SpecifyKind(filter.ToDate.Value, DateTimeKind.Local).ToUniversalTime().AddDays(1).AddTicks(-1);

            bool validDateRange = true;
            if (fromDateUtc.HasValue && toDateUtc.HasValue)
                validDateRange = fromDateUtc <= toDateUtc;

            var filteredFines = fines
                .Where(f =>
                    (string.IsNullOrEmpty(filter.PlateNumber) || (f.Vehicle?.PlateNumber != null && f.Vehicle.PlateNumber.Contains(filter.PlateNumber))) &&
                    (!fromDateUtc.HasValue || f.FineDate >= fromDateUtc.Value) &&
                    (!toDateUtc.HasValue || f.FineDate <= toDateUtc.Value) &&
                    validDateRange
                )
                .ToList();

            var fineDtos = filteredFines.Select(f => new FineResponseDTO
            {
                IDPK_Fine = f.IDPK_Fine,
                FineAmount = f.FineAmount,
                FineReason = f.FineReason,
                FineDate = f.FineDate.ToLocalTime(),
                PlateNumber = f.PlateNumber,
                PoliceFullName = f.PoliceOfficer?.PersonalId ?? "-",
                RecipientFullName = f.FineRecipient != null ? $"{f.FineRecipient.FirstName} {f.FineRecipient.LastName}" : null
            }).ToList();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.ToLower();
                fineDtos = fineDtos.Where(f =>
                    (!string.IsNullOrEmpty(f.PlateNumber) && f.PlateNumber.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(f.RecipientFullName) && f.RecipientFullName.ToLower().Contains(searchLower)) ||
                    (!string.IsNullOrEmpty(f.FineReason) && f.FineReason.ToLower().Contains(searchLower)) ||
                    f.FineAmount.ToString().Contains(searchLower)
                ).ToList();
            }

            var helper = new PaginationHelper<FineResponseDTO>();
            return helper.GetPaginatedData(
                fineDtos,
                filter.Page,
                filter.PageSize,
                filter.SortField,
                filter.SortOrder
            );
        }

        public async Task<PaginationResult<FineResponseDTO>> GetAllFinesAsync(FineFilterDTO filter)
        {
            var query = _repo.QueryAllFines();

            if (filter.FromDate.HasValue)
            {
                var fromDateUtc = DateTime.SpecifyKind(filter.FromDate.Value, DateTimeKind.Local).ToUniversalTime();
                query = query.Where(f => f.FineDate >= fromDateUtc);
            }

            if (filter.ToDate.HasValue)
            {
                var toDateUtc = DateTime.SpecifyKind(filter.ToDate.Value, DateTimeKind.Local).ToUniversalTime().AddDays(1).AddTicks(-1);
                query = query.Where(f => f.FineDate <= toDateUtc);
            }

            if (!string.IsNullOrWhiteSpace(filter.PlateNumber))
            {
                query = query.Where(f => f.Vehicle != null && f.Vehicle.PlateNumber.Contains(filter.PlateNumber));
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.ToLower();
                query = query.Where(f =>
                    (f.PlateNumber != null && f.PlateNumber.ToLower().Contains(searchLower)) ||
                    (f.FineRecipient.FirstName + " " + f.FineRecipient.LastName).ToLower().Contains(searchLower) ||
                    (f.FineReason != null && f.FineReason.ToLower().Contains(searchLower)) ||
                    (f.PoliceOfficer.PersonalId != null && f.PoliceOfficer.PersonalId.ToLower().Contains(searchLower)) ||
                    f.FineAmount.ToString().Contains(searchLower)
                );
            }

            bool descending = filter.SortOrder?.ToLower() == "desc";
            query = filter.SortField?.ToLower() switch
            {
                "finedate" => descending ? query.OrderByDescending(f => f.FineDate) : query.OrderBy(f => f.FineDate),
                "fineamount" => descending ? query.OrderByDescending(f => f.FineAmount) : query.OrderBy(f => f.FineAmount),
                _ => descending ? query.OrderByDescending(f => f.CreatedOn) : query.OrderBy(f => f.CreatedOn),
            };

            var fineDtos = await query
                .Select(f => new FineResponseDTO
                {
                    IDPK_Fine = f.IDPK_Fine,
                    FineAmount = f.FineAmount,
                    FineReason = f.FineReason,
                    FineDate = f.FineDate.ToLocalTime(),
                    PlateNumber = f.PlateNumber,
                    PoliceFullName = f.PoliceOfficer != null && f.PoliceOfficer.PersonalId != null ? f.PoliceOfficer.PersonalId : "-",
                    RecipientFullName = f.FineRecipient != null ? f.FineRecipient.FirstName + " " + f.FineRecipient.LastName : null
                })
                .ToListAsync();

            var helper = new PaginationHelper<FineResponseDTO>();

            var sortOrder = filter.SortOrder ?? "asc";
            var sortField = filter.SortField ?? "CreatedOn";

            var result = helper.GetPaginatedData(
                fineDtos,
                filter.Page,
                filter.PageSize,
                sortField,
                sortOrder
            );

            return result;
        }

        public async Task<object?> GetRecipientDetailsByPlateAsync(string plate)
        {
            var vehicle = await _repo.GetVehicleByPlateAsync(plate);

            if (vehicle != null && vehicle.IDFK_Owner != null)
            {
                var owner = await _repo.GetUserByIdAsync(vehicle.IDFK_Owner);
                if (owner != null)
                {
                    return new
                    {
                        IsFrom = "Vehicle",
                        FirstName = owner.FirstName,
                        FatherName = owner.FatherName,
                        LastName = owner.LastName,
                        PersonalId = owner.PersonalId
                    };
                }
            }

            var recipient = await _repo.GetFineRecipientByPlateAsync(plate);
            if (recipient != null)
            {
                return new
                {
                    IsFrom = "FineRecipient",
                    FirstName = recipient.FirstName,
                    FatherName = recipient.FatherName,
                    LastName = recipient.LastName,
                    PersonalId = recipient.PersonalId
                };
            }

            return null;
        }

        public async Task<int> GetFinesCountAsync(string policeId)
        {
            return await _repo.CountFinesByPoliceAsync(policeId);
        }

        public async Task<int> GetFinesCountForUserAsync(string userId)
        {
            return await _repo.CountFinesForUserAsync(userId);
        }

    }
}
