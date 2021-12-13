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
    public class HistoryController : Controller
    {
        private readonly IProjectRepository _repository;
        public HistoryController(IProjectRepository projRepo)
        {
            _repository = projRepo;
        }

        [HttpGet]
        public IEnumerable<HistModel> GetHistList()
        {
            return _repository.GetHist();
        }

        [HttpGet("{id}")]
        public IActionResult GetHistory(int id)
        {
            var existingHist = _repository.HistById(id);
            
            if(existingHist == null) 
            {
                return BadRequest(new { message = "No match found in the database." });
            }
            
            return Ok(existingHist);
        }

        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(HistDto hist)
        {
            // Required fields
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            // if everything is correct
            var newHist = new HistModel {
                userId = hist.userId,
                changedItem = hist.changedItem,
                oldValue = hist.oldValue,
                newValue = hist.newValue
            };
            // Add the new information
            return Created("History successfully created.", await _repository.AddHist(newHist));
        }

        [HttpPut("Edit/{id}")]
        public IActionResult Edit(int id, HistDto hist)
        {
            // get company info by that id
            var existingHist = _repository.HistById(id);
            
            // check if the value is null
            if (existingHist == null)
            {
                return BadRequest(new
                {
                    message = "No match found in the database."
                });
            }

            if (hist.userId != null) existingHist.userId = hist.userId;
            if (hist.changedItem != null) existingHist.changedItem = hist.changedItem;
            if (hist.oldValue != null) existingHist.oldValue = hist.oldValue;
            if (hist.newValue != null) existingHist.newValue = hist.newValue;

            // save the changes
            _repository.SaveChanges();

            return Ok(existingHist);
        }

        [HttpDelete("Remove/{id}")]
        public IActionResult Delete(int id)
        {
            // get company by id
            var existingHist = _repository.HistById(id);

            // check if value is null
            if (existingHist == null)
            {
                return BadRequest(new
                {
                    message = "Operation could not complete. No match found in the database."
                });     
            }

            _repository.DeleteHist(existingHist); 

            return Ok(new { message = "History was successfully removed from your database." });
        }

    }
}