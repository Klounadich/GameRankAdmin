using GameRankAdminPanel.Models;
using Microsoft.AspNetCore.Identity;
using GameRankAdminPanel.Data;
using GameRankAdminPanel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameRankAdminPanel.Services;

public class UserMgmtService : IUserMgmtService
{   
    private UserManager<IdentityUser> _userManager;
    private readonly AdminPanelDBContext _adminPanelDBContext;
    
    public UserMgmtService(UserManager<IdentityUser> userManager , AdminPanelDBContext adminPanelDBContext)
    {
        _adminPanelDBContext = adminPanelDBContext;
        _userManager = userManager;
    }

    public async Task<UserDtOs.UserData> GetUsers(IdentityUser user)
    {
        var role = await _userManager.GetRolesAsync(user);
        var userDataAdmin = await _adminPanelDBContext.UserDataAdmin
            .Where(x => x.Id == user.Id)
            .FirstOrDefaultAsync();

        if (userDataAdmin == null)
        {
            Console.WriteLine("UserDataAdmin not found");
        }

        var id = user.Id;
        return new UserDtOs.UserData
        {
            UserName = user.UserName,
            Id = user.Id,
            IPAdress = await _adminPanelDBContext.UserDataAdmin
                .Where(x => x.Id == id) 
                .Select(x => x.IPAdress)
                .FirstOrDefaultAsync() ?? "0.0.0.0", 
            Status = await _adminPanelDBContext.UserDataAdmin
                .Where(x => x.Id == id)
                .Select(x => x.Status)
                .FirstOrDefaultAsync() ?? "banned",
            Role = role

        };
    }

    public async Task<UserDtOs.Result> GetBannedUsers()
    {
        var getBannedUsers = _adminPanelDBContext.UserDataAdmin.Where(x => x.Status == "banned")
            .Select(x => new { x.Id, x.IPAdress });
        
        var result = getBannedUsers;
        return new UserDtOs.Result
        {
            Success = true,
            Message = result
        };

    }

    public async Task<UserDtOs.Result> SuspectUser()
    {
        var  getSuspectUsers = _adminPanelDBContext.SuspectUsers.AsQueryable();
        return new UserDtOs.Result
        {
            Success = true,
            Message = getSuspectUsers
        };
        
    }
}