using System.Collections.Generic;
using System.Threading.Tasks;
using bugspotAPI.Models;
using bugspotAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bugspotAPI.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class TypeController : Controller
    {
        private readonly IProjectRepository _repository;
        public TypeController(IProjectRepository projRepo)
        {
            _repository = projRepo;
        }

        [HttpGet]
        public IEnumerable<TypeModel> GetTypeList()
        {
            return _repository.GetTypes();
        }
        
        [HttpPost("Create")]
        public async Task<IActionResult> Create(TypeModel type)
        {
            // if everything is correct
            var newtype = new TypeModel {
                typeName = type.typeName,
            };
            // Add the new information
            return Created("Type successfully created.", await _repository.AddType(newtype));
        }

        [HttpPut("Edit/{id}")]
        public IActionResult Edit(int id, TypeModel type)
        {
            // get company info by that id
            var existingType = _repository.TypeById(id);
            
            // check if the value is null
            if (existingType == null)
            {
                return BadRequest(new
                {
                    message = "No match found in the database."
                });
            }

            if (type.typeName != null) existingType.typeName = type.typeName;

            // save the changes
            _repository.SaveChanges();

            return Ok(existingType);
        }

        [HttpDelete("Remove/{id}")]
        public IActionResult Delete(int id)
        {
            // get company by id
            var existingType = _repository.TypeById(id);

            // check if value is null
            if (existingType == null)
            {
                return BadRequest(new
                {
                    message = "Operation could not complete. No match found in the database."
                });     
            }

            _repository.DeleteType(existingType); 

            return Ok(new { message = "Type was successfully removed from your database." });
        }
        
    }
}