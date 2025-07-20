// using Real_State.Modules;
// using Real_State.Data;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Mvc.RazorPages;

// [ApiController]
// [Route("[controller]")]
// public class LoanController : ControllerBase
// {
//     private readonly ApplicationDbContext _context;

//     public LoanController(ApplicationDbContext context)
//     {
//         _context = context;
//     }

//     [HttpPost("AddLoan")]
//     public async Task<IActionResult> AddNewLoan([FromBody] LoanModule loan)
//     {
//         await _context.LoanTable.AddAsync(loan);
//         await _context.SaveChangesAsync();
//         return Ok("Loan added successfully.");
//     }

//     [Authorize]
//     [HttpGet("GetAllLoans")]
//     public async Task<IActionResult> GetAllLoan(int Page= 1 , int pageSize =10)
//     {
//         var loans = await _context.LoanTable
//         .Skip((Page - 1)*pageSize)
//         .Take(pageSize)
//         .ToListAsync();
//         return Ok(loans);
//     }

//     [Authorize]
//     [HttpGet("GetLoan/{id}")]
//     public async Task<IActionResult> GetLoanById(int id)
//     {
//         var loan = await _context.LoanTable.FindAsync(id);
//         if (loan == null)
//         {
//             return NotFound($"Loan with ID {id} not found.");
//         }
//         return Ok(loan);
//     }

//     [Authorize]
//     [HttpPut("UpdateLoan/{id}")]
//     public async Task<IActionResult> UpdateLoan(int id, [FromBody] LoanModule updatedLoan)
//     {
//         var loan = await _context.LoanTable.FindAsync(id);
//         if (loan == null)
//         {
//             return NotFound($"Loan with ID {id} not found.");
//         }

//         loan.Loan_Type = updatedLoan.Loan_Type;
//         loan.LoanName = updatedLoan.LoanName;
//         loan.Budget = updatedLoan.Budget;
//         loan.Currency = updatedLoan.Currency;
//         await _context.SaveChangesAsync();
//         return Ok("Loan updated successfully.");
//     }

//     [Authorize]
//     [HttpDelete("DeleteLoan/{id}")]
//     public async Task<IActionResult> DeleteLoan(int id)
//     {
//         var loan = await _context.LoanTable.FindAsync(id);
//         if (loan == null)
//         {
//             return NotFound($"Loan with ID {id} not found.");
//         }

//         _context.LoanTable.Remove(loan);
//         await _context.SaveChangesAsync();
//         return Ok("Loan deleted successfully.");
//     }
// }
