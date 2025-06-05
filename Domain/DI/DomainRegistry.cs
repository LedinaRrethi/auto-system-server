using DAL.Contracts;
using DAL.DI;
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