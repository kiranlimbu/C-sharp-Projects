using System.Collections.Generic;
using System.Threading.Tasks;
using bugspotAPI.Models;

namespace bugspotAPI.Repositories
{
    public interface IUserRoleService
    {
        // ROLE START ------------------------
        Task<bool> verifyRole(UserModel user, string role);
        Task<IEnumerable<string>> GetRolesAsync(UserModel user);
        Task<bool> AddUserToRoleAsync(UserModel user, string role);
        Task<bool> RemoveRoleAsync(UserModel user, string role);
        Task<bool> RemoveUserRolesAsync(UserModel user, IEnumerable<string> roles);
        Task<List<UserModel>> GetUsersInRoleAsync(string role, int companyId);
        Task<List<UserModel>> GetUsersNotInRoleAsync(string role, int companyId);
        Task<string> RoleByIdAsync(string roleId);
        // ROLE END --------------------------
    }
}