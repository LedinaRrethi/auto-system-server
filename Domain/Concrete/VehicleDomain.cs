//using AutoMapper;
//using DAL.Contracts;
//using DAL.UoW;
//using DTO.VehicleDTO;
//using Entities.Models;
//using Helpers.Enumerations;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Domain.Contracts;
//using DTO.VehicleRequest;

//namespace Domain.Concrete
//{
//    public class VehicleRequestDomain : IVehicleRequestDomain
//    {
//        private readonly IVehicleRequestRepository _vehicleRequestRepository;
//        private readonly AutoSystemDbContext _context;

//        public VehicleRequestDomain(IVehicleRequestRepository vehicleRequestRepository, AutoSystemDbContext context)
//        {
//            _vehicleRequestRepository = vehicleRequestRepository;
//            _context = context;
//        }

//        public async Task RegisterVehicleAsync(VehicleRequestDTO dto, string userId)
//        {
//            using var transaction = await _context.Database.BeginTransactionAsync();
//            try
//            {
//                var vehicle = new Auto_Vehicles
//                {
//                    IDPK_Vehicle = Guid.NewGuid(),
//                    IDFK_Owner = userId,
//                    Plate = "TEMP", // do zëvendësohet nga JSON  
//                    Color = "TEMP",
//                    Doors = 0,
//                    Seats = 0,
//                    Chassis = "TEMP",
//                    Status = VehicleStatus.Pending,
//                    Invalidated = 0,
//                    CreatedBy = userId,
//                    CreatedOn = DateTime.UtcNow
//                };

//                // Optional: deserializo DTO për të plotësuar më shumë fusha nga JSON  
//                await _vehicleRequestRepository.AddVehicleAsync(vehicle);

//                var request = new Auto_VehicleChangeRequests
//                {
//                    IDPK_ChangeRequest = Guid.NewGuid(),
//                    IDFK_Vehicle = vehicle.IDPK_Vehicle,
//                    IDFK_Requester = userId,
//                    RequestType = ChangeRequestType.Register,
//                    RequestDataJson = dto.RequestDataJson,
//                    CurrentDataSnapshotJson = "",
//                    Status = ChangeRequestStatus.Pending,
//                    RequesterComment = dto.RequesterComment,
//                    CreatedBy = userId,
//                    CreatedOn = DateTime.UtcNow
//                };

//                await _vehicleRequestRepository.AddRequestAsync(request);
//                await _context.SaveChangesAsync();
//                await transaction.CommitAsync();
//            }
//            catch
//            {
//                await transaction.RollbackAsync();
//                throw;
//            }
//        }

//        public async Task<List<Auto_VehicleChangeRequests>> GetMyRequestsAsync(string userId)
//        {
//            return await _vehicleRequestRepository.GetRequestsByUserAsync(userId);
//        }
//    }
//}
