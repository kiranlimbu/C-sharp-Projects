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
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _repository;
        private readonly IUserRepository _userRepository;
        public ProjectController(IProjectRepository projRepo, IUserRepository userRepository)
        {
            _repository = projRepo;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IEnumerable<ProjectModel> GetProjectList()
        {
            return _repository.GetProjects();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id, int companyId)
        {
            var existingProj = await _repository.ProjByIdAsync(id, companyId);
            
            if(existingProj == null) 
            {
                return BadRequest(new { message = "No match found in the database." });
            }
            
            return Ok(existingProj);
        }

        
        [HttpPost("Create/{email}")] // add user while creating project
        public async Task<IActionResult> Create(ProjectDto proj, string email)
        {
            UserModel user = await _userRepository.GetByEmailAsync(email);
            // check if user is null
            if (user == null) return BadRequest("User email does not exist.");

            // Required fields
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            // if everything is correct
            var newProj = new ProjectModel {
                companyId = proj.companyId,
                projName = proj.projName,
                projDescription = proj.projDescription,
                endDate = proj.endDate,
                UserModelId = user.Id
            };
            // Add the new project
            ProjectModel project = await _repository.AddProj(newProj);

            return Ok("Project Successfully Created.");
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, ProjectDto proj, int companyId)
        {
            // get company info by that id
            var existingProj = await _repository.ProjByIdAsync(id, companyId);
            
            // check if the value is null
            if (existingProj == null)
            {
                return BadRequest(new
                {
                    message = "No match found in the database."
                });
            }

            if (proj.companyId != null) existingProj.companyId = proj.companyId;
            if (proj.projName != null) existingProj.projName = proj.projName;
            if (proj.projDescription != null) existingProj.projDescription = proj.projDescription;
            if (proj.startDate != null) existingProj.startDate = proj.startDate;
            if (proj.endDate != null) existingProj.endDate = proj.endDate;
            existingProj.archived = proj.archived;

            // save the changes
            _repository.SaveChanges();

            return Ok(existingProj);
        }

        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> Delete(int id, int companyId)
        {
            // get company by id
            var existingProj = await _repository.ProjByIdAsync(id, companyId);

            // check if value is null
            if (existingProj == null)
            {
                return BadRequest(new
                {
                    message = "Operation could not complete. No match found in the database."
                });     
            }

            _repository.DeleteProj(existingProj); 

            return Ok(new { message = "Project was successfully removed from your database." });
        }

        [HttpGet("GetProjUserExceptPM/{id}")]
        public async Task<List<UserModel>> GetProjUserExceptPM(int id)
        {
            return await _repository.GetProjUsersExceptPMAsync(id);
        }

    }
}