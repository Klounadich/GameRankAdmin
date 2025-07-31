using GameRankAdminPanel.Models;
using Microsoft.AspNetCore.Identity;

namespace GameRankAdminPanel.Interfaces;

public interface IUserMgmtService
{
    public Task<UserDtOs.UserData> GetUsers(IdentityUser user);
    public Task<UserDtOs.Result> GetBannedUsers();
    public Task<UserDtOs.Result> SuspectUser();
    /*
    public Task<UserDtOs.Result> ChangeUserRole(UserDtOs.UserData userData);
    public Task<UserDtOs.Result> BanUser(UserDtOs.UserData userData);
    public Task<UserDtOs.Result> UnbanUser(UserDtOs.UserData userData);
    public Task<UserDtOs.Result> DeleteUser(UserDtOs.UserData userData);
    */
}