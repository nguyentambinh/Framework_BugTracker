using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using BugTracker.Core.Interfaces;
using BugTracker.Data.Context;
using BugTracker.Data.Repositories;
using BugTracker.Service.Services;
using BugTracker.Infrastructure;
using Unity.Lifetime;
using BugTracker.Data.Services;

namespace BugTracker.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<BugTrackerDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IAuthService, AuthService>();
            container.RegisterType<IUserContext, WebUserContext>();
            container.RegisterType<ISecurityLogger, SecurityLogger>();
            container.RegisterType<IBugService, BugService>();
            container.RegisterType<IPermissionService, PermissionService>();
            container.RegisterType<IUserPermissionManager, UserPermissionManager>(new HierarchicalLifetimeManager());
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}