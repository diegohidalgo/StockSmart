using Autofac;
using StockSmart.Application.Common.Abstract;
using StockSmart.Application.Services;
using StockSmart.Domain.Common.Abstract;
using StockSmart.Infrastructure.Repositories;
using StockSmart.Infrastructure.Services;
using Module = Autofac.Module;

namespace StockSmart.WebApp.Modules
{
    public class ApiAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Application.AssemblyReference).Assembly)
                .Where(t => typeof(IMapper).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Domain.AssemblyReference).Assembly, typeof(Infrastructure.AssemblyReference).Assembly)
                .AsClosedTypesOf(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterDecorator<CachedStatusRepository, IStatusRepository>();

            builder.RegisterType<DiscountService>()
                .As<IDiscountService>()
                .InstancePerDependency();
        }
    }
}
