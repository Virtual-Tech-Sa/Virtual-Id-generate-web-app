using Microsoft.AspNetCore.Mvc;
using VID.Models;
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

        [HttpPost]
        public async Task<ActionResult<Person>> Create(Person person)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            //admin.ChildDob = DateTime.SpecifyKind(admin.ChildDob, DateTimeKind.Utc);
            var created = await _repository.CreateAsync(person);
            
            return CreatedAtAction(nameof(GetById), new { id = created.PersonId}, created);
           
            
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
    }
}
