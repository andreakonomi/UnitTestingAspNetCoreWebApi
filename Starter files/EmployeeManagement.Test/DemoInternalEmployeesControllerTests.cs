using System.Security.Claims;
using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManagement.Test;

public class DemoInternalEmployeesControllerTests
{
    [Fact]
    public async Task CreateInternalEmployee_InvalidInput_MustReturnBadRequest()
    {
        // Arrange
        var employeeServiceMock = new Mock<IEmployeeService>();
        var mapperMock = new Mock<IMapper>();
        var demoInternalEmployeesController = new DemoInternalEmployeesController(
            employeeServiceMock.Object, mapperMock.Object);

        var internalEmployeeForCreationDto = new InternalEmployeeForCreationDto();

        // as the model state is made invalid during the process of model binding from json,xml.. our tests wont trigger it, but we can do this
        demoInternalEmployeesController.ModelState.AddModelError("FirstName", "Required");
        
        // Act
        var result = await demoInternalEmployeesController.CreateInternalEmployee(internalEmployeeForCreationDto);

        // Assert
        var actionResult = Assert.IsType<ActionResult<InternalEmployeeDto>>(result);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    [Fact]
    public void
        GetProtectedInternalEmployees_GetActionForUserInAdminRole_MustRedirectToGetInternalEmployeesOnProtectedInternalEmployees()
    {
        // Arrange
        var employeeServiceMock = new Mock<IEmployeeService>();
        var mapperMock = new Mock<IMapper>();
        var demoInternalEmployeesController = new DemoInternalEmployeesController(
            employeeServiceMock.Object, mapperMock.Object);

        var userClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, "Karen"),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var claimsIdentity = new ClaimsIdentity(userClaims, "UnitTest");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var httpContext = new DefaultHttpContext()
        {
            User = claimsPrincipal
        };

        demoInternalEmployeesController.ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                };

        // Act
        var result = demoInternalEmployeesController.GetProtectedInternalEmployees();
        
        // Assert
        var action = Assert.IsAssignableFrom<IActionResult>(result);
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        
        Assert.Equal("GetInternalEmployees", redirectToActionResult.ActionName);
        Assert.Equal("ProtectedInternalEmployees", redirectToActionResult.ControllerName);
    }
    
    [Fact]
    public void
        GetProtectedInternalEmployees_GetActionForUserInAdminRole_MustRedirectToGetInternalEmployeesOnProtectedInternalEmployees_WithMoq()
    {
        // Arrange
        var employeeServiceMock = new Mock<IEmployeeService>();
        var mapperMock = new Mock<IMapper>();
        var demoInternalEmployeesController = new DemoInternalEmployeesController(
            employeeServiceMock.Object, mapperMock.Object);

        var mockPrincipal = new Mock<ClaimsPrincipal>();
        mockPrincipal.Setup(x =>
            x.IsInRole(It.Is<string>(s => s == "Admin"))).Returns(true);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User)
            .Returns(mockPrincipal.Object);
        
        demoInternalEmployeesController.ControllerContext = new ControllerContext()
        {
            HttpContext = httpContextMock.Object
        };

        // Act
        var result = demoInternalEmployeesController.GetProtectedInternalEmployees();
        
        // Assert
        var action = Assert.IsAssignableFrom<IActionResult>(result);
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        
        Assert.Equal("GetInternalEmployees", redirectToActionResult.ActionName);
        Assert.Equal("ProtectedInternalEmployees", redirectToActionResult.ControllerName);
    }
}