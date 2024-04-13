using EmployeeManagement.DataAccess.Entities;

namespace EmployeeManagement.Test;

public class EmployeeTests
{
    [Fact]
    public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameIsConcatenation()
    {
        // Arrange
        var employee = new InternalEmployee("John", "Doe", 0, 2500, false, 1);
        
        // Act
        employee.FirstName = "Kevin";
        employee.LastName = "Durant";
        
        // Assert
        Assert.Equal("Kevin Durant", employee.FullName, ignoreCase: false);
        
        // How to use regex match
        //Assert.Matches("regex expression", "value");
    }
    
    // Note: the below tests are just showcases of string assertions, the first one covers all of them imo
    
    [Fact]
    public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameStartsWithFirstName()
    {
        // Arrange
        var employee = new InternalEmployee("John", "Doe", 0, 2500, false, 1);
        
        // Act
        employee.FirstName = "Kevin";
        employee.LastName = "Durant";
        
        // Assert
        Assert.StartsWith(employee.FirstName, employee.FullName);
    }
    
    [Fact]
    public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameEndsWithLastName()
    {
        // Arrange
        var employee = new InternalEmployee("John", "Doe", 0, 2500, false, 1);
        
        // Act
        employee.FirstName = "Kevin";
        employee.LastName = "Durant";
        
        // Assert
        Assert.EndsWith(employee.LastName, employee.FullName);
    }
    
    [Fact]
    public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameContainsPartOfConcatenation()
    {
        // Arrange
        var employee = new InternalEmployee("John", "Doe", 0, 2500, false, 1);
        
        // Act
        employee.FirstName = "Kevin";
        employee.LastName = "Durant";
        
        // Assert
        Assert.Contains("in Du", employee.FullName);
    }
    
} 