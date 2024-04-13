using EmployeeManagement.Business;
using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Services.Test;
using EmployeeManagement.Test.Fixtures;

namespace EmployeeManagement.Test;

// XUnit supplies on the constructor of the first test a new instance of the fixture provided by the interface
// and every other test gets the same instance. After the last test the fixture is disposed.
public class EmployeeServiceTests : IClassFixture<EmployeeServiceFixture>
{
    private readonly EmployeeServiceFixture _employeeServiceFixture;
    
    public EmployeeServiceTests(EmployeeServiceFixture employeeServiceFixture)
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
        var obligatoryCourse = _employeeServiceFixture.EmployeeManagementTestDataRepository
            .GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));

        // Act
        var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("John", "Doe");

        // Assert
        Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
    }
    
    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse_WithPredicate()
    {
        // Act
        var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("John", "Doe");

        // Assert
        Assert.Contains(internalEmployee.AttendedCourses, 
            course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
    }
    
    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMatchObligatoryCourses()
    {
        // Arrange
        var obligatoryCourses = _employeeServiceFixture.EmployeeManagementTestDataRepository
            .GetCourses(
            Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
            Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        
        // Act
        var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("John", "Doe");

        // Assert
        Assert.Equal(obligatoryCourses, internalEmployee.AttendedCourses);
    }
    
    /// <summary>
    /// Bus rule: Obligatory courses should have been around sometime to get validated and verified.
    /// </summary>
    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustNotBeNew()
    {
        // Act
        var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("John", "Doe");

        // Assert
        // foreach (var course in internalEmployee.AttendedCourses)
        // {
        //     Assert.False(course.IsNew);
        // }
        Assert.All(internalEmployee.AttendedCourses, course => Assert.False(course.IsNew));
    }
    
    [Fact]
    public async Task CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse_Async()
    {
        // Arrange
        // id below is of the first obligatory course
        var obligatoryCourse = await _employeeServiceFixture.EmployeeManagementTestDataRepository
            .GetCourseAsync(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));

        // Act
        var internalEmployee = await _employeeServiceFixture.EmployeeService.CreateInternalEmployeeAsync("John", "Doe");

        // Assert
        Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
    }

    /// <summary>
    /// BR: An internal employee cant be given a minimum raise two times in a row. If occured throws exception.
    /// Minimum raise is 100, if given raise below that throws exception.
    /// </summary>
    [Fact]
    public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown()
    {
        // Arrange
        // Note: We just new up the internal employee and not through factory because we want to keep the test focused as much as posible
        // and not touch parts that are not related to our test case
        var internalEmployee = new InternalEmployee("John", "Doe", 5, 3000, false, 1);

        // Act and Assert
        await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(
            async () => await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 50)
        );
    }

    /// <summary>
    /// BR: When an employee is absent and is marked an event should be fired for it
    /// </summary>
    [Fact]
    public void NotifyOgAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentMustBeTriggered()
    {
        // Arrange
        var internalEmployee = new InternalEmployee("John", "Doe", 5, 3000, false, 1);

        // Act & Assert
        Assert.Raises<EmployeeIsAbsentEventArgs>(
            handler => _employeeServiceFixture.EmployeeService.EmployeeIsAbsent += handler, // how to attach to the event
            handler => _employeeServiceFixture.EmployeeService.EmployeeIsAbsent -= handler, // how to detach from event
            () => _employeeServiceFixture.EmployeeService.NotifyOfAbsence(internalEmployee));   // how to trigger the event
    }
    
}