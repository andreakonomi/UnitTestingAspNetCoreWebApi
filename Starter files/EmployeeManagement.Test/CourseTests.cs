using EmployeeManagement.DataAccess.Entities;

namespace EmployeeManagement.Test;

public class CourseTests
{
    /// <summary>
    /// Assert the unit of behaviour when a new course is created is considered a new course.
    /// </summary>
    [Fact]
    public void CourseConstructor_ConstructCourse_IsNewMustBeTrue()
    {
        // Arrange
        // nothing to arrange
        
        // Act
        var course = new Course("Disaster Management 101");
        
        // Assert
        Assert.True(course.IsNew);
    }
}