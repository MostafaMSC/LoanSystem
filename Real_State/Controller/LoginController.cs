using Microsoft.AspNetCore.Mvc;
using Real_State.Modules;
using Real_State.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; // <-- Add this for async database operations

namespace Loan_System.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("AllUser")]
        public async Task<IActionResult> GetAllUsers()
        {
            var AllUSers = await _context.UsersTable.ToListAsync();
            return Ok(AllUSers);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModule request)
                {
                // Check if user already exists
                var checkUser = await _context.UsersTable
                    .FirstOrDefaultAsync(u => u.Username == request.Username);

                if (checkUser == null)
                {
                    var newUser = new RegisterModule
                    {
                        Username = request.Username,
                        Password = request.Password,
                        PhonNumber = request.PhonNumber,
                        Role = request.Role ?? "User" 

                    };

                    await _context.UsersTable.AddAsync(newUser);
                    await _context.SaveChangesAsync(); 

                    return Ok(new { message = "User registered successfully!" });
                }

                return Unauthorized("User already exists.");
            }
        [Authorize(Roles = "Admin")]
        [HttpPut("EditUser/{id}")]
        public async Task<IActionResult> EditUser(int id, [FromBody] RegisterModule updatedUser)
        {
            var user = await _context.UsersTable.FindAsync(id);
            if (user == null)
                return NotFound("User not found");

            user.Username = updatedUser.Username;
            user.Password = updatedUser.Password;
            user.Role = updatedUser.Role;

            await _context.SaveChangesAsync();
            return Ok("User updated successfully.");
        }

[HttpPost("ForgotPassword")]
public async Task<IActionResult> ForgotPassword([FromBody] RegisterModule request)
{
    var user = await _context.UsersTable.FirstOrDefaultAsync(u => u.Username == request.Username);
    if (user == null)
        return NotFound("User not found");

    user.Password = request.Password;
    await _context.SaveChangesAsync();

    return Ok("Password reset successful.");
}

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RegisterModule request)
        {
            var checkUser = await _context.UsersTable
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

            if (checkUser != null)
            {
                var token = GenerateJwtToken(checkUser);
                return Ok(new { token });
            }

            return Unauthorized("Invalid credentials");
        }

        private string GenerateJwtToken(RegisterModule user)
{
    var key = "LoanSystem_SecretKey@2025!LongKey32Chars";

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role ?? "User")
    };

    var token = new JwtSecurityToken(
        issuer: "http://localhost:5109/",
        audience: "http://localhost:5109/",
        claims: claims,
        expires: DateTime.Now.AddHours(24),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}


    }
    
    }
