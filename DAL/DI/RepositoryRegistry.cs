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
            For<IAdminRepository>().Use<AdminRepository>();
            For<IDirectorateRepository>().Use<DirectorateRepository>();
            For<IVehicleRequestRepository>().Use<VehicleRequestRepository>();
            For<IAdminVehicleRequestRepository>().Use<AdminVehicleRequestRepository>();
            IncludeRegistry<UnitOfWorkRegistry>();

        }
    }
}