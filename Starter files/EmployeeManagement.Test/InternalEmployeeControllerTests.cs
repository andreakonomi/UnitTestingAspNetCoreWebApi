using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManagement.Test;

public class InternalEmployeeControllerTests
{
    private readonly InternalEmployeesController _sut;
    private readonly InternalEmployee _firstEmployee;
    
    public InternalEmployeeControllerTests()
    {
        _firstEmployee = new InternalEmployee("Megan", "Jones", 2, 3000, false, 2)
        {
            Id = Guid.Parse("72f2f5fe-e50c-4966-8420-d50258aefdcb"),
            SuggestedBonus = 400
        };
        
        var employeeServiceMock = new Mock<IEmployeeService>();
        employeeServiceMock
            .Setup(m => m.FetchInternalEmployeesAsync())
            .ReturnsAsync(new List<InternalEmployee>()
            {
                _firstEmployee,
                new InternalEmployee("Jaimy", "Johnson", 3, 3400, true, 1),
                new InternalEmployee("Anne", "Adams", 3, 4000, false, 3),
            });

        // var mapperMock = new Mock<IMapper>();
        // mapperMock.Setup(m =>
        //         m.Map<InternalEmployee, Models.InternalEmployeeDto>
        //             (It.IsAny<InternalEmployee>()))
        //     .Returns(new Models.InternalEmployeeDto());
        var mapperConfiguration = new MapperConfiguration(
            cfg => cfg.AddProfile<MapperProfiles.EmployeeProfile>());
        var mapper = new Mapper(mapperConfiguration);

        _sut = new InternalEmployeesController(
            employeeServiceMock.Object, mapper);
    }
    
    [Fact]
    public async Task GetInternalEmployees_GetAction_MustReturnOkObjectResult()
    {
        // Arrange
        
        
        // Act
        var result = await _sut.GetInternalEmployees();
        
        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.InternalEmployeeDto>>>(result);
        Assert.IsType<OkObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task GetInternalEmployees_GetAction_MustReturnIEnumerableOfInternalEmployeeDtoAsModelType()
    {
        // Arrange
        
        // Act
        var result = await _sut.GetInternalEmployees();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.InternalEmployeeDto>>>(result);

        Assert.IsAssignableFrom<IEnumerable<Models.InternalEmployeeDto>>(
            ((OkObjectResult)actionResult.Result).Value);
    }

    [Fact]
    public async Task GetInternalEmployees_GetAction_MustReturnNumberOfInputtedInternalEmployees()
    {
        // Arrange
        
        // Act
        var result = await _sut.GetInternalEmployees();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.InternalEmployeeDto>>>(result);
        
        Assert.Equal(3, 
            ((IEnumerable<Models.InternalEmployeeDto>)
                ((OkObjectResult)actionResult.Result).Value).Count());
    }

    /// <summary>
    /// This controller combines all tests from other methods above as it is resonable in this case to treat as one unit
    /// the whole controller action and test it on one method if the assertions come from the same flow
    /// </summary>
    [Fact]
    public async Task GetInternalEmployees_GetAction_ReturnsOkObjectResultWithCorrectAmountOfInternalEmployees()
    {
        // Arrange
        
        // Act
        var result = await _sut.GetInternalEmployees();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.InternalEmployeeDto>>>(result);    //check type
        // Assert.IsType<OkObjectResult>(actionResult.Result);
        // Assert.IsAssignableFrom<IEnumerable<Models.InternalEmployeeDto>>(
        //     ((OkObjectResult)actionResult.Result).Value);
        //
        // Assert.Equal(3, 
        //     ((IEnumerable<Models.InternalEmployeeDto>)
        //         ((OkObjectResult)actionResult.Result).Value).Count());
        
        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);    // check ok response
        var dtos = Assert.IsAssignableFrom<IEnumerable<Models.InternalEmployeeDto>>(okObjectResult.Value);  // check value is wanted type
        Assert.Equal(3, dtos.Count());  // check content of value in response
        
        // Test mapping of fields
        var firstEmployee = dtos.First();
        Assert.Equal(_firstEmployee.Id, firstEmployee.Id);
        Assert.Equal(_firstEmployee.FirstName, firstEmployee.FirstName);
        Assert.Equal(_firstEmployee.LastName, firstEmployee.LastName);
        Assert.Equal(_firstEmployee.Salary, firstEmployee.Salary);
        Assert.Equal(_firstEmployee.SuggestedBonus, firstEmployee.SuggestedBonus);
        Assert.Equal(_firstEmployee.YearsInService, firstEmployee.YearsInService);
    }
}