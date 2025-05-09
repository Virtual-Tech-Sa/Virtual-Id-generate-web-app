using Microsoft.AspNetCore.Mvc;
using VID.Models;
using VID.Repositories;

namespace VID.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NextOfKinController : ControllerBase
    {
        private readonly IGenericRepository<NextOfKin> _repository;

        public NextOfKinController(IGenericRepository<NextOfKin> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NextOfKin>>> GetAll()
        {
            var children = await _repository.GetAllAsync();
            return Ok(children);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NextOfKin>> GetById(int id)
        {
            var child = await _repository.GetByIdAsync(id);
            if (child == null)
            {
                return NotFound();
            }
            return Ok(child);
        }

        [HttpPost]
        public async Task<ActionResult<NextOfKin>> Create(NextOfKin nextOfKin)
        {
            if(!ModelState.IsValid){

                return BadRequest(ModelState);

            }
            //admin.ChildDob = DateTime.SpecifyKind(admin.ChildDob, DateTimeKind.Utc);
            var created = await _repository.CreateAsync(nextOfKin);
            
            return CreatedAtAction(nameof(GetById), new { id = created.NextOfKinId}, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NextOfKin nextOfKin)
        {
            // if (id != user.UserId)
            // {
            //     return BadRequest();
            // }

            //admin.ChildDob = DateTime.SpecifyKind(profile.ChildDob, DateTimeKind.Utc);
            await _repository.UpdateAsync(nextOfKin);
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
