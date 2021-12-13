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
    public class SeverityController : Controller
    {
        private readonly IProjectRepository _repository;
        public SeverityController(IProjectRepository projRepo)
        {
            _repository = projRepo;
        }

        [HttpGet]
        public IEnumerable<SeverityModel> GetSeverityList()
        {
            return _repository.GetSeverity();
        }
        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(SeverityModel severity)
        {
            // if everything is correct
            var newSeverity = new SeverityModel {
                severityName = severity.severityName,
            };
            // Add the new information
            return Created("Severity successfully created.", await _repository.AddSeverity(newSeverity));
        }

        [HttpPut("Edit/{id}")]
        public IActionResult Edit(int id, SeverityModel severity)
        {
            // get company info by that id
            var existingSeverity = _repository.SeverityById(id);
            
            // check if the value is null
            if (existingSeverity == null)
            {
                return BadRequest(new
                {
                    message = "No match found in the database."
                });
            }

            if (severity.severityName != null) existingSeverity.severityName = severity.severityName;

            // save the changes
            _repository.SaveChanges();

            return Ok(existingSeverity);
        }

        [HttpDelete("Remove/{id}")]
        public IActionResult Delete(int id)
        {
            // get company by id
            var existingSeverity = _repository.SeverityById(id);

            // check if value is null
            if (existingSeverity == null)
            {
                return BadRequest(new
                {
                    message = "Operation could not complete. No match found in the database."
                });     
            }

            _repository.DeleteSeverity(existingSeverity); 

            return Ok(new { message = "Severity was successfully removed from your database." });
        }

    }
}