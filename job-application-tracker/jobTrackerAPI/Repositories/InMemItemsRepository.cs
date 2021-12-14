using System;
using System.Collections.Generic;
using System.Linq;
using jobTrackerAPI.Data;
using jobTrackerAPI.Model;

namespace jobTrackerAPI.Repositories
{
    // In Memory Repository
    public class InMemItemsRepository : IApplicationsRepository
    {
        // Data Table Access
        private ApplicationContext _context;

        // Constructor, Dependency injection(context)
        public InMemItemsRepository(ApplicationContext context)
        {
            _context = context;
        }

        // Get all applications
        public IEnumerable<Application> GetApplications()
        {
            return _context.JobApplication.ToList();
        }

        // Get a application (Single)
        public Application GetApplication(int id)
        {
            var item = _context.JobApplication.Where(item => item.id == id).SingleOrDefault();
            return item;
        }

        // Insert an application
        public void AddApplication(Application application)
        {
            _context.JobApplication.Add(application);
            _context.SaveChanges();
        }

        // Update an application
        public void UpdateApplication(Application application)
        {
            var index = _context.JobApplication.Where(item => item.id == application.id).SingleOrDefault();
            if (index != null)
            {
                try
                {
                    index = application;
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    Console.WriteLine("Operation could not complete. Changes were not Saved!");
                }
            }
        }

        // Delete an application
        public void RemoveApplication(int id)
        {
            var index = _context.JobApplication.Where(item => item.id == id).SingleOrDefault();
            if (index != null)
                _context.JobApplication.Remove(index);
            
            _context.SaveChanges();

        }
    }
}