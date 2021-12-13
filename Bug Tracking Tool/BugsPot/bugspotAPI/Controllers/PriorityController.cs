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
    public class PriorityController : Controller
    {
        private readonly IProjectRepository _repository;
        public PriorityController(IProjectRepository projRepo)
        {
            _repository = projRepo;
        }

        [HttpGet]
        public IEnumerable<PriorityModel> GetPriorityList()
        {
            return _repository.GetPriority();
        }
        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(PriorityModel priority)
        {
            // if everything is correct
            var newPriority = new PriorityModel {
                priorityName = priority.priorityName,
            };
            // Add the new information
            return Created("Priority Successfully Created.", await _repository.AddPriority(newPriority));
        }

        [HttpPut("Edit/{id}")]
        public IActionResult Edit(int id, PriorityModel pri)
        {
            // get company info by that id
            var existingPriority = _repository.PriorityById(id);
            
            // check if the value is null
            if (existingPriority == null)
            {
                return BadRequest(new
                {
                    message = "No match found in the database."
                });
            }

            if (pri.priorityName != null) existingPriority.priorityName = pri.priorityName;

            // save the changes
            _repository.SaveChanges();

            return Ok(existingPriority);
        }

        [HttpDelete("Remove/{id}")]
        public IActionResult Delete(int id)
        {
            // get company by id
            var existingPriority = _repository.PriorityById(id);

            // check if value is null
            if (existingPriority == null)
            {
                return BadRequest(new
                {
                    message = "Operation could not complete. No match found in the database."
                });     
            }

            _repository.DeletePriority(existingPriority); 

            return Ok(new { message = "Project was successfully removed from your database." });
        }

    }
}