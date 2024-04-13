using EmployeeManagement.Business;
using EmployeeManagement.Test.Fixtures;

namespace EmployeeManagement.Test;

public class EmployeeServiceTestsWithAspnetCoreDI : IClassFixture<EmployeeServiceWithAspNetCoreDIFixture>
{
    private readonly EmployeeServiceWithAspNetCoreDIFixture _employeeServiceFixture;

    public EmployeeServiceTestsWithAspnetCoreDI(EmployeeServiceWithAspNetCoreDIFixture employeeServiceFixture)
    {
        _employeeServiceFixture = employeeServiceFixture;
    }

    /// <summary>
    /// Bus Rule: When a new employee comes onboard he goes through multiple obligatory courses.
    /// </summary>
    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse()
    {
        // Arrange
        // id below is of the first obligatory course
        var obligatoryCourse = _employeeServiceFixture.EmployeeManagementRepository
            .GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));

        // Act
        var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("John", "Doe");

        // Assert
        Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
    }
}