using System;
using System.Collections.Generic;
using jobTrackerAPI.Model;

namespace jobTrackerAPI.Repositories
{
    public interface IApplicationsRepository
    {
        IEnumerable<Application> GetApplications();
        Application GetApplication(int id);
        void AddApplication(Application application);
        void UpdateApplication(Application application);
        void RemoveApplication(int id);
    }
}