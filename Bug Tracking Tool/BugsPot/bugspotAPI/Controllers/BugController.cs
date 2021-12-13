using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using bugspotAPI.Dtos;
using bugspotAPI.Models;
using bugspotAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bugspotAPI.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class BugController : Controller
    {
        private readonly IProjectRepository _repository;
        public BugController(IProjectRepository projRepo)
        {
            _repository = projRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<BugModel>> GetBugList(int companyId)
        {
            return await _repository.GetBugsByCoAsync(companyId);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBug(int id)
        {
            var existingBug = await _repository.BugByIdAsync(id);
            
            if(existingBug == null) 
            {
                return BadRequest(new { message = "No match found in the database." });
            }
            
            return Ok(existingBug);
        }

        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(BugDto bug)
        {
            if (ModelState.IsValid)
            {
                var newBug = new BugModel {
                    title = bug.title,
                    typeId = bug.typeId,
                    statusId = bug.statusId,
                    priorityId = bug.priorityId,
                    severityId = bug.severityId,
                    reporterId = bug.reporterId,
                    developerId = bug.developerId,
                    description = bug.description,
                    stepsToProd = bug.stepsToProd,
                    actualRes = bug.actualRes,
                    expectedRes = bug.expectedRes
                };

                return Created("Success!", await _repository.AddBug(newBug));
            }

            return new JsonResult("Something went wrong") {StatusCode = 500};
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, BugEditDto bug)
        {
            // get bug info by that id
            var existingBug = await _repository.BugByIdAsync(id);
            
            // check if the value is null
            if (existingBug == null)
            {
                return BadRequest(new
                {
                    message = "No match found in the database."
                });
            }
            // update value
            if (bug.title != null) existingBug.title = bug.title;
            existingBug.typeId = bug.typeId;
            existingBug.statusId = bug.statusId;
            existingBug.priorityId = bug.priorityId;
            existingBug.severityId = bug.severityId;
            if (bug.reporterId != null) existingBug.reporterId = bug.reporterId;
            if (bug.developerId != null) existingBug.developerId = bug.developerId;

            // save the changes
            _repository.SaveChanges();

            return Ok(existingBug);
        }

        [HttpDelete("Archive/{id}")]
        public async Task<IActionResult> ArchiveBug(int id)
        {
            // get company by id
            var existingBug = await _repository.BugByIdAsync(id);

            // check if value is null
            if (existingBug != null)
            {
                await _repository.ArchiveBugAsync(existingBug);   
            }
            else
            {
                return BadRequest(new
                {
                    message = "Operation could not complete. No match found in the database."
                });
            }

            return Ok(new { message = "Record was successfully removed from your database." });

        }
        
    }
}