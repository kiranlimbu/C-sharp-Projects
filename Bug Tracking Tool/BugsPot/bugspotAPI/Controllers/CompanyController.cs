using System;
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
    public class CompanyController : Controller
    {
        private readonly IProjectRepository _repository;
        private readonly IUserRepository _userRepository;
        public CompanyController(IProjectRepository projRepo, IUserRepository userRepository)
        {
            _repository = projRepo;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IEnumerable<CompanyModel> GetCompanyList()
        {
            return _repository.GetCompany();
        }

        
        [HttpPost("Create/{email}")]
        public async Task<IActionResult> Create(CompanyDto company, string email)
        {
            // get user
            UserModel user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return BadRequest("User email does not exist.");

            // Required fields
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            var newCompany = new CompanyModel {
                coName = company.coName,
                coDescription = company.coDescription,
            };

            // add to database
            var companyInfo = await _repository.AddCompany(newCompany);
            
            // add company id to user profile
            user.companyId = companyInfo.companyId;

            // update user table
            await _userRepository.UpdateUserAsync(user);

            return Ok("Company Successfully Created.");
        }

        [HttpPut("Edit/{id}")]
        public IActionResult Edit(int id, CompanyDto company)
        {
            // get company info by that id
            var existingCo = _repository.CompanyById(id);
            
            // check if the value is null
            if (existingCo == null)
            {
                return BadRequest(new
                {
                    message = "No match found in the database."
                });
            }

            if (company.coName != null) existingCo.coName = company.coName;
            if (company.coDescription != null) existingCo.coDescription = company.coDescription;

            // save the changes
            _repository.SaveChanges();

            return Ok(existingCo);
        }

        [HttpDelete("Remove/{id}")]
        public IActionResult Delete(int id)
        {
            // get company by id
            var existingCo = _repository.CompanyById(id);

            // check if value is null
            if (existingCo != null)
            {
                _repository.DeleteCompany(existingCo);   
            }
            else
            {
                return BadRequest(new
                {
                    message = "Operation could not complete. No match found in the database."
                });
            }

            return Ok(new { message = "Comapny was successfully removed from your database." });

        }
    }
}