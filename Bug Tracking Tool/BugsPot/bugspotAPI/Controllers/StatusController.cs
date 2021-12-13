using System.Collections.Generic;
using System.Threading.Tasks;
using bugspotAPI.Models;
using bugspotAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bugspotAPI.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class StatusController : Controller
    {
        private readonly IProjectRepository _repository;
        public StatusController(IProjectRepository projRepo)
        {
            _repository = projRepo;
        }

        [HttpGet]
        public IEnumerable<StatusModel> GetStatusList()
        {
            return _repository.GetStatus();
        }
        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(StatusModel status)
        {
            // if everything is correct
            var newStatus = new StatusModel {
                statusName = status.statusName,
            };
            // Add the new information
            return Created("Status successfully created.", await _repository.AddStatus(newStatus));
        }

        [HttpPut("Edit/{id}")]
        public IActionResult Edit(int id, StatusModel status)
        {
            // get company info by that id
            var existingStatus = _repository.StatusById(id);
            
            // check if the value is null
            if (existingStatus == null)
            {
                return BadRequest(new
                {
                    message = "No match found in the database."
                });
            }

            if (status.statusName != null) existingStatus.statusName = status.statusName;

            // save the changes
            _repository.SaveChanges();

            return Ok(existingStatus);
        }

        [HttpDelete("Remove/{id}")]
        public IActionResult Delete(int id)
        {
            // get company by id
            var existingStatus = _repository.StatusById(id);

            // check if value is null
            if (existingStatus == null)
            {
                return BadRequest(new
                {
                    message = "Operation could not complete. No match found in the database."
                });     
            }

            _repository.DeleteStatus(existingStatus); 

            return Ok(new { message = "Status was successfully removed from your database." });
        }
        
    }
}