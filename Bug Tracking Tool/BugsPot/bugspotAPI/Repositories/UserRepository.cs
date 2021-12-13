using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bugspotAPI.Data;
using bugspotAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace bugspotAPI.Repositories
{
    public class UserRepository : IUserRepository, IUserRoleService
    {
        private readonly BugspotContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly IEmailSender _emailSender;

        public UserRepository(BugspotContext context, 
                            RoleManager<IdentityRole> roleManager, 
                            UserManager<UserModel> userManager,
                            IEmailSender emailSender)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        // USERS START ---------------------------
        public async Task<UserModel> Create(UserModel user)
        {
            _context.Users.Add(user); // add user to the table
            await _context.SaveChangesAsync(); // return the user that was just created
            
            return user;
        }

        public void DeleteUser(UserModel user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public async Task<UserModel> GetByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(user => user.Email == email);
        }

        public UserModel GetById(string id)
        {
            return _context.Users.SingleOrDefault(user => user.Id == id);
        }

        public IEnumerable<UserModel> GetMembers()
        {
             return _context.Users;
        }
        
        public async Task UpdateUserAsync(UserModel user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        // USERS END ---------------------------

        // ROLE START ------------------------
        public async Task<bool> verifyRole(UserModel user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<IEnumerable<string>> GetRolesAsync(UserModel user)
        {
            IEnumerable<string> res = await _userManager.GetRolesAsync(user);
            return res;
        }

        public async Task<bool> AddUserToRoleAsync(UserModel user, string role)
        {
            bool res = (await _userManager.AddToRoleAsync(user, role)).Succeeded;

            return res;
        }

        public async Task<bool> RemoveRoleAsync(UserModel user, string role)
        {
            return (await _userManager.RemoveFromRoleAsync(user, role)).Succeeded;
        }

        public async Task<bool> RemoveUserRolesAsync(UserModel user, IEnumerable<string> roles)
        {
            return (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;
        }

        public async Task<List<UserModel>> GetUsersInRoleAsync(string role, int companyId)
        {
            List<UserModel> users = (await _userManager.GetUsersInRoleAsync(role)).ToList();
            List<UserModel> res = users.Where(item => item.companyId == companyId).ToList();
            return res;
        }

        public async Task<List<UserModel>> GetUsersNotInRoleAsync(string role, int companyId)
        {
            List<string> userIds = (await _userManager.GetUsersInRoleAsync(role)).Select(u => u.Id).ToList();
            // from User table retrun rows that does not contain userIds
            List<UserModel> roleUsers = _context.Users.Where(u => !userIds.Contains(u.Id)).ToList(); 
            List<UserModel> res = roleUsers.Where(item => item.companyId == companyId).ToList();
            return res;
        }

        public async Task<string> RoleByIdAsync(string roleId)
        {
            IdentityRole role = _context.Roles.Find(roleId);
            string res = await _roleManager.GetRoleNameAsync(role);
            return res;
        }
        

        // ROLE START ------------------------

        // NOTIFICATION START -------------------------

        public async Task<List<NotifiModel>> GetReceivedNotifiAsync(string userId)
        {
            try
            {
                 List<NotifiModel> notifications = await _context.Notifications
                                                     .Include(n => n.receiver)
                                                     .Include(n => n.sender)
                                                     .Include(n => n.bug)
                                                         .ThenInclude(b => b.project)
                                                     .Where(n => n.receiverId == userId)
                                                     .ToListAsync();
                 return notifications;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<NotifiModel>> GetSentNotifiAsync(string userId)
        {
            try
            {
                List<NotifiModel> notifications = await _context.Notifications
                                                    .Include(n => n.receiver)
                                                    .Include(n => n.sender)
                                                    .Include(n => n.bug)
                                                        .ThenInclude(b => b.project)
                                                    .Where(n => n.senderId == userId)
                                                    .ToListAsync();

                return notifications;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        
        public async Task AddNotifiAsync(NotifiModel notifi)
        {
            try
            {
                 await _context.Notifications.AddAsync(notifi);
                 await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        // not in CONTROLLER
        public async Task SendEmailNotifiByRoleAsync(NotifiModel notifi, int companyId, string role)
        {
            try
            {
                 List<UserModel> users = await GetUsersInRoleAsync(role, companyId);

                 foreach (var user in users)
                 {
                     notifi.receiverId = user.Id;
                     await SendEmailNotifiAsync(notifi, notifi.title);
                 }
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task SendUsersNotifiAsync(NotifiModel notifi, List<UserModel> members)
        {
            try
            {
                 foreach (var user in members)
                 {
                     notifi.receiverId = user.Id;
                     await SendEmailNotifiAsync(notifi, notifi.title);
                 }
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<bool> SendEmailNotifiAsync(NotifiModel notifi, string emailSubject)
        {
            UserModel user = await _context.Users.FirstOrDefaultAsync(u => u.Id == notifi.receiverId);

            if (user != null)
            {
                // Send Email
                try
                {
                    await _emailSender.SendEmailAsync(user.Email, emailSubject, notifi.message);
                    return true;
                }
                catch (System.Exception)
                {
                    
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        // NOTIFICATION END -------------------------

        // INVITE START -------------------------

        public async Task<InviteModel> GetInviteAsync(int inviteId, int companyId)
        {
             try
             {
                return await _context.Invites
                                .Where(i => i.companyId == i.companyId)
                                .Include(i => i.company)
                                .Include(i => i.project)
                                .Include(i => i.invitor)
                                .FirstOrDefaultAsync(i => i.inviteId == inviteId);
             }
             catch (System.Exception)
             {
                 
                 throw;
             }
        }

        public async Task<InviteModel> GetInviteAsync(Guid token, string email, int companyId)
        {
            try
            {
                return await _context.Invites
                                 .Where(i => i.companyId == i.companyId)
                                 .Include(i => i.company)
                                 .Include(i => i.project)
                                 .Include(i => i.invitor)
                                 .FirstOrDefaultAsync(i => i.companyToken == token && i.inviteeEmail == email);   
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task AddInviteAsync(InviteModel invite)
        {
            try
            {
                 await _context.Invites.AddAsync(invite);
                 await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        // not in CONTROLLER
        public async Task<bool> AcceptInviteAsync(Guid? token, string userId, int companyId)
        {
            InviteModel invite = await _context.Invites.FirstOrDefaultAsync(i => i.companyToken == token);

            if (invite != null)
            {
                try
                {
                     invite.IsValid = false; // invite can not be used again
                     invite.inviteeId = userId; // registration info stored
                     await _context.SaveChangesAsync();
                     return true;
                }
                catch (System.Exception)
                {
                    
                    throw;
                }
            }

            return false;
        }

        public async Task<bool> VerifyInviteAsync(Guid token, string email, int companyId)
        {
            try
            {
                // keyword Any returns bool value
                 return await _context.Invites
                            .Where(i => i.companyId == companyId)
                            .AnyAsync(i => i.companyToken == token && i.inviteeEmail == email);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<bool> ValidateInviteCodeAsync(Guid? token)
        {
            bool res = false;

            // check for null values
            if (token == null)
                return res;

            // find token
            InviteModel invite = await _context.Invites.FirstOrDefaultAsync(i => i.companyToken == token);

            if (invite != null)
            {
                // find invite date
                DateTime inviteDate = invite.inviteDate.DateTime;

                // custom validation of invite based on the date it was issued
                // In this case we are allowing an invite to be valid for 7 days
                bool validDate = (DateTime.Now - inviteDate).TotalDays <= 7;

                if (validDate)
                    res = invite.IsValid; // check invite's isValid state
            }

            return res;
        }

        // INVITE END -------------------------



    }
}