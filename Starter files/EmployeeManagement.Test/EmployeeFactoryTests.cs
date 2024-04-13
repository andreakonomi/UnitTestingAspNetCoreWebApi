using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;

namespace EmployeeManagement.Test;

public class EmployeeFactoryTests : IDisposable
{
    private EmployeeFactory _employeeFactory;

    public EmployeeFactoryTests()
    {
        // Creating shared context for each test (aka system under test)
        _employeeFactory = new EmployeeFactory();
    }
    
    public void Dispose()
    {
        // Here dispose the test context, if required
    }
    
    /// <summary>
    /// Assert the business behaviour that new internal employyes have salary 2500
    /// </summary>
    [Fact]      //Name of unit _ Scenario _ Expected Result
    public void CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500()
    {
        var employee = (InternalEmployee)_employeeFactory.CreateEmployee("John", "Doe");
        
        Assert.Equal(2500, employee.Salary);
        // You can also assert for floating values the no of precision to check to
        //Assert.Equal(2500, employee.Salary, 3);   //check until 3 precision digits
    }
    
    [Fact]
    public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500and3500()
    {
        var employee = (InternalEmployee)_employeeFactory.CreateEmployee("John", "Doe");
        
        // You can put the message to show when test fails
        Assert.True(employee.Salary is >= 2500 and <= 3500, "Salary is not within the allowed range");
        //Assert.InRange(employee.Salary, 2499, 3500);
    }

    [Fact]
    public void CreateEmployee_IsExternalIsTrue_ReturnTypeMustBeExternalEmployee()
    {
        // Act
        var employee = _employeeFactory.CreateEmployee("John", "Doe", "Marvin", true);

        // Assert
        Assert.IsType<ExternalEmployee>(employee); 
        
        // is employee of this type or derived types, same as the method of the reflection api, for types that implement interface
        //Assert.IsAssignableFrom<Employee>(employee);
    }
}