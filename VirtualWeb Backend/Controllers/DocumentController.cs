// DocumentController.cs
using Microsoft.AspNetCore.Mvc;
using VID.Models;
using VID.Repositories;
using System.IO;
using System.Threading.Tasks;

namespace VID.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IGenericRepository<Document> _repository;

        public DocumentController(IGenericRepository<Document> repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<Document>> UploadDocuments([FromForm] DocumentUploadDto dto)
        {
            try
            {
                var document = new Document
                {
                    ApplicationId = dto.ApplicationId,
                    DocumentCode = "APPLICATION_DOCS"
                };

                if (dto.BirthCertificate != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await dto.BirthCertificate.CopyToAsync(memoryStream);
                        document.BirthCertificate = memoryStream.ToArray();
                    }
                }

                if (dto.IdCopy != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await dto.IdCopy.CopyToAsync(memoryStream);
                        document.IdCopy = memoryStream.ToArray();
                    }
                }

                var result = await _repository.CreateAsync(document);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

    public class DocumentUploadDto
    {
        public required string ApplicationId { get; set; }
        public required IFormFile BirthCertificate { get; set; }
        public required IFormFile IdCopy { get; set; }
    }
}