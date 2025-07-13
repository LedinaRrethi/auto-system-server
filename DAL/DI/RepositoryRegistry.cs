using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Concrete;
using DAL.Contracts;
using DAL.Repositories;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.DI
{
    public class RepositoryRegistry : ServiceRegistry
    {
        public RepositoryRegistry()
        {
            For(typeof(IRepository<>)).Use(typeof(BaseRepository<>));

            For<IAdminRepository>().Use<AdminRepository>();
            For<IDirectorateRepository>().Use<DirectorateRepository>();
            For<IVehicleRequestRepository>().Use<VehicleRequestRepository>();
            For<IAdminVehicleRequestRepository>().Use<AdminVehicleRequestRepository>();
            For<IFineRepository>().Use<FineRepository>();
            For<IInspectionRequestRepository>().Use<InspectionRequestRepository>();
            For<IInspectionRepository>().Use<InspectionRepository>();
            For<INotificationRepository>().Use<NotificationRepository>();
            For<IUserRepository>().Use<UserRepository>();
            IncludeRegistry<UnitOfWorkRegistry>();

        }
    }
}