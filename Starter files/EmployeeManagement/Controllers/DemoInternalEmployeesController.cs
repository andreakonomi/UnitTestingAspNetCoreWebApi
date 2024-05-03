using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers;

/// <summary>
/// This showcases when for some reason we dont want to have ApiController attribute applied to our controllers.
/// And we want to valdiate the behaviour of not valid input.
/// </summary>
[Route("api/demointernalemployees")]
public class DemoInternalEmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IMapper _mapper;

    public DemoInternalEmployeesController(IEmployeeService employeeService, IMapper mapper)
    {
        _employeeService = employeeService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<InternalEmployeeDto>> CreateInternalEmployee(
        InternalEmployeeForCreationDto internalEmployeeForCreation)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        // create an internal employee entity with default values filled out
        // and the values inputted via the POST request
        var internalEmployee =
            await _employeeService.CreateInternalEmployeeAsync(
                internalEmployeeForCreation.FirstName, internalEmployeeForCreation.LastName);

        // persist it
        await _employeeService.AddInternalEmployeeAsync(internalEmployee);
 
        // return created employee after mapping to a DTO
        return CreatedAtAction("GetInternalEmployee",
            _mapper.Map<InternalEmployeeDto>(internalEmployee),
            new { employeeId = internalEmployee.Id } );
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult GetProtectedInternalEmployees()
    {
        if (User.IsInRole("Admin"))
        {
            return RedirectToAction("GetInternalEmployees", "ProtectedInternalEmployees");
        }

        return RedirectToAction("GetInternalEmployees", "InternalEmployees");
    }
}