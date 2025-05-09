using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace VID.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(IConfiguration configuration, ILogger<NotificationController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public class EmailRequest
        {
            public string ToEmail { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ToEmail))
                {
                    return BadRequest("Email address is required");
                }

                // Get email settings from configuration
                string smtpHost = _configuration["EmailSettings2:SmtpHost"] ?? "smtp.gmail.com";
                int smtpPort = int.Parse(_configuration["EmailSettings2:SmtpPort"] ?? "587");
                string smtpUsername = _configuration["EmailSettings2:Username"] ?? "your-email@gmail.com";
                string smtpPassword = _configuration["EmailSettings2:Password"] ?? "your-app-password";
                string fromEmail = _configuration["EmailSettings2:FromEmail"] ?? "home-affairs@example.com";
                string fromName = _configuration["EmailSettings2:FromName"] ?? "Home Affairs Department";

                using (var client = new SmtpClient(smtpHost, smtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                    var message = new MailMessage
                    {
                        From = new MailAddress(fromEmail, fromName),
                        Subject = request.Subject,
                        Body = request.Body,
                        IsBodyHtml = false
                    };

                    message.To.Add(request.ToEmail);

                    await client.SendMailAsync(message);
                    _logger.LogInformation($"Email notification sent to {request.ToEmail}");

                    return Ok(new { success = true, message = "Email sent successfully" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email notification");
                return StatusCode(500, new { success = false, message = "Failed to send email", error = ex.Message });
            }
        }
    }
}