using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using BC = BCrypt.Net.BCrypt;
using VID.Data;

namespace VID.Services
{
    public class PasswordResetService
    {
        private readonly ApplicationDbContext _context;

        //private readonly ApplicationDbContext _context;

        private readonly ILogger<PasswordResetService> _logger;

        public PasswordResetService(ApplicationDbContext context, ILogger<PasswordResetService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> ForgotPassword(ForgotPasswordDto dto)
        {
            var person = await _context.Person
                .FirstOrDefaultAsync(p => p.Email == dto.Email);

            if (person == null)
                return false;

            // Generate 6-digit OTP
            string otp = GenerateOTP();

            // Store OTP as reset token with expiration
            person.PasswordResetToken = otp;
            //person.ResetTokenExpires = DateTime.UtcNow.AddMinutes(30);

            await _context.SaveChangesAsync();

            // Send OTP via email
            await SendOtpEmail(person.Email, otp);

            return true;
        }

        public async Task<bool> VerifyOTP(VerifyOtpDto dto) 
        {
            try 
            {
                _logger.LogInformation("Verifying OTP for email: {Email}", dto.Email);

                // Trim the OTP to handle potential whitespace issues
                string sanitizedOtp = dto.Otp?.Trim() ?? string.Empty;

                var person = await _context.Person
                    .FirstOrDefaultAsync(p => p.Email == dto.Email);

                if (person == null)
                {
                    _logger.LogWarning("OTP verification failed: Person not found for email: {Email}", dto.Email);
                    return false;
                }

                if (person.PasswordResetToken == null)
                {
                    _logger.LogWarning("OTP verification failed: No reset token exists for email: {Email}", dto.Email);
                    return false;
                }

                if (person.ResetTokenExpires < DateTime.UtcNow)
                {
                    _logger.LogWarning("OTP verification failed: Token expired at {Expiry}, current time: {Now} for email: {Email}", 
                        person.ResetTokenExpires, DateTime.UtcNow, dto.Email);
                    return false;
                }

                // Log the comparison values (but mask sensitive data in production)
                _logger.LogDebug("Comparing received OTP: {ReceivedOTP} with stored token", sanitizedOtp);

                // Check if we're using BCrypt or direct comparison
                bool isOtpValid;
                if (person.PasswordResetToken.StartsWith("$2a$") || person.PasswordResetToken.StartsWith("$2b$"))
                {
                    // The token is a BCrypt hash
                    isOtpValid = BCrypt.Net.BCrypt.Verify(sanitizedOtp, person.PasswordResetToken);
                    _logger.LogDebug("Using BCrypt verification for OTP");
                }
                else
                {
                    // Direct string comparison (for non-hashed tokens)
                    isOtpValid = person.PasswordResetToken.Equals(sanitizedOtp);
                    _logger.LogDebug("Using direct string comparison for OTP");
                }

                if (!isOtpValid)
                {
                    _logger.LogWarning("OTP verification failed: Invalid OTP provided for email: {Email}", dto.Email);
                }
                else
                {
                    _logger.LogInformation("OTP verified successfully for email: {Email}", dto.Email);
                }

                return isOtpValid;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error verifying OTP for email: {Email}", dto.Email);
                throw; // Rethrow to let the controller handle it
            } 
        }



        public async Task<bool> ResetPassword(ResetPasswordDto dto)
        {
            var person = await _context.Person
                .FirstOrDefaultAsync(p => p.Email == dto.Email);

            if (person == null) 
                return false;

            // Update password
            person.UserPassword = dto.NewPassword;
            person.PasswordResetToken = null;
            person.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return true;
        }

        private static string GenerateOTP()
        {
            return RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        }

        private static async Task SendOtpEmail(string email, string otp)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("sifisom380@gmail.com", "kcca qboz blrd ruts"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sifisom380@gmail.com"),
                Subject = "Password Reset OTP",
                Body = $"Your OTP is: {otp}",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);


            // try
            //     {
            //     var emailUser = Environment.GetEnvironmentVariable("SMTP_EMAIL") ?? "your-email@gmail.com";
            //     var emailPass = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "your-app-password";

            //     using var smtpClient = new SmtpClient("smtp.gmail.com")
            //     {
            //         Port = 587,
            //         Credentials = new NetworkCredential(emailUser, emailPass),
            //         EnableSsl = true,
            //     };

            //     var mailMessage = new MailMessage
            //     {
            //         From = new MailAddress(emailUser),
            //         Subject = "Password Reset OTP",
            //         Body = $"Your OTP is: {otp}",
            //         IsBodyHtml = false,
            //     };
            //     mailMessage.To.Add(email);

            //     await smtpClient.SendMailAsync(mailMessage);
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"Failed to send OTP email: {ex.Message}");
            //     // Log error properly in production
            // }

        }
    }
}