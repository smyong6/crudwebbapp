using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Service.Business;
using Service.Common.Interfaces;

namespace Service.Tests;

public abstract class TestBase
{
    protected IServiceProvider _serviceProvider;

    protected Guid TestClientId = Guid.NewGuid();

    protected TestBase()
    {
        var services = new ServiceCollection();

        services.AddDbContext<ContactDbContext>(c =>
        {
            c.UseInMemoryDatabase(databaseName: "Contact",
                b => b.EnableNullChecks(false));
            c.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            c.EnableSensitiveDataLogging(true);
        });

        services.AddScoped<IContactBusiness, ContactBusiness>();

        _serviceProvider = services.BuildServiceProvider();
    }
}
