using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VID.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VID.Data;
using VID.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase{

    private readonly ApplicationDbContext _context;
    private readonly  IConfiguration _configuration;
    private readonly PasswordResetService _passwordResetService;

    private readonly ILogger<AuthController> _logger;

    //private readonly IEmailSender _emailSender;

    public AuthController(ApplicationDbContext context, IConfiguration configuration, PasswordResetService passwordResetService, ILogger<AuthController> logger )
    {
        _context = context;
        _configuration = configuration;

        _passwordResetService = passwordResetService;

        _logger = logger;

        //_emailSender = emailSender;
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        
        var result = await _passwordResetService.ForgotPassword(dto);
        return result 
            ? Ok(new { message = "OTP sent successfully" }) 
            : NotFound(new { message = "Email not found" });
    }

    [HttpPost("verify-otp")]
    public async Task<bool> VerifyOTP(VerifyOtpDto dto)
    {
        try
        {
            var person = await _context.Person
                .FirstOrDefaultAsync(p => p.Email == dto.Email);

            if (person == null || 
                person.PasswordResetToken == null  )
                //person.ResetTokenExpires < DateTime.UtcNow)
                return false;

            // Verify the actual OTP value here
            return person.PasswordResetToken.Equals(dto.Otp);
            //return BCrypt.Net.BCrypt.Verify(dto.Otp, person.PasswordResetToken);
        }
        catch (Exception ex)
        {
            // Avoid null reference exception if logger is null
            if (_logger != null)
            {
                _logger.LogError(ex, "Error verifying OTP for email: {Email}", dto.Email);
                
            }
            
            throw; // Or return an appropriate error response
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var result = await _passwordResetService.ResetPassword(dto);
        return result 
            ? Ok(new { message = "Password reset successfully" }) 
            : BadRequest(new { message = "Password reset failed" });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (model == null)
        {
            return BadRequest("Invalid client request");
        }

        try{
            // Find user by username/email
            var user = await _context.Person
                .FirstOrDefaultAsync(p => p.Email == model.Email);

            // Check if user exists and password is correct

            if (user == null || string.IsNullOrEmpty(user.UserPassword) || string.IsNullOrEmpty(model.Password) || !VerifyPassword(model.Password, user.UserPassword))
            {
                return Unauthorized("Invalid credentials");
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token = token,
                userId = user.PersonId,
                email = user.Email
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(500, "Internal server error");
        }
    }
    private string GenerateJwtToken(Person user)
    {

#pragma warning disable CS8604 // Possible null reference argument.
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
#pragma warning restore CS8604 // Possible null reference argument.

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            //new Claim(JwtRegisteredClaimNames.Sub, user.IdentityId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
     private bool VerifyPassword(string password, string storedHash)
    {
        Boolean value = false;
        // In a real app, use a proper password hashing library
        // This is a simplified example
        if(password.Equals(storedHash)){
            value = true;
        }

        return value;
    }


}
public class LoginModel
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}