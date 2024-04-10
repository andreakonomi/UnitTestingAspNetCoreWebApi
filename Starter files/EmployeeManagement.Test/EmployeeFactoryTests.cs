using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;

namespace EmployeeManagement.Test;

public class EmployeeFactoryTests
{
    [Fact]
    public void CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500()
    {
        var employeeFactory = new EmployeeFactory();

        var employee = (InternalEmployee)employeeFactory.CreateEmployee("John", "Doe");
        
        Assert.Equal(2500, employee.Salary);
    }
}