using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bugspotAPI.Models;

namespace bugspotAPI.Repositories
{
    public interface IUserRepository
    {
        // USER START -----------------
        Task<UserModel> Create(UserModel user);
        Task<UserModel> GetByEmailAsync(string email);
        UserModel GetById(string id);
        void DeleteUser(UserModel user);
        IEnumerable<UserModel> GetMembers();
        Task UpdateUserAsync(UserModel user);
        // USER END -----------------

        // Notification START ----------------------
        Task<List<NotifiModel>> GetReceivedNotifiAsync(string userId); ///
        Task<List<NotifiModel>> GetSentNotifiAsync(string userId); ///
        Task AddNotifiAsync(NotifiModel notifi); ///
        // not in CONTROLLER
        Task SendEmailNotifiByRoleAsync(NotifiModel notifi, int companyId, string role); //
        Task SendUsersNotifiAsync(NotifiModel notifi, List<UserModel> members); //
        Task<bool> SendEmailNotifiAsync(NotifiModel notifi, string emailSubject); //

        // Notification END ----------------------

        // INVITE START ----------------------
        Task<InviteModel> GetInviteAsync(int inviteId, int companyId); //
        Task<InviteModel> GetInviteAsync(Guid token, string email, int companyId); //
        Task AddInviteAsync(InviteModel invite); //
        // not in CONTROLLER
        Task<bool> AcceptInviteAsync(Guid? token, string userId, int companyId); //
        Task<bool> VerifyInviteAsync(Guid token, string emil, int companyId); //
        Task<bool> ValidateInviteCodeAsync(Guid? token); //

        // INVITE END ----------------------
    }
}