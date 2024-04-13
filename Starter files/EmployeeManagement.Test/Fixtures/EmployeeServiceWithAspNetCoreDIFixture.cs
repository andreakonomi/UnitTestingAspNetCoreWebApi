using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Services;
using EmployeeManagement.Services.Test;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Test.Fixtures;

/// <summary>
/// Sets up a DI container for all the dependencies required so it can serve as a fixture. It exposes properties of class types
/// by requesting that type from the DI container. This is very rarely used, but it may be useful if you are facing a big dependency tree for your classes
/// </summary>
public class EmployeeServiceWithAspNetCoreDIFixture
{
    private readonly ServiceProvider _serviceProvider;

    public IEmployeeManagementRepository EmployeeManagementRepository =>
        _serviceProvider.GetService<IEmployeeManagementRepository>()!;

    public IEmployeeService EmployeeService => _serviceProvider.GetService<IEmployeeService>()!;

    public EmployeeServiceWithAspNetCoreDIFixture()
    {
        var services = new ServiceCollection();
        services.AddScoped<EmployeeFactory>();
        services.AddScoped<IEmployeeManagementRepository, EmployeeManagementTestDataRepository>();
        services.AddScoped<IEmployeeService, EmployeeService>();

        _serviceProvider = services.BuildServiceProvider();
    }
}