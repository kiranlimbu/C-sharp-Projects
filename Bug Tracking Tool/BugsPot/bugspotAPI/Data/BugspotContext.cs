using System;
using bugspotAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace bugspotAPI.Data
{
    public class BugspotContext : IdentityDbContext<UserModel>
    {
        public DbSet<CompanyModel> Companies { get; set; }
        public DbSet<ProjectModel> Projects { get; set; }
        public override DbSet<UserModel> Users { get; set; }
        public DbSet<BugModel> Bugs { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<HistModel> History { get; set; }
        public DbSet<AttachmentModel> Attachments { get; set; }
        public DbSet<NotifiModel> Notifications { get; set; }
        public DbSet<SeverityModel> Severities { get; set; }
        public DbSet<PriorityModel> Priorities { get; set; }
        public DbSet<StatusModel> Statuses { get; set; }
        public DbSet<TypeModel> Types { get; set; }
        public DbSet<InviteModel> Invites { get; set; }

        public BugspotContext( DbContextOptions<BugspotContext> options ) : base( options )
        {
        }
    }
}