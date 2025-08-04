using GameRankAdminPanel.Models;
using Microsoft.AspNetCore.Identity;

namespace GameRankAdminPanel.Interfaces;

public interface IUserMgmtService
{
    public Task<UserDtOs.UserData> GetUsers(IdentityUser user);
    public Task<UserDtOs.Result> GetBannedUsers();
    public Task<UserDtOs.Result> SuspectUser();
    
  
    public Task<UserDtOs.ActionResult> BanUser(IdentityUser user);
   
    public Task<UserDtOs.ActionResult> ChangeUserRole(IdentityUser user , string newRole);
    
   public Task<UserDtOs.ActionResult> UnbanUser(IdentityUser user);
    
    
}