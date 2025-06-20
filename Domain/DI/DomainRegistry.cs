using DAL.Contracts;
using DAL.DI;
using Domain.Concrete;
using Domain.Contracts;
using Lamar;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.DI
{
    public class DomainRegistry : ServiceRegistry
    {
        public DomainRegistry()
        {
            IncludeRegistry<DomainUnitOfWorkRegistry>();
            For<IAuthDomain>().Use<AuthDomain>();
            For<IAdminDomain>().Use<AdminDomain>();
            For<IDirectorateDomain>().Use<DirectorateDomain>();
            For<IVehicleRequestDomain>().Use<VehicleRequestDomain>();
            For<IAdminVehicleRequestDomain>().Use<AdminVehicleRequestDomain>();
            For<IFineDomain>().Use<FineDomain>();
            For<IInspectionDomain>().Use<InspectionDomain>();
            

            AddRepositoryRegistries();
            AddHttpContextRegistries();
        }
        private void AddRepositoryRegistries()
        {
            IncludeRegistry<RepositoryRegistry>();
        }
        private void AddHttpContextRegistries()
        {
            For<IHttpContextAccessor>().Use<HttpContextAccessor>();
        }
    }
}