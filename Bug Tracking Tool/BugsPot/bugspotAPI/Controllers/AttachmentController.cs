using System.Collections.Generic;
using System.Threading.Tasks;
using bugspotAPI.Dtos;
using bugspotAPI.Models;
using bugspotAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bugspotAPI.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class AttachmentController : Controller
    {
        private readonly IProjectRepository _repository;
        public AttachmentController(IProjectRepository projRepo)
        {
            _repository = projRepo;
        }

        [HttpGet]
        public IEnumerable<AttachmentModel> GetAttachmentList()
        {
            return _repository.GetAttachment();
        }

        [HttpGet("{id}")]
        public IActionResult GetAttachment(int id)
        {
            var existingAttachment = _repository.AttachmentById(id);
            
            if(existingAttachment == null) 
            {
                return BadRequest(new { message = "No match found in the database." });
            }
            
            return Ok(existingAttachment);
        }

        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(AttachmentDto attachment)
        {
            // Required fields
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            // if everything is correct
            var newAttachment = new AttachmentModel {
                userId = attachment.userId,
                imageName = attachment.imageName,
                imageData = attachment.imageData,
                formFile = attachment.formFile,
                fileExtension = attachment.fileExtension
            };
            // Add the new information
            return Created("Attachment successfully added.", await _repository.AddAttachment(newAttachment));
        }

        [HttpDelete("Remove/{id}")]
        public IActionResult Delete(int id)
        {
            // get company by id
            var existingAttachment = _repository.AttachmentById(id);

            // check if value is null
            if (existingAttachment == null)
            {
                return BadRequest(new
                {
                    message = "Operation could not complete. No match found in the database."
                });     
            }

            _repository.DeleteAttachment(existingAttachment); 

            return Ok(new { message = "Attachment was successfully removed from your database." });
        }
        
    }
}