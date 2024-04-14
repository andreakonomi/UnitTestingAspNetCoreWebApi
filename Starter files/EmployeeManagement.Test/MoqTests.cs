using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Services.Test;
using Moq;

namespace EmployeeManagement.Test;

public class MoqTests
{
    [Fact]
    public void FetchInternalEmployee_EmployeeFetched_SuggestedBonusMustBeCalculated()
    {
        // Arrange
        var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        //var employeeFactory = new EmployeeFactory();
        var employeeFactoryMock = new Mock<EmployeeFactory>();  // if we see the method on act it doesnt use the employeeFactory so no need to mock specific actions
        var employeeService = new EmployeeService(employeeManagementTestDataRepository, employeeFactoryMock.Object);

        // Act
        var employee = employeeService.FetchInternalEmployee(Guid.Parse("72f2f5fe-e50c-4966-8420-d50258aefdcb"));

        // Assert
        Assert.Equal(400, employee.SuggestedBonus);
    }

    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_SuggestedBonusMustBeCalculated()
    {
        // Arrange
        var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        
        // Note: You can only mock overridable methods, abstract ones, contract methods from interfaces
        // the class, type should not be tied to a concretion in order to allow mocks to be wired (power of abstraction)
        // COUPLE WITH ABSTRACTIONS, NOT CONCRETIONS
        
        var employeeFactoryMock = new Mock<EmployeeFactory>();
        employeeFactoryMock.Setup(m => m.CreateEmployee(
                "John",    // this mock is returned if Kevin is passed as firstName 
                It.IsAny<string>(), // this mock is returned id any value of string is passed for lastname
                null,
                false))
            .Returns(new InternalEmployee("John", "Doe", 5, 2500, false, 1));
        
        employeeFactoryMock.Setup(m => m.CreateEmployee(
                "Sandy",
                It.IsAny<string>(),
                null,
                false))
            .Returns(new InternalEmployee("Sandy", "Doe", 0, 3000, false, 1));
        
        employeeFactoryMock.Setup(m => m.CreateEmployee(
                It.Is<string>(value => value.Contains('a')),    // more complex pattern matching
                It.IsAny<string>(),
                null,
                false))
            .Returns(new InternalEmployee("SomeoneWithAna", "Doe", 0, 3000, false, 1));

        
        var employeeService = new EmployeeService(employeeManagementTestDataRepository, employeeFactoryMock.Object);

        // suggested bonus for new employees =
        // (years in service if > 0) * attended courses * 100
        decimal suggestedBonus = 1000;
        
        // Act
        var employee = employeeService.CreateInternalEmployee("John", "Doe");
        // if this method passed in first name something different then John or Sandy then the above wired mocks would not be returned as the setup is not satisfied

        // Assert
        Assert.Equal(suggestedBonus, employee.SuggestedBonus);
    }
}