using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bugspotAPI.Data;
using bugspotAPI.Models;
using bugspotAPI.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace bugspotAPI.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly BugspotContext _context;
        private readonly IUserRoleService _userRoleService;
        public ProjectRepository(BugspotContext context, IUserRoleService userRoleService)
        {
            _context = context;
            _userRoleService = userRoleService;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        // COMPANY START ----------------------
        public IEnumerable<CompanyModel> GetCompany()
        {
            return _context.Companies;
        }

        public async Task<CompanyModel> AddCompany(CompanyModel comapny)
        {
            try
            {
                 _context.Companies.Add(comapny); // add user to the table
                 await _context.SaveChangesAsync(); // return the id of the user that was just created
                 
                 return comapny;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public CompanyModel CompanyById(int? id)
        {
            try
            {
                 CompanyModel company = new();
                 
                 if (id != null)
                 {
                     company = _context.Companies
                                     .Include(co => co.members)
                                     .Include(co => co.projects)
                                     .Include(co => co.invites)
                                     .SingleOrDefault(co => co.companyId == id);
                 }
                 return company;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public void DeleteCompany(CompanyModel compnay)
        {
            try
            {
                 _context.Companies.Remove(compnay);
                 _context.SaveChanges();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        // Not included in CONTROLLER
        public async Task<List<UserModel>> GetMembersAsync(int companyId)
        {
            try
            {
                 List<UserModel> res = await _context.Users.Where(u => u.companyId == companyId).ToListAsync();
                 return res;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
        public async Task<List<ProjectModel>> GetProjectsAsync(int companyId)
        {
            try
            {
                 List<ProjectModel> res = await _context.Projects.Where(u => u.companyId == companyId)
                                                                 .Include(p => p.members)
                                                                 .Include(p => p.bugs)
                                                                     .ThenInclude(b => b.developer)
                                                                 .Include(p => p.bugs)
                                                                     .ThenInclude(b => b.reporter)
                                                                 .Include(p => p.bugs)
                                                                     .ThenInclude(b => b.comments)
                                                                 .Include(p => p.bugs)
                                                                     .ThenInclude(b => b.severity)
                                                                 .Include(p => p.bugs)
                                                                     .ThenInclude(b => b.priority)
                                                                 .Include(p => p.bugs)
                                                                     .ThenInclude(b => b.status)
                                                                 .Include(p => p.bugs)
                                                                     .ThenInclude(b => b.type)
                                                                 .Include(p => p.bugs)
                                                                     .ThenInclude(b => b.comments)
                                                                 .Include(p => p.bugs)
                                                                     .ThenInclude(b => b.attachments)
                                                                 .Include(p => p.bugs)
                                                                     .ThenInclude(b => b.notifications)
                                                                 .Include(p => p.bugs)
                                                                     .ThenInclude(b => b.history)
                                                                 .ToListAsync();
                 return res;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
        public async Task<List<BugModel>> GetBugsAsync(int companyId)
        {
            try
            {
                 List<ProjectModel> projects = await GetProjectsAsync(companyId);
                 List<BugModel> res = projects.SelectMany(item => item.bugs).ToList();
                 return res;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
        // COMPANY END ----------------------

        // PROJECT START ----------------------
        public IEnumerable<ProjectModel> GetProjects()
        {
            return _context.Projects;
        }

        public async Task<ProjectModel> AddProj(ProjectModel proj)
        {
            try
            {
                 _context.Projects.Add(proj); // add project to the table
                 await _context.SaveChangesAsync(); // return the id of the user that was just created
                 
                 return proj;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<ProjectModel> ProjByIdAsync(int projId, int companyId)
        {
            try
            {
                 return await _context.Projects
                             .Include(p => p.bugs)
                             .Include(p => p.members)
                             .SingleOrDefaultAsync(p => p.projectId == projId && p.companyId == companyId);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public void DeleteProj(ProjectModel proj)
        {
            try
            {
                 _context.Projects.Remove(proj);
                 _context.SaveChanges();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        // not included in CONTROLLER
        public async Task<bool> AddPMAsync(string userId, int projectId)
        {
            // find currrent PM
            UserModel currentPM = await GetProjPMAsync(projectId);

            // Remove current PM
            if (currentPM != null)
            {
                try
                {
                     await DeletePMAsync(projectId);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"*** Error removing current PM. *** {ex.Message}");
                    return false;
                }
            }

            // Add new PM
            try
            {
                 await AddPMAsync(userId, projectId);
                 return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"*** Error adding current PM. *** {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddUserToProjAsync(string userId, int projectId)
        {
            // find user
            UserModel user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                // find project
                ProjectModel proj = await _context.Projects.FirstOrDefaultAsync(p => p.projectId == projectId);

                // if user is not in the project already
                if (!await IsUserOnProj(userId, projectId))
                {
                    try
                    {
                        proj.members.Add(user); // add to the project
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return false;
        }

        public async Task ArchiveProjAsync(ProjectModel proj)
        {
            proj.archived = true;
            _context.Update(proj);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProjectModel>> GetProjByCoAsync(int companyId)
        {
            List<ProjectModel> res = await _context.Projects
                                        .Where(p => p.companyId == companyId)
                                        .Include(p => p.members)
                                        .Include(p => p.bugs)
                                            .ThenInclude(b => b.reporter)
                                        .Include(p => p.bugs)
                                            .ThenInclude(b => b.developer)
                                        .Include(p => p.bugs)
                                            .ThenInclude(b => b.severity)
                                        .Include(p => p.bugs)
                                            .ThenInclude(b => b.priority)
                                        .Include(p => p.bugs)
                                            .ThenInclude(b => b.status)
                                        .Include(p => p.bugs)
                                            .ThenInclude(b => b.type)
                                        .Include(p => p.bugs)
                                            .ThenInclude(b => b.attachments)
                                        .Include(p => p.bugs)
                                            .ThenInclude(b => b.comments)
                                        .Include(p => p.bugs)
                                            .ThenInclude(b => b.history)
                                        .Include(p => p.bugs)
                                            .ThenInclude(b => b.notifications)
                                        .ToListAsync();
            return res;
        }

        public async Task<List<UserModel>> GetProjUsersExceptPMAsync(int projectId)
        {
            List<UserModel> users = new();

            List<UserModel> members = (
                await _context.Projects
                    .Include(p => p.members)
                    .FirstOrDefaultAsync(p => p.projectId == projectId)
            ).members.ToList();

            foreach(var user in members)
            {
                if(!await _userRoleService.verifyRole(user, Roles.ProjectManager.ToString()))
                {
                    members.Add(user);
                }
            }

            return members;
        }

        public async Task<List<ProjectModel>> GetArchivedProjAsync(int companyId)
        {
            List<ProjectModel> projects = await GetProjByCoAsync(companyId);
            List<ProjectModel> res = projects.Where(p => p.archived == true).ToList();
            return res;
        }

        public async Task<List<UserModel>> GetProjUsersByRoleAsync(int projectId, string role)
        {
            List<UserModel> members = new();

            ProjectModel proj = await _context.Projects
                            .Include(p => p.members)
                            .FirstOrDefaultAsync(p => p.projectId == projectId);
            
            foreach (var user in proj.members)
            {
                if (await _userRoleService.verifyRole(user, role))
                {
                    members.Add(user);
                }
            }

            return members;
        }

        public async Task<List<UserModel>> GetProjDevAsync(int projectId)
        {
            return await GetProjUsersByRoleAsync(projectId, Roles.Developer.ToString());
        }

        public async Task<UserModel> GetProjPMAsync(int projectId)
        {
            ProjectModel proj = await _context.Projects
                                .Include(p => p.members)
                                .FirstOrDefaultAsync(p => p.projectId == projectId);
            
            foreach (var user in proj?.members) // if proj is null, dont run code
            {
                if (await _userRoleService.verifyRole(user, Roles.ProjectManager.ToString()))
                    return user;
            }
            return null;
        }

        public async Task<List<UserModel>> GetReportersOnProjAsync(int projectId)
        {
            return await GetProjUsersByRoleAsync(projectId, Roles.Reporter.ToString());
        }

        public async Task<List<UserModel>> GetUserNotOnProjAsync(int projectId, int companyId)
        {
            List<UserModel> users = await _context.Users.Where(u => u.projects.All(p => p.projectId != projectId)).ToListAsync();

            return users.Where(u => u.companyId == companyId).ToList();
        }
        public async Task<List<ProjectModel>> GetUserProjectsAsync(string userId)
        {
            try
            {
                List<ProjectModel> userProjects = (
                    await _context.Users // get info from user table
                        .Include(u => u.projects)
                            .ThenInclude(p => p.company)
                        .Include(u => u.projects)
                            .ThenInclude(p => p.members)
                        .Include(u => u.projects)
                            .ThenInclude(p => p.bugs)
                                .ThenInclude(b => b.reporter)
                        .Include(u => u.projects)
                            .ThenInclude(p => p.bugs)
                                .ThenInclude(b => b.developer)
                        .Include(u => u.projects)
                            .ThenInclude(p => p.bugs)
                                .ThenInclude(b => b.severity)
                        .Include(u => u.projects)
                            .ThenInclude(p => p.bugs)
                                .ThenInclude(b => b.priority)
                        .Include(u => u.projects)
                            .ThenInclude(p => p.bugs)
                                .ThenInclude(b => b.status)
                        .Include(u => u.projects)
                            .ThenInclude(p => p.bugs)
                                .ThenInclude(b => b.type)
                        .FirstOrDefaultAsync(u => u.Id == userId)
                ).projects.ToList(); // converting to project type

                return userProjects;
            
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"*** Error Getting user projects list. *** {ex.Message}");
                throw;
            }
        }

        public async Task<bool> IsUserOnProj(string userId, int projectId)
        {
            ProjectModel proj = await _context.Projects.SingleOrDefaultAsync(p => p.projectId == projectId);
                            // .Include(p => p.members)
                            // .SingleOrDefaultAsync(p => p.projectId == projectId);
            bool res = false;

            if (proj != null)
            {
                res = proj.members.Any(m => m.Id == userId);
            }
            return res;
        }

        public async Task DeletePMAsync(int projectId)
        {
            try
            {
                // Find project
                ProjectModel proj = await _context.Projects
                                    .Include(p => p.members)
                                    .FirstOrDefaultAsync(p => p.projectId == projectId);
                // look for the user with that role
                foreach(var user in proj?.members) // ? null check
                {
                    if (await _userRoleService.verifyRole(user, Roles.ProjectManager.ToString()))
                    {
                        await DeleteUserAsync(user.Id, projectId);
                    }
                }
            }
            catch (System.Exception)
            {
                
                throw;
            }
            
        }
        public async Task DeleteUserFromProjByRoleAsync(string role, int projectId)
        {
            try
            {
                List<UserModel> members = await GetProjUsersByRoleAsync(projectId, role);
                ProjectModel proj = await _context.Projects
                        .FirstOrDefaultAsync(p => p.projectId == projectId);

                foreach (var user in members)
                {
                    proj.members.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"*** Error removing users from project. {ex.Message}");
                throw;
            }

        }
        public async Task DeleteUserAsync(string userId, int projectId)
        {
            try
            {
                // find user and project
                UserModel user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                ProjectModel proj = await _context.Projects.FirstOrDefaultAsync(p => p.projectId == projectId);

                // check the user exist
                // the method also checks if project exist
                if (await IsUserOnProj(userId, projectId))
                {
                    proj.members.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            { 
                throw;
            }
        }
        public async Task UpdateProjAsync(ProjectModel proj)
        {
            _context.Update(proj);
            await _context.SaveChangesAsync();
        }

        // PROJECT END ---------------------

        // BUG START -----------------------
        public async Task<List<BugModel>> GetBugsByCoAsync(int companyId)
        {
            try
            {
                List<BugModel> bugs = await _context.Projects
                                            .Where(p => p.companyId == companyId)
                                            .SelectMany(p => p.bugs)
                                                .Include(b => b.reporter)
                                                .Include(b => b.developer)
                                                .Include(b => b.severity)
                                                .Include(b => b.priority)
                                                .Include(b => b.status)
                                                .Include(b => b.type)
                                                .Include(b => b.attachments)
                                                .Include(b => b.comments)
                                                .Include(p => p.notifications)
                                                .Include(p => p.history)
                                            .ToListAsync();
                
                return bugs;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<BugModel> AddBug(BugModel bug)
        {
            try
            {
                 _context.Bugs.Add(bug);
                 await _context.SaveChangesAsync();
     
                 return bug;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<BugModel> BugByIdAsync(int id)
        {
            return await _context.Bugs.FirstOrDefaultAsync(item => item.bugId == id);
        }

        // not in CONTROLLER
        public async Task UpdateBugAsync(BugModel bug)
        {
            try
            {
                 _context.Update(bug);
                 await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task ArchiveBugAsync(BugModel bug)
        {
            try
            {
                 bug.archived = true;
                 _context.Update(bug);
                 await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task AssignBugAsync(int bugId, string userId)
        {
            // find bug
            BugModel bug = await _context.Bugs.FirstOrDefaultAsync(b => b.bugId == bugId);

            if (bug != null)
            {
                try
                {
                    bug.developerId = userId;
                    bug.statusId = (await LookupBugStatusIdAsync("Assigned")).Value;
                    await _context.SaveChangesAsync();
                }
                catch (System.Exception)
                {
                    
                    throw;
                }
            }
        }

        public async Task<List<BugModel>> GetArchivedBugAsync(int companyId)
        {
            return (await GetBugsByCoAsync(companyId)).Where(b => b.archived == true).ToList();
        }

        public async Task<List<BugModel>> GetBugsBySeverityAsync(int companyId, string severity)
        {
            int bugSeverityId = (await LookupBugSeverityIdAsync(severity)).Value;

            try
            {
                List<BugModel> bugs = await GetBugsByCoAsync(companyId);
                return bugs.Where(b => b.severityId == bugSeverityId).ToList();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<BugModel>> GetBugsByPriorityAsync(int companyId, string priority)
        {
            int bugPrirityId = (await LookupBugPriorityIdAsync(priority)).Value;

            try
            {
                List<BugModel> bugs = await GetBugsByCoAsync(companyId);
                return bugs.Where(b => b.priorityId == bugPrirityId).ToList();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<BugModel>> GetBugsByStatusAsync(int companyId, string status)
        {
            int bugStatusId = (await LookupBugStatusIdAsync(status)).Value;

            try
            {
                List<BugModel> bugs = await _context.Projects.Where(p => p.companyId == companyId)
                                                .SelectMany(p => p.bugs)
                                                    .Include(b => b.reporter)
                                                    .Include(b => b.developer)
                                                    .Include(b => b.severity)
                                                    .Include(b => b.priority)
                                                    .Include(b => b.status)
                                                    .Include(b => b.type)
                                                    .Include(b => b.attachments)
                                                    .Include(b => b.comments)
                                                    .Include(b => b.notifications)
                                                    .Include(b => b.history)
                                                .Where(b => b.statusId == bugStatusId)
                                                .ToListAsync();
                return bugs;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<BugModel>> GetBugsByTypeAsync(int companyId, string type)
        {
            int bugTypeId = (await LookupBugTypeIdAsync(type)).Value;

            try
            {
                List<BugModel> bugs = await _context.Projects.Where(p => p.companyId == companyId)
                                                .SelectMany(p => p.bugs)
                                                    .Include(b => b.reporter)
                                                    .Include(b => b.developer)
                                                    .Include(b => b.severity)
                                                    .Include(b => b.priority)
                                                    .Include(b => b.status)
                                                    .Include(b => b.type)
                                                    .Include(b => b.attachments)
                                                    .Include(b => b.comments)
                                                    .Include(b => b.notifications)
                                                    .Include(b => b.history)
                                                .Where(b => b.typeId == bugTypeId)
                                                .ToListAsync();
                return bugs;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<UserModel> GetBugDeveloperAsync(int bugId, int companyId)
        {
            try
            {
                BugModel bug = (await GetBugsByCoAsync(companyId)).FirstOrDefault(b => b.bugId == bugId);
                return bug?.developer;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<BugModel>> GetBugsByRoleAsync(string role, string userId, int companyId)
        {
            List<BugModel> bugs = new();

            try
            {
                 if(role == Roles.Admin.ToString())
                 {
                     bugs = await GetBugsByCoAsync(companyId);
                 }
                 else if (role == Roles.Reporter.ToString())
                 {
                     bugs = (await GetBugsByCoAsync(companyId)).Where(b => b.reporterId == userId).ToList();
                 }
                 else if (role == Roles.Developer.ToString())
                 {
                     bugs = (await GetBugsByCoAsync(companyId)).Where(b => b.developerId == userId).ToList();
                 }
                 else if (role == Roles.ProjectManager.ToString())
                 {
                     bugs = await GetBugsByUserIdAsync(userId, companyId);
                 }

                 return bugs;
            }
            catch (System.Exception)
            {
                
                throw;
            }

        }

        public async Task<List<BugModel>> GetBugsByUserIdAsync(string userId, int companyId)
        {
            List<BugModel> bugs = new();
            UserModel user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            try
            {
                 if (await _userRoleService.verifyRole(user, Roles.Admin.ToString()))
                {
                    bugs = (await GetProjByCoAsync(companyId)).SelectMany(p => p.bugs).ToList();
                }
                else if (await _userRoleService.verifyRole(user, Roles.Reporter.ToString()))
                {
                    bugs = (await GetBugsByCoAsync(companyId)).Where(b => b.reporterId == userId).ToList();
                }
                else if (await _userRoleService.verifyRole(user, Roles.Developer.ToString()))
                {
                    bugs = (await GetBugsByCoAsync(companyId)).Where(b => b.developerId == userId).ToList();
                }
                else if (await _userRoleService.verifyRole(user, Roles.ProjectManager.ToString()))
                {
                    bugs = (await GetUserProjectsAsync(userId)).SelectMany(p => p.bugs).ToList();
                }

                return bugs;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<BugModel>> GetProjBugsByRoleAsync(string role, string userId, int projectId, int companyId)
        {
            List<BugModel> bugs = new();

            try
            {
                bugs = (await GetBugsByRoleAsync(role, userId, companyId)).Where(p => p.projectId == projectId).ToList();
                return bugs;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<BugModel>> GetProjBugsByStatusAsync(string status, int companyId, int projectId)
        {
            try
            {
                List<BugModel> bugs = (await GetBugsByStatusAsync(companyId, status)).Where(p => p.projectId == projectId).ToList();
                return bugs;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<BugModel>> GetProjBugsBySeverityAsync(string severity, int companyId, int projectId)
        {
            try
            {
                 List<BugModel> bugs = (await GetBugsBySeverityAsync(companyId, severity)).Where(p => p.projectId == projectId).ToList();
                 return bugs;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<BugModel>> GetProjBugsByPriorityAsync(string priority, int companyId, int projectId)
        {
            try
            {
                 List<BugModel> bugs = (await GetBugsByPriorityAsync(companyId, priority)).Where(p => p.projectId == projectId).ToList();
                 return bugs;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<BugModel>> GetProjBugsByTypeAsync(string type, int companyId, int projectId)
        {
            try
            {
                 List<BugModel> bugs = (await GetBugsByTypeAsync(companyId, type)).Where(p => p.projectId == projectId).ToList();
                 return bugs;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
    

        public async Task<int?> LookupBugPriorityIdAsync(string priority)
        {
            try
            {
                PriorityModel res = await _context.Priorities.FirstOrDefaultAsync(p => p.priorityName == priority);
                return res?.priorityId;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<int?> LookupBugSeverityIdAsync(string severity)
        {
            try
            {
                SeverityModel res = await _context.Severities.FirstOrDefaultAsync(p => p.severityName == severity);
                return res?.severityId;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<int?> LookupBugStatusIdAsync(string status)
        {
            try
            {
                StatusModel res = await _context.Statuses.FirstOrDefaultAsync(p => p.statusName == status);
                return res?.statusId;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<int?> LookupBugTypeIdAsync(string type)
        {
            try
            {
                TypeModel res = await _context.Types.FirstOrDefaultAsync(p => p.typeName == type);
                return res?.typeId;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        // BUG END -------------------------

        // Comment START -----------------------
        public IEnumerable<CommentModel> GetComment()
        {
            return _context.Comments;
        }

        public async Task<CommentModel> AddComment(CommentModel comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public CommentModel CommentById(int id)
        {
            return _context.Comments.SingleOrDefault(item => item.commentId == id);
        }

        public void DeleteComment(CommentModel comment)
        {
            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }
        // Comment END -------------------------

        // Attachment START -----------------------
        public IEnumerable<AttachmentModel> GetAttachment()
        {
            return _context.Attachments;
        }

        public async Task<AttachmentModel> AddAttachment(AttachmentModel attahment)
        {
            _context.Attachments.Add(attahment);
            await _context.SaveChangesAsync();

            return attahment;
        }

        public AttachmentModel AttachmentById(int id)
        {
            return _context.Attachments.SingleOrDefault(item => item.attachmentId == id);
        }

        public void DeleteAttachment(AttachmentModel attahment)
        {
            _context.Attachments.Remove(attahment);
            _context.SaveChanges();
        }
        
        // Attachment END -------------------------

        // Severity START -------------------------

        public IEnumerable<SeverityModel> GetSeverity()
        {
             return _context.Severities;
        }

        public async Task<SeverityModel> AddSeverity(SeverityModel severity)
        {
            _context.Severities.Add(severity);
            await _context.SaveChangesAsync();

            return severity;
        }

        public SeverityModel SeverityById(int id)
        {
            return _context.Severities.SingleOrDefault(item => item.severityId == id);
        }

        public void DeleteSeverity(SeverityModel severity)
        {
            _context.Severities.Remove(severity);
            _context.SaveChanges();
        }
        // Severity END -------------------------

        // Priority START -------------------------

        public IEnumerable<PriorityModel> GetPriority()
        {
             return _context.Priorities;
        }

        public async Task<PriorityModel> AddPriority(PriorityModel priority)
        {
            _context.Priorities.Add(priority);
            await _context.SaveChangesAsync();

            return priority;
        }

        public PriorityModel PriorityById(int id)
        {
            return _context.Priorities.SingleOrDefault(item => item.priorityId == id);
        }

        public void DeletePriority(PriorityModel priority)
        {
            _context.Priorities.Remove(priority);
            _context.SaveChanges();
        }
        // Priority END -------------------------

        // Status START -------------------------

        public IEnumerable<StatusModel> GetStatus()
        {
             return _context.Statuses;
        }

        public async Task<StatusModel> AddStatus(StatusModel status)
        {
            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();

            return status;
        }

        public StatusModel StatusById(int id)
        {
            return _context.Statuses.SingleOrDefault(item => item.statusId == id);
        }

        public void DeleteStatus(StatusModel status)
        {
            _context.Statuses.Remove(status);
            _context.SaveChanges();
        }
        // Status END -------------------------

        // Type START -------------------------

        public IEnumerable<TypeModel> GetTypes()
        {
             return _context.Types;
        }

        public async Task<TypeModel> AddType(TypeModel type)
        {
            _context.Types.Add(type);
            await _context.SaveChangesAsync();

            return type;
        }

        public TypeModel TypeById(int id)
        {
            return _context.Types.SingleOrDefault(item => item.typeId == id);
        }

        public void DeleteType(TypeModel type)
        {
            _context.Types.Remove(type);
            _context.SaveChanges();
        }
        // Type END -------------------------

        // History START -------------------------

        public IEnumerable<HistModel> GetHist()
        {
             return _context.History;
        }

        public async Task<HistModel> AddHist(HistModel hist)
        {
            _context.History.Add(hist);
            await _context.SaveChangesAsync();

            return hist;
        }

        public HistModel HistById(int id)
        {
            return _context.History.SingleOrDefault(item => item.historyId == id);
        }

        public void DeleteHist(HistModel hist)
        {
            _context.History.Remove(hist);
            _context.SaveChanges();
        }

        // not included in CONTROLLER
        public async Task AddHistAsync(BugModel oldBug, BugModel newBug, string userId)
        {
            // When Bug is new
            if (oldBug == null && newBug != null)
            {
                HistModel hist = new()
                {
                    bugId = newBug.bugId,
                    changedItem = "",
                    oldValue = "",
                    newValue = "",
                    modDate = DateTimeOffset.Now,
                    userId = userId
                };
                
                try
                {
                    await _context.History.AddAsync(hist);
                    await _context.SaveChangesAsync();
                }
                catch (System.Exception)
                {
                    
                    throw;
                }
            }
            else // when Bug is old
            {
                // check title
                if (oldBug.title != newBug.title)
                {
                    HistModel hist = new()
                    {
                        bugId = newBug.bugId,
                        changedItem = "Title",
                        oldValue = oldBug.title,
                        newValue = newBug.title,
                        modDate = DateTimeOffset.Now,
                        userId = userId
                    };

                    await _context.History.AddAsync(hist);
                }

                // check severity
                if (oldBug.severityId != newBug.severityId)
                {
                    HistModel hist = new()
                    {
                        bugId = newBug.bugId,
                        changedItem = "Severity",
                        oldValue = oldBug.severity.severityName,
                        newValue = newBug.severity.severityName,
                        modDate = DateTimeOffset.Now,
                        userId = userId
                    };

                    await _context.History.AddAsync(hist);
                }

                // check priority
                if (oldBug.priorityId != newBug.priorityId)
                {
                    HistModel hist = new()
                    {
                        bugId = newBug.bugId,
                        changedItem = "Priority",
                        oldValue = oldBug.priority.priorityName,
                        newValue = newBug.priority.priorityName,
                        modDate = DateTimeOffset.Now,
                        userId = userId
                    };

                    await _context.History.AddAsync(hist);
                }

                // check status
                if (oldBug.statusId != newBug.statusId)
                {
                    HistModel hist = new()
                    {
                        bugId = newBug.bugId,
                        changedItem = "Status",
                        oldValue = oldBug.status.statusName,
                        newValue = newBug.status.statusName,
                        modDate = DateTimeOffset.Now,
                        userId = userId
                    };

                    await _context.History.AddAsync(hist);
                }

                // check type
                if (oldBug.typeId != newBug.typeId)
                {
                    HistModel hist = new()
                    {
                        bugId = newBug.bugId,
                        changedItem = "Type",
                        oldValue = oldBug.type.typeName,
                        newValue = newBug.type.typeName,
                        modDate = DateTimeOffset.Now,
                        userId = userId
                    };

                    await _context.History.AddAsync(hist);
                }

                 // check developer
                if (oldBug.developerId != newBug.developerId)
                {
                    HistModel hist = new()
                    {
                        bugId = newBug.bugId,
                        changedItem = "Developer",
                        oldValue = oldBug.developer?.fullName ?? "Not Assigned", // ?? (if not null assign left value else assign right value)
                        newValue = newBug.developer?.fullName,
                        modDate = DateTimeOffset.Now,
                        userId = userId
                    };

                    await _context.History.AddAsync(hist);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<HistModel>> GetCoBugsHistAsync(int companyId)
        {
            try
            {
                 List<ProjectModel> proj = (await _context.Companies
                                             .Include(c => c.projects)
                                                 .ThenInclude(p => p.bugs)
                                                     .ThenInclude(b => b.history)
                                                         .ThenInclude(h => h.user)
                                             .FirstOrDefaultAsync(c => c.companyId == companyId))
                                             .projects.ToList();
     
                 List<BugModel> bugs = proj.SelectMany(p => p.bugs).ToList();
                 List<HistModel> hist = bugs.SelectMany(b => b.history).ToList();
                 return hist;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<HistModel>> GetProjBugsHistAsync(int projId, int companyId)
        {
            try
            {
                ProjectModel proj = await _context.Projects.Where(p => p.companyId == companyId)
                                        .Include(p => p.bugs)
                                            .ThenInclude(b => b.history)
                                                .ThenInclude(h => h.user)
                                        .FirstOrDefaultAsync(p => p.projectId == projId);

                List<HistModel> hist = proj.bugs.SelectMany(b => b.history).ToList();
                return hist;            
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
        // History END -------------------------
    }
}