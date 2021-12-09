using System;
using System.Collections.Generic;
using jobTrackerAPI.Model;
using jobTrackerAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace jobTrackerAPI.Controller
{
    [ApiController]
    [Route("api")] // name "api" is made up 
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationsRepository _repository;

        public ApplicationController (IApplicationsRepository repository)
        {
            _repository = repository;
        }

        // GET /api
        [HttpGet]
        public IEnumerable<Application> GetApplications()
        {
            var applications = _repository.GetApplications();
            return applications;
        }

        // GET /api/{id}
        [HttpGet("{id}")]
        public ActionResult<Application> GetApplication(int id)
        {
            var res = _repository.GetApplication(id);

            if (res is null)
                return NotFound(); // NotFound() is part of ControllerBase

            return res;

        }

        // POST /api
        [HttpPost]
        public ActionResult<Application> CreateApplication(Application application)
        {
            var newApp = new Application() {
                company= application.company,
                position=application.position,
                website=application.website,
                address=application.address,
                contact=application.contact,
                phone=application.phone,
                notes=application.notes
            };
            
            _repository.AddApplication(newApp);

            return CreatedAtAction("GetApplications", newApp);
        }

        // PUT /api/{id}
        [HttpPut("{id}")] // PUT requires two input parameter
        public ActionResult EditApplication(int id, Application application)
        {
            var existingApp = _repository.GetApplication(id);

            if (existingApp is null)
                return NotFound();

            if (application.company != null) existingApp.company = application.company;
            if (application.position != null) existingApp.position = application.position;
            if (application.website != null) existingApp.website = application.website;
            if (application.address != null) existingApp.address = application.address;
            if (application.contact != null) existingApp.contact = application.contact;
            if (application.phone != null) existingApp.phone = application.phone;
            if (application.notes != null) existingApp.notes = existingApp.notes;
            
            _repository.UpdateApplication(existingApp);
            return NoContent();
        }

        // DELETE /api/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteApplication(int id)
        {
            var existingApp = _repository.GetApplication(id);

            if (existingApp is null)
                return NotFound();
            
            _repository.RemoveApplication(id);

            return NoContent();
        }
    }
}