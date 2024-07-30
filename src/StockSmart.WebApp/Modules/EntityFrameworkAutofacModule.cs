using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StockSmart.Domain.Common.Abstract;
using StockSmart.Infrastructure;
using Module = Autofac.Module;

namespace StockSmart.WebApp.Modules;

public class EntityFrameworkAutofacModule(IConfiguration configuration) : Module
{
    private readonly IConfiguration _configuration = configuration;

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(x =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder
                .UseSqlServer(_configuration.GetConnectionString("Database"));
            return new AppDbContext(optionsBuilder.Options);
        }).InstancePerLifetimeScope();

        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerDependency();

    }
}
