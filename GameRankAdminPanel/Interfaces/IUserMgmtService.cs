using GameRankAdminPanel.Models;

namespace GameRankAdminPanel.Interfaces;

public interface IUserMgmtService
{
    public Task<UserDtOs.Result> GetUsers(UserDtOs.UserData userData);
    public Task<UserDtOs.Result> GetBannedUsers();
    public Task<UserDtOs.Result> SuscpectUser();
    
    public Task<UserDtOs.Result> ChangeUserRole(UserDtOs.UserData userData);
    public Task<UserDtOs.Result> BanUser(UserDtOs.UserData userData);
    public Task<UserDtOs.Result> UnbanUser(UserDtOs.UserData userData);
    public Task<UserDtOs.Result> DeleteUser(UserDtOs.UserData userData);
    
}