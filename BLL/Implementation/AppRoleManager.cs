using System;
using DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace BLL.Implementation
{
    public class AppRoleManager : RoleManager<IdentityRole>, IDisposable
    {
        public AppRoleManager(RoleStore<IdentityRole> store)
            : base(store)
        { }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            ApplicationContext db = context.Get<ApplicationContext>();
            AppRoleManager manager = new AppRoleManager(new RoleStore<IdentityRole>(db));
            return manager;
        }
    }
}
