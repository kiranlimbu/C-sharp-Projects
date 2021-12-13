using System.Collections.Generic;
using System.Threading.Tasks;
using bugspotAPI.Models;
using Microsoft.AspNetCore.Http;

namespace bugspotAPI.Repositories
{
    public interface IProjectRepository
    {
        void SaveChanges(); 

        // COMPANY START ----------------------
        IEnumerable<CompanyModel> GetCompany();
        Task<CompanyModel> AddCompany(CompanyModel company);
        CompanyModel CompanyById(int? id);
        void DeleteCompany(CompanyModel company);
        // not included in CONTROLLER
        Task<List<UserModel>> GetMembersAsync(int companyId);
        Task<List<ProjectModel>> GetProjectsAsync(int companyId);
        Task<List<BugModel>> GetBugsAsync(int companyId);
        
        // COMPANY END ----------------------

        // PROJECT START ----------------------
        IEnumerable<ProjectModel> GetProjects();
        Task<ProjectModel> AddProj(ProjectModel proj);
        Task<ProjectModel> ProjByIdAsync(int projId, int companyId);
        void DeleteProj(ProjectModel proj);
        // not included in CONTROLLER
        Task<bool> AddPMAsync(string userId, int projectId); //
        Task<bool> AddUserToProjAsync(string userId, int projectId); //
        Task ArchiveProjAsync(ProjectModel proj); //
        Task<List<ProjectModel>> GetProjByCoAsync(int companyId); //
        Task<List<UserModel>> GetProjUsersExceptPMAsync(int projectId); //
        Task<List<ProjectModel>> GetArchivedProjAsync(int companyId);//
        Task<List<UserModel>> GetProjDevAsync(int projectId); //
        Task<UserModel> GetProjPMAsync(int projectId); //
        Task<List<UserModel>> GetProjUsersByRoleAsync(int projectId, string role); //
        Task<List<UserModel>> GetReportersOnProjAsync(int projectId); //
        Task<List<UserModel>> GetUserNotOnProjAsync(int projectId, int companyId); //
        Task<List<ProjectModel>> GetUserProjectsAsync(string userId); //
        Task<bool> IsUserOnProj(string userId, int projectId); //
        Task DeletePMAsync(int projectId); //
        Task DeleteUserFromProjByRoleAsync(string role, int projectId); //
        Task DeleteUserAsync(string userId, int projectId); //
        Task UpdateProjAsync(ProjectModel proj); //

        // PROJECT END ----------------------

        // BUG START ----------------------
        Task<List<BugModel>> GetBugsByCoAsync(int companyId);
        Task<BugModel> AddBug(BugModel bug);
        Task<BugModel> BugByIdAsync(int id);

        // not in CONTROLLER
        Task UpdateBugAsync(BugModel bug); //
        Task ArchiveBugAsync(BugModel bug); // 
        Task AssignBugAsync(int bugId, string userId); //
        Task<List<BugModel>> GetArchivedBugAsync(int companyId); //
        Task<List<BugModel>> GetBugsBySeverityAsync(int companyId, string severity); //
        Task<List<BugModel>> GetBugsByPriorityAsync(int companyId, string priority); //
        Task<List<BugModel>> GetBugsByStatusAsync(int companyId, string status); //
        Task<List<BugModel>> GetBugsByTypeAsync(int companyId, string type); //
        Task<UserModel> GetBugDeveloperAsync(int ticketId, int companyId); //
        Task<List<BugModel>> GetBugsByRoleAsync(string role, string userId, int companyId); //
        Task<List<BugModel>> GetBugsByUserIdAsync(string userId, int companyId); //
        Task<List<BugModel>> GetProjBugsByRoleAsync(string role, string userId, int projectId, int companyId); //
        Task<List<BugModel>> GetProjBugsByStatusAsync(string status, int companyId, int projectId); //
        Task<List<BugModel>> GetProjBugsBySeverityAsync(string severity, int companyId, int projectId); //
        Task<List<BugModel>> GetProjBugsByPriorityAsync(string priority, int companyId, int projectId); //
        Task<List<BugModel>> GetProjBugsByTypeAsync(string type, int companyId, int projectId); //

        Task<int?> LookupBugPriorityIdAsync(string priority); //
        Task<int?> LookupBugSeverityIdAsync(string severity); // 
        Task<int?> LookupBugStatusIdAsync(string status); // 
        Task<int?> LookupBugTypeIdAsync(string type); // 

        // BUG END ----------------------

        // Comment START ----------------------
        IEnumerable<CommentModel> GetComment();
        Task<CommentModel> AddComment(CommentModel comment);
        CommentModel CommentById(int id);
        void DeleteComment(CommentModel comment);
        // Comment END ----------------------

        // Attachment START ----------------------
        IEnumerable<AttachmentModel> GetAttachment();
        Task<AttachmentModel> AddAttachment(AttachmentModel attahment);
        AttachmentModel AttachmentById(int id);
        void DeleteAttachment(AttachmentModel attahment);
        
        // Attachment END ----------------------

        // Severity START ----------------------
        IEnumerable<SeverityModel> GetSeverity();
        Task<SeverityModel> AddSeverity(SeverityModel severity);
        SeverityModel SeverityById(int id);
        void DeleteSeverity(SeverityModel severity);
        // Severity END ----------------------

        // Priority START ----------------------
        IEnumerable<PriorityModel> GetPriority();
        Task<PriorityModel> AddPriority(PriorityModel priority);
        PriorityModel PriorityById(int id);
        void DeletePriority(PriorityModel priority);
        // Priority END ----------------------

        // Status START ----------------------
        IEnumerable<StatusModel> GetStatus();
        Task<StatusModel> AddStatus(StatusModel status);
        StatusModel StatusById(int id);
        void DeleteStatus(StatusModel status);
        // Status END ----------------------

        // Type START ----------------------
        IEnumerable<TypeModel> GetTypes();
        Task<TypeModel> AddType(TypeModel type);
        TypeModel TypeById(int id);
        void DeleteType(TypeModel type);
        // Type END ----------------------

        // History START ----------------------
        IEnumerable<HistModel> GetHist();
        Task<HistModel> AddHist(HistModel hist);
        HistModel HistById(int id);
        void DeleteHist(HistModel hist);

        // not included in CONTROLLER
        Task AddHistAsync(BugModel oldBug, BugModel newBug, string userId); //
        Task<List<HistModel>> GetProjBugsHistAsync(int projId, int companyId);
        Task<List<HistModel>> GetCoBugsHistAsync(int companyId); //
        // History END ----------------------
    }
}