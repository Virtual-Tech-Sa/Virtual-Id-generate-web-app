using Microsoft.AspNetCore.Mvc;
using VID.Models;
using Npgsql;

using VID.Repositories;

namespace VID.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IGenericRepository<Person> _repository;

        public PersonController(IGenericRepository<Person> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetAll()
        {
            var children = await _repository.GetAllAsync();
            return Ok(children);
        }

        [HttpGet("exists/{idNumber}")]
        public async Task<IActionResult> CheckIfIdExists(string idNumber)
        {
            try
            {
                using (var connection = new NpgsqlConnection("Host=vid-fulldatabase.crq2q6u64pfx.eu-north-1.rds.amazonaws.com;Port=5432;Database=postgres;Username=postgres;Password=Moment2Sifiso#;"))
                {
                    await connection.OpenAsync();

                    // Replace these with the actual schema and table name from the debug endpoint
                    string schemaName = "public";
                    string tableName = "person";

                    var query = $"SELECT COUNT(*) FROM {schemaName}.{tableName} WHERE \"IDENTITY_ID\" = @IdNumber";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdNumber", idNumber);
                        var count = (long)await command.ExecuteScalarAsync();
                        return Ok(new { exists = count > 0 });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetById(int id)
        {
            var child = await _repository.GetByIdAsync(id);
            if (child == null)
            {
                return NotFound();
            }
            return Ok(child);
        }


        [HttpGet("debug/tables")]
        public async Task<IActionResult> GetTableInfo()
        {
            try
            {
                using (var connection = new NpgsqlConnection("Host=home-affairsdb.crq2q6u64pfx.eu-north-1.rds.amazonaws.com;Port=5432;Database=postgres;Username=postgres;Password=Moment2Sifiso#;"))
                {
                    await connection.OpenAsync();

                    // Get all tables in all schemas
                    var query = @"
                SELECT table_schema, table_name 
                FROM information_schema.tables 
                WHERE table_type = 'BASE TABLE' 
                ORDER BY table_schema, table_name";

                    var tables = new List<object>();

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                tables.Add(new
                                {
                                    schema = reader.GetString(0),
                                    table = reader.GetString(1)
                                });
                            }
                        }
                    }

                    return Ok(tables);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("child-info/{idNumber}")]
        public async Task<IActionResult> GetChildInfoById(string idNumber)
        {
            try
            {
                using (var connection = new NpgsqlConnection("Host=home-affairsdb.crq2q6u64pfx.eu-north-1.rds.amazonaws.com;Port=5432;Database=postgres;Username=postgres;Password=Moment2Sifiso#;"))
                {
                    await connection.OpenAsync();
                    var query = @"
                                SELECT 
                                    ""childFirstname"" AS ""Firstname"", 
                                    ""childSurname"" AS ""Surname"", 
                                    ""childGender"" AS ""Gender"", 
                                    ""childDateOfBirth"" AS ""DateOfBirth""
                                FROM public.""ChildDetails""
                                WHERE ""childIdentityNumber"" = @IdNumber
                                LIMIT 1";


                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdNumber", idNumber);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var childInfo = new
                                {
                                    Firstname = reader["Firstname"].ToString(),
                                    Surname = reader["Surname"].ToString(),
                                    Gender = reader["Gender"].ToString(),
                                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]).ToString("yyyy-MM-dd")
                                };
                                return Ok(childInfo);
                            }
                            return NotFound("ID number not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Person>> Create(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //admin.ChildDob = DateTime.SpecifyKind(admin.ChildDob, DateTimeKind.Utc);
            var created = await _repository.CreateAsync(person);

            return CreatedAtAction(nameof(GetById), new { id = created.PersonId }, created);


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Person person)
        {
            if (id != person.PersonId)
            {
                return BadRequest("Person ID mismatch.");
            }

            await _repository.UpdateAsync(person);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("exists/email/{email}")]
    public async Task<IActionResult> CheckIfEmailExists(string email)
    {
        try
        {
            using (var connection = new NpgsqlConnection("Host=vid-fulldatabase.crq2q6u64pfx.eu-north-1.rds.amazonaws.com;Port=5432;Database=postgres;Username=postgres;Password=Moment2Sifiso#;"))
            {
                await connection.OpenAsync();
                var query = $"SELECT COUNT(*) FROM public.person WHERE \"Email\" = @Email";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    var count = (long)await command.ExecuteScalarAsync();
                    return Ok(new { exists = count > 0 });
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
        
        [HttpPost("upload-photo")]
        public async Task<IActionResult> UploadPhoto([FromForm] IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Data", "Photos");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }
            return Ok(new { filePath = $"/Data/Photos/{uniqueFileName}" });
        }
    }
}
