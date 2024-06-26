using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Test.Fixtures;
using EmployeeManagement.Test.TestData;

namespace EmployeeManagement.Test;

[Collection("EmployeeServiceCollection")]   // this groups it on the collection fixture with the same name
public class DataDrivenEmployeeServiceTests //: IClassFixture<EmployeeServiceFixture>
{
    private readonly EmployeeServiceFixture _employeeServiceFixture;

    public DataDrivenEmployeeServiceTests(EmployeeServiceFixture employeeServiceFixture)
    {
        _employeeServiceFixture = employeeServiceFixture;
    }
    
    /// <summary>
    /// Bus Rule: When a new employee comes onboard he goes through multiple obligatory courses.
    /// </summary>
    [Theory]
    [InlineData("37e03ca7-c730-4351-834c-b66f280cdb01")]
    [InlineData("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e")]
    public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedObligatoryCourse(Guid courseId)
    {
        // Arrange

        // Act
        var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("John", "Doe");

        // Assert
        Assert.Contains(internalEmployee.AttendedCourses, course => course.Id == courseId);
    }

    // These are member data, to share theory data with multiple tests in this class
    public static IEnumerable<object[]> ExampleTestDataForGiveRaise_WithProperty =>
        new List<object[]>
        {
            new object[] { 100, true },
            new object[] { 200, false }
        };

    public static IEnumerable<object[]> ExampleTestDataForGiveRaise_WithMethod(int testDataInstancesToProvide)
    {
        var testData = new List<object[]>
        {
            new object[] { 100, true },
            new object[] { 200, false }
        };
        return testData.Take(testDataInstancesToProvide);
    }
    
    public static TheoryData<int, bool> StronglyTypedExampleTestDataForGiveRaise_WithMethod()
    {
        return new TheoryData<int, bool>()
        { 
            { 100, true }, 
            { 200, false }
        };
    }
     
    
    [Theory]
    //[MemberData(nameof(ExampleTestDataForGiveRaise_WithProperty))]
    //[MemberData(nameof(ExampleTestDataForGiveRaise_WithMethod), 1)]
    // [MemberData(
    //     nameof(DataDrivenEmployeeServiceTests.ExampleTestDataForGiveRaise_WithMethod),  //this form allows you to share test data with other classes
    //     1,
    //     MemberType = typeof(DataDrivenEmployeeServiceTests))]
    //[ClassData(typeof(EmployeeServiceTestData))]  //this uses the untyped object[] version of class data
    //[ClassData(typeof(StronglyTypedEmployeeServiceTestData))]   //this works with strongly typed parameters on that other class
    //[MemberData(nameof(StronglyTypedExampleTestDataForGiveRaise_WithMethod))]   // strongly typed member data
    [ClassData(typeof(StronglyTypedEmployeeServiceTestData_FromFile))]
    public async Task GiveRaise_MinimumRaiseGiven_EmployeeMinimumRaiseGivenMatchesValue(
        int raiseGiven, bool expectedValueForMinimumRaiseGiven)
    {
        // Arrange
        var internalEmployee = new InternalEmployee(
            "John", "Doe", 5, 3000, false, 1);

        // Act
        await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, raiseGiven);

        // Assert
        Assert.Equal(expectedValueForMinimumRaiseGiven, internalEmployee.MinimumRaiseGiven);
    }
    
    [Fact]
    public async Task GiveRaise_MoreThenMinimumRaiseGiven_EmployeeMinimumRaiseGivenMustBeTrue()
    {
        // Arrange
        var internalEmployee = new InternalEmployee(
            "John", "Doe", 5, 3000, false, 1);

        // Act
        await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 200);

        // Assert
        Assert.False(internalEmployee.MinimumRaiseGiven);
    }
    
}