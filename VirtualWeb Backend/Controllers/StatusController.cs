using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using VID.Models;
using VID.Repositories;

namespace VID.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly IGenericRepository<Status> _repository;

        public StatusController(IGenericRepository<Status> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Status>>> GetAll()
        {
            var children = await _repository.GetAllAsync();
            return Ok(children);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetById(int id)
        {
            var child = await _repository.GetByIdAsync(id);
            if (child == null)
            {
                return NotFound();
            }
            return Ok(child);
        }

        // [HttpPost]
        // public async Task<ActionResult<Admin>> Create(Admin admin)
        // {
        //     //admin.ChildDob = DateTime.SpecifyKind(admin.ChildDob, DateTimeKind.Utc);
        //     var created = await _repository.CreateAsync(admin);
        //     return CreatedAtAction(nameof(GetById), new { id = created.AdminId }, created);
        // }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Status status)
        {
            if (id != status.StatusID)
            {
                return BadRequest();
            }

            //admin.ChildDob = DateTime.SpecifyKind(profile.ChildDob, DateTimeKind.Utc);
            await _repository.UpdateAsync(status);
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
