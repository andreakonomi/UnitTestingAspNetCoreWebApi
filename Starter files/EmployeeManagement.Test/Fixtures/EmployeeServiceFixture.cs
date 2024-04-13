using EmployeeManagement.Business;
using EmployeeManagement.Services.Test;

namespace EmployeeManagement.Test.Fixtures;

/// <summary>
/// Creating a test context fixture class for all tests of a class to reuse, if a dependency includes a cost heavy initialization operation and
/// we dont want to new it up on every test. In this case EmployeeManagementTestDataRepository is that class.
/// </summary>
public class EmployeeServiceFixture : IDisposable
{
    public EmployeeManagementTestDataRepository EmployeeManagementTestDataRepository { get;  }
    public EmployeeService EmployeeService { get;  }

    public EmployeeServiceFixture()
    {
        EmployeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        EmployeeService = new EmployeeService(EmployeeManagementTestDataRepository, new EmployeeFactory());
    }

    public void Dispose()
    {
        // clean up if required
    }
}