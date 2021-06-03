using DAL;
using DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace BLL.Implementation
{
    public class AppUserManager : UserManager<User>
    {
        public AppUserManager(IUserStore<User> store)
            : base(store)
        { }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options,
            IOwinContext context)
        {
            ApplicationContext db = context.Get<ApplicationContext>();
            AppUserManager manager = new AppUserManager(new UserStore<User>(db));
            return manager;
        }
    }
}