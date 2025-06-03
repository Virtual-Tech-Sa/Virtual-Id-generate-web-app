// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using VID.Models;
// using VID.Data;

// namespace VID.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class AdminController : ControllerBase
//     {
//         private readonly ApplicationDbContext _context;

//         public AdminController(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         // POST: api/Admin/login
//         [HttpPost("login")]
//         public async Task<ActionResult<object>> Login([FromBody] AdminLoginModel model)
//         {
//             if (model == null)
//             {
//                 return BadRequest("Invalid login data");
//             }

//             var admin = await _context.Admins
//                 .FirstOrDefaultAsync(a => a.AdminUsername == model.AdminUsername);

//             if (admin == null)
//             {
//                 return Ok(new { success = false, message = "Invalid username or password" });
//             }

//             // In a production environment, you should use proper password hashing
//             if (admin.AdminPassword != model.AdminPassword)
//             {
//                 return Ok(new { success = false, message = "Invalid username or password" });
//             }

//             return Ok(new { 
//                 success = true, 
//                 adminId = admin.AdminId,
//                 username = admin.AdminUsername,
//                 name = admin.AdminName,
//                 surname = admin.AdminSurname
//             });
//         }

//         // GET: api/Admin/users/count
//         [HttpGet("users/count")]
//         public async Task<ActionResult<object>> GetUsersCount()
//         {
//             var count = await _context.Persons.CountAsync();
//             return Ok(new { count });
//         }

//         // GET: api/Admin/registrations/successful
//         [HttpGet("registrations/successful")]
//         public async Task<ActionResult<IEnumerable<object>>> GetSuccessfulRegistrations()
//         {
//             var registrations = await _context.Persons
//                 .Select(p => new
//                 {
//                     identityId = p.IdentityId,
//                     firstname = p.Firstname,
//                     surname = p.Surname,
//                     email = p.Email,
//                     gender = p.Gender,
//                     registrationDate = p.RegistrationDate
//                 })
//                 .OrderByDescending(p => p.registrationDate)
//                 .ToListAsync();

//             return Ok(registrations);
//         }

//         // GET: api/Admin/registrations/declined
//         [HttpGet("registrations/declined")]
//         public async Task<ActionResult<IEnumerable<object>>> GetDeclinedRegistrations()
//         {
//             var declined = await _context.FailedRegistrations
//                 .Select(f => new
//                 {
//                     identityId = f.IdentityId,
//                     attemptDate = f.AttemptDate,
//                     reason = f.Reason
//                 })
//                 .OrderByDescending(f => f.attemptDate)
//                 .ToListAsync();

//             return Ok(declined);
//         }

//         // GET: api/Admin/activity/recent
//         [HttpGet("activity/recent")]
//         public async Task<ActionResult<IEnumerable<object>>> GetRecentActivity()
//         {
//             var activities = await _context.UserActivities
//                 .OrderByDescending(a => a.Timestamp)
//                 .Take(50)  // Limit to most recent 50 activities
//                 .Select(a => new
//                 {
//                     activityId = a.ActivityId,
//                     identityId = a.IdentityId,
//                     activityType = a.ActivityType,
//                     details = a.Details,
//                     timestamp = a.Timestamp
//                 })
//                 .ToListAsync();

//             return Ok(activities);
//         }

//         // POST: api/Admin/log-failed-registration
//         [HttpPost("log-failed-registration")]
//         public async Task<ActionResult> LogFailedRegistration([FromBody] FailedRegistration model)
//         {
//             if (model == null)
//             {
//                 return BadRequest("Invalid data");
//             }

//             model.AttemptDate = DateTime.UtcNow;
            
//             _context.FailedRegistrations.Add(model);
//             await _context.SaveChangesAsync();
            
//             return Ok(new { success = true });
//         }

//         // POST: api/Admin/log-activity
//         [HttpPost("log-activity")]
//         public async Task<ActionResult> LogActivity([FromBody] UserActivity model)
//         {
//             if (model == null)
//             {
//                 return BadRequest("Invalid data");
//             }

//             model.Timestamp = DateTime.UtcNow;
            
//             _context.UserActivities.Add(model);
//             await _context.SaveChangesAsync();
            
//             return Ok(new { success = true });
//         }
//     }
// }