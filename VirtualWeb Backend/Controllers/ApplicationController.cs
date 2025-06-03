using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using VID.Models;
using VID.Repositories;
using Npgsql;


namespace VID.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IGenericRepository<Application> _repository;



        public ApplicationController(IGenericRepository<Application> repository)
        {
            _repository = repository;

        }

        

        private async Task<bool> CheckParentExists(string parentId)
        {
            Console.WriteLine($"CheckParentExists called with ParentId: {parentId}");

            if (string.IsNullOrEmpty(parentId))
            {
                Console.WriteLine("ParentId is empty or null. Returning false.");
                return false; // If parentId is empty, no need to query the database
            }

            try
            {
                using (var connection = new NpgsqlConnection("Host=localhost;Port=5432;Database=HomeAffairsDB;Username=postgres;Password=sifisom;"))
                {
                    Console.WriteLine("Opening database connection...");
                    await connection.OpenAsync();
                    Console.WriteLine("Database connection opened.");

                    var query = @"
                        SELECT COUNT(*)
                        FROM public.""IdentityDetails""
                        WHERE ""identityNumber"" = @ParentId";

                    Console.WriteLine("Executing query:");
                    Console.WriteLine(query);

                    var command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ParentId", parentId);

                    Console.WriteLine($"Parameters: ParentId={parentId}");

                    var count = (long?)await command.ExecuteScalarAsync() ?? 0;

                    Console.WriteLine($"Query result count: {count}");

                    if (count == 0)
                    {
                        Console.WriteLine("No matching record found in the database for the provided ParentId.");
                        return false;
                    }

                    Console.WriteLine("Matching record found in the database for the provided ParentId.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CheckParentExists: {ex.Message}");
                throw; // Re-throw the exception after logging it
            }
        }

            
        
    
            [HttpPost("SubmitAll")]
            public async Task<IActionResult> SubmitAll([FromForm] ApplicationSubmissionDto dto)
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(dto.PersonId) || string.IsNullOrWhiteSpace(dto.FullName))
                    return BadRequest("PersonId and FullName are required.");

                // Handle profile photo upload
                string photoPath = null;
                if (dto.ProfilePicture != null && dto.ProfilePicture.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Photos");
                    Directory.CreateDirectory(uploadsFolder);
                    var fileName = $"{Guid.NewGuid()}_{dto.ProfilePicture.FileName}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.ProfilePicture.CopyToAsync(stream);
                    }
                    photoPath = $"/Data/Photos/{fileName}";
                }

                // TODO: Save all dto fields and photoPath to your database here

                return Ok(new { message = "Application submitted successfully!", photoPath });
            }
        

        [HttpGet("GetParentInfoByChildId/{childIdNumber}")]
        public async Task<IActionResult> GetParentInfoByChildId(string childIdNumber)
        {
            try
            {
                using (var connection = new NpgsqlConnection("Host=localhost;Port=5432;Database=HomeAffairsDB;Username=postgres;Password=sifisom;"))
                {
                    await connection.OpenAsync();
                    var query = @"
                                SELECT 
                                    i.""firstname"" || ' ' || i.""surname"" AS ""ParentName"", 
                                    i.""identityNumber"" AS ""ParentId"", 
                                    i.""gender"" AS ""ParentGender""
                                FROM public.""IdentityDetails"" i
                                JOIN public.""ChildDetails"" c ON i.""identityId"" = c.""parentIdentityId""
                                WHERE c.""childIdentityNumber"" = @ChildIdNumber
                                LIMIT 1";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ChildIdNumber", childIdNumber);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var parentInfo = new
                                {
                                    ParentName = reader["ParentName"].ToString(),
                                    ParentId = reader["ParentId"].ToString(),
                                    ParentGender = reader["ParentGender"].ToString()
                                };
                                return Ok(parentInfo);
                            }
                            return NotFound("No parent found for this ID number.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetAll()
        {
            var children = await _repository.GetAllAsync();
            return Ok(children);
        }

        // [HttpGet("{applicationId}/profilepicture")]
        // public IActionResult GetProfilePicture(int applicationId)
        // {
        //     var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfilePictures");
        //     var filePath = Path.Combine(folderPath, $"{applicationId}.jpeg");

        //     if (!System.IO.File.Exists(filePath))
        //     {
        //         // ✅ Return default image if user-specific image not found
        //         var defaultImagePath = Path.Combine(folderPath, "muntu.jpeg");

        //         if (System.IO.File.Exists(defaultImagePath))
        //         {
        //             return File(System.IO.File.OpenRead(defaultImagePath), "image/jpeg");
        //         }

        //         // ❌ Default also not found, return 404
        //         return NotFound();
        //     }

        //     // ✅ Serve the actual profile picture
        //     var imageFileStream = System.IO.File.OpenRead(filePath);
        //     return File(imageFileStream, "image/jpeg");
        // }

        [HttpGet("{id}/profilepicture")]
        // public async Task<IActionResult> GetProfilePicture(int id)
        // {
        //     // try
        //     // {
        //     //     var profilePicture = await _applicationService.GetProfilePictureAsync(id);

        //     //     if (profilePicture == null || profilePicture.Length == 0)
        //     //     {
        //     //         // Return 404 if no profile picture exists
        //     //         return NotFound("Profile picture not found for this application");
        //     //     }

        //     //     // Return the image with the correct content type
        //     //     return File(profilePicture, "image/jpeg"); // Adjust content type if needed
        //     // }
        //     // catch (Exception ex)
        //     // {
        //     //     // Log the exception
        //     //     Console.Error.WriteLine($"Error retrieving profile picture for application {id}: {ex.Message}");
        //     //     return StatusCode(500, "An error occurred while retrieving the profile picture");
        //     // }

        //     return StatusCode(500, "An error occurred while retrieving the profile picture");
        // }

        [HttpGet("{id}")]
        public async Task<ActionResult<Application>> GetById(int id)
        {
            var child = await _repository.GetByIdAsync(id);
            if (child == null)
            {
                return NotFound();
            }
            return Ok(child);
        }

        [HttpPost]
        public async Task<ActionResult<Application>> Create(Application application)
        {
            try
            {
                // Validate that at least one parent ID is provided
                if (string.IsNullOrEmpty(application.FatherId) && string.IsNullOrEmpty(application.MotherId))
                {
                    return BadRequest("Either FatherId or MotherId must be provided.");
                }

                // Check if application with this PersonId already exists
                var allApplications = await _repository.GetAllAsync();
                bool exists = allApplications.Any(a => a.PersonId == application.PersonId);

                if (exists)
                {
                    return BadRequest("An application with this ID number already exists.");
                }

                // Check if the filled parent exists in the Home Affairs database
                bool parentExists = false;

                if (!string.IsNullOrEmpty(application.FatherId))
                {
                    parentExists = await CheckParentExists(application.FatherId);
                }

                if (!parentExists && !string.IsNullOrEmpty(application.MotherId))
                {
                    parentExists = await CheckParentExists(application.MotherId);
                }

                if (!parentExists)
                {
                    return BadRequest("No matching record found in the Home Affairs database for the provided parent.");
                }

                // Create the application
                var result = await _repository.CreateAsync(application);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("CheckExists/{personId}")]
        public async Task<IActionResult> CheckExists(string personId)
        {
            try
            {
                // Get all applications and filter by PersonId
                var allApplications = await _repository.GetAllAsync();

                // Check if any application has the matching PersonId (as string, no conversion)
                bool exists = allApplications.Any(a => a.PersonId == personId);

                Console.WriteLine($"CheckExists for ID {personId}: {exists}");

                return Ok(exists);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CheckExists: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Application application)
        {
            if (id != application.ApplicationId)
            {
                return BadRequest();
            }

            //admin.ChildDob = DateTime.SpecifyKind(profile.ChildDob, DateTimeKind.Utc);
            await _repository.UpdateAsync(application);
            return NoContent();
        }

        

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        

// Alternative model with PascalCase (if your API uses PascalCase)


// Updated Controller Method with better error handling
        [HttpPost("update-photo")]
        public async Task<IActionResult> UpdateChildPhoto([FromBody] ChildPhotoUpdateModel model)
        {
            // Log the received model for debugging
            Console.WriteLine($"Received model: ChildIdNumber={model?.ChildIdNumber}, PhotoFilePath={model?.PhotoFilePath}");
            
            if (model == null)
            {
                return BadRequest(new { error = "No data received", details = "Model is null" });
            }

            if (string.IsNullOrEmpty(model.ChildIdNumber))
            {
                return BadRequest(new { error = "Child ID Number is missing", field = "childIdNumber" });
            }

            if (string.IsNullOrEmpty(model.PhotoFilePath))
            {
                return BadRequest(new { error = "Photo File Path is missing", field = "photoFilePath" });
            }

            try
            {
                // Construct the full file path
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), model.PhotoFilePath);
                Console.WriteLine($"Looking for file at: {fullPath}");
                
                // Check if file exists
                if (!System.IO.File.Exists(fullPath))
                {
                    return BadRequest(new { 
                        error = "Photo file not found", 
                        attemptedPath = fullPath,
                        workingDirectory = Directory.GetCurrentDirectory(),
                        suggestion = "Make sure the photo was uploaded correctly"
                    });
                }

                // Read the image file as binary data
                byte[] photoBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
                Console.WriteLine($"Read {photoBytes.Length} bytes from file");

                using (var connection = new NpgsqlConnection("Host=localhost;Port=5432;Database=HomeAffairsDB;Username=postgres;Password=sifisom;"))
                {
                    await connection.OpenAsync();

                    var query = @"
                        UPDATE public.""ChildDetails""
                        SET ""childPhoto"" = @PhotoData
                        WHERE ""childIdentityNumber"" = @ChildIdNumber";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PhotoData", photoBytes);
                        command.Parameters.AddWithValue("@ChildIdNumber", model.ChildIdNumber);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        Console.WriteLine($"Rows affected: {rowsAffected}");

                        if (rowsAffected == 0)
                        {
                            return NotFound(new { 
                                error = "No child found with the given ID number",
                                searchedId = model.ChildIdNumber 
                            });
                        }
                    }
                }

                return Ok(new { 
                    message = "Child photo updated successfully.",
                    childId = model.ChildIdNumber,
                    photoSize = photoBytes.Length
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { 
                    error = "Internal server error", 
                    details = ex.Message 
                });
            }
        }

    }
}
