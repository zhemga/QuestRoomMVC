using Autofac;
using Autofac.Integration.Mvc;
using BLL.Interfaces;
using BLL.Implementation;
using DAL;
using GameStore.DAL.Repository;
using GameStore.DAL.Repository.Interface;
using System.Data.Entity;
using AutoMapper;
using System.Web.Mvc;

namespace UI.Utils
{
    public class AutofacConfiguration
    {
        public static void Configurate()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<ApplicationContext>().As<DbContext>();
            builder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IGenericRepository<>));
            builder.RegisterType<RoomService>().As<IRoomService>();

            var configurationManager = new MapperConfiguration(cfg => cfg.AddProfile(new AutomapperConfiguration()));
            builder.RegisterInstance(configurationManager.CreateMapper());

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}