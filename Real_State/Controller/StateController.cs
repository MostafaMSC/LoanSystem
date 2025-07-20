using Real_State.Modules;
using Real_State.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class StateController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StateController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("AddState")]
    public async Task<IActionResult> AddNewState([FromBody] StateModule State)
    {
        await _context.StatesTable.AddAsync(State);
        await _context.SaveChangesAsync();
        return Ok("State added successfully.");
    }

    [Authorize]
    [HttpGet("GetAllStates")]
    public async Task<IActionResult> GetAllStates( int page = 1 , int pageSize = 10)
    {
        var Contracts = await _context.StatesTable
        .Skip((page - 1 )* pageSize)
        .Take(pageSize)
        .ToListAsync();
        return Ok(Contracts);
    }
    [Authorize]
    [HttpGet("GetAllStatesByUserId")]
    public async Task<IActionResult> GetAllStatesByUserId(int OwnerId, int page = 1 , int pageSize = 10)
    {
        var State = await _context.StatesTable
        .Where(a => a.OwnerId == OwnerId)
        .Skip((page - 1 ) * pageSize)
        .Take(pageSize)
        .ToListAsync();
        return Ok(State);
    }

    [Authorize]
    [HttpGet("GetState/{id}")]
    public async Task<IActionResult> GetStateById(int id)
    {
        var loan = await _context.StatesTable.FindAsync(id);
        if (loan == null)
        {
            return NotFound($"State with ID {id} not found.");
        }
        return Ok(loan);
    }

    [Authorize]
    [HttpPut("UpdateState/{id}")]
    public async Task<IActionResult> UpdateState(int id, [FromBody] StateModule updatedState)
    {
        var State = await _context.StatesTable.FindAsync(id);
        if (State == null)
        {
            return NotFound($"State with ID {id} not found.");
        }

        State.StateName = updatedState.StateName;
        State.Description = updatedState.Description;
        State.StateType = updatedState.StateType;
        State.City = updatedState.City;
        State.Location = updatedState.Location;
        State.Price = updatedState.Price;
        State.PublishedDate = updatedState.PublishedDate;
        // Contract.EndDate = updatedContracte.EndDate;
        State.Status =  updatedState.Status;
        await _context.SaveChangesAsync();
        return Ok("State updated successfully.");
    }

    [Authorize]
    [HttpDelete("DeleteState/{id}")]
    public async Task<IActionResult> StateLoan(int id)
    {
        var loan = await _context.StatesTable.FindAsync(id);
        if (loan == null)
        {
            return NotFound($"State with ID {id} not found.");
        }

        _context.StatesTable.Remove(loan);
        await _context.SaveChangesAsync();
        return Ok("State deleted successfully.");
    }
}
