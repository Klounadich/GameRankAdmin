using GameRankAdminPanel.Models;
using Microsoft.AspNetCore.Identity;

namespace GameRankAdminPanel.Interfaces;

public interface IUserMgmtService
{
    public Task<List<UserDtOs.UserData>> GetUsers(string users);
    public Task<UserDtOs.Result> GetBannedUsers();
    public Task<UserDtOs.Result> SuspectUser();
    
  
    public Task<UserDtOs.ActionResult> BanUser(IdentityUser user);
   
    public Task<UserDtOs.ActionResult> ChangeUserRole(IdentityUser user , string newRole , string senderId , string senderName);
    
   public Task<UserDtOs.ActionResult> UnbanUser(IdentityUser user);
    
    
}