﻿using DAL.DI;
using Domain.Concrete;
using Domain.Contracts;
using Domain.Notifications;
using Lamar;
using Microsoft.AspNetCore.Http;

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
            For<IInspectionRequestDomain>().Use<InspectionRequestDomain>();
            For<IInspectionDomain>().Use<InspectionDomain>();
            For<IUserDomain>().Use<UserDomain>();

            For<INotificationDomain>().Use<NotificationDomain>();

            For<NotificationHub>();

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