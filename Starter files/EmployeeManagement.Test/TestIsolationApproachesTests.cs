using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.DbContexts;
using EmployeeManagement.DataAccess.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;

namespace EmployeeManagement.Test;

public class TestIsolationApproachesTests
{
    // BL: Employees can attend courses, the more courses they atend their potential bonus is increased
    [Fact]
    public async Task AttendCourseAsync_CourseAttended_SuggestedBonusMustBeSuccessfullyRecalculated()
    {
        // Arrange
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var optionsBuilder = new DbContextOptionsBuilder<EmployeeDbContext>()
            .UseSqlite(connection);

        var dbContext = new EmployeeDbContext(optionsBuilder.Options);
        dbContext.Database.Migrate();   // ensure the in memory database is created and migrations are executed

        var employeesRepository = new EmployeeManagementRepository(dbContext);
        var employeesService = new EmployeeService(employeesRepository, new EmployeeFactory());

        // get course Dealing with Customers 101. Note: Seed data are included on the db context
        var courseToAttend =
            await employeesRepository.GetCourseAsync(Guid.Parse("844e14ce-c055-49e9-9610-855669c9859b"));
        
        // get existing employee Megan Jones
        var employee =
            await employeesRepository.GetInternalEmployeeAsync(Guid.Parse("72f2f5fe-e50c-4966-8420-d50258aefdcb"));

        if (courseToAttend is null || employee is null)
        {
            throw new XunitException("Arranging the test failed");
        }

        // expected suggested bonus after attending the course
        var expectedSuggestedBonus = employee.YearsInService * (employee.AttendedCourses.Count + 1) * 100;

        // Act
        await employeesService.AttendCourseAsync(employee, courseToAttend);

        // Assert
        Assert.Equal(expectedSuggestedBonus, employee.SuggestedBonus);
    }
}