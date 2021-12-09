using System;
using jobTrackerAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace jobTrackerAPI.Data
{
    public class ApplicationContext : DbContext
    {
        // table
        public DbSet<Application> JobApplication {get;set;}
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
    }
}