using Microsoft.AspNetCore.Mvc;
using VID.Models;
using VID.Repositories;

namespace VID.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicantController : ControllerBase
    {
        private readonly IGenericRepository<Applicant> _repository;

        public ApplicantController(IGenericRepository<Applicant> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Applicant>>> GetAll()
        {
            var children = await _repository.GetAllAsync();
            return Ok(children);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Applicant>> GetById(int id)
        {
            var child = await _repository.GetByIdAsync(id);
            if (child == null)
            {
                return NotFound();
            }
            return Ok(child);
        }

        [HttpPost]
        public async Task<ActionResult<Applicant>> Create(Applicant applicant)
        {
            //admin.ChildDob = DateTime.SpecifyKind(admin.ChildDob, DateTimeKind.Utc);
            if(!ModelState.IsValid){

                return BadRequest(ModelState);

            }
            
            var created = await _repository.CreateAsync(applicant);
            return CreatedAtAction(nameof(GetById), new { id = created.ApplicantId}, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,Applicant applicant)
        {
            // if (id != user.UserId)
            // {
            //     return BadRequest();
            // }

            //admin.ChildDob = DateTime.SpecifyKind(profile.ChildDob, DateTimeKind.Utc);
            await _repository.UpdateAsync(applicant);
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
