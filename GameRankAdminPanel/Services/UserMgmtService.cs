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

    public UserMgmtService(UserManager<IdentityUser> userManager, AdminPanelDBContext adminPanelDBContext)
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
        var getSuspectUsers = _adminPanelDBContext.SuspectUsers.AsQueryable();
        return new UserDtOs.Result
        {
            Success = true,
            Message = getSuspectUsers
        };

    }

    public async Task<UserDtOs.ActionResult> BanUser(IdentityUser user)
    {
        var Id = user.Id;
        var GetUserData =
            await _adminPanelDBContext.UserDataAdmin.FirstOrDefaultAsync(x => x.Id == Id && x.Status != "banned");
        if (GetUserData != null)
        {
            GetUserData.Status = "banned";
            _adminPanelDBContext.SaveChanges();
            return new UserDtOs.ActionResult()
            {
                Success = true,
                Message = $"Пользователь{user.UserName} был успешно забанен"
            };
        }
        else
        {
            return new UserDtOs.ActionResult
            {
                Success = false,
                Message = $"Пользователь {user.UserName} не был забанен . Попробуйте позже"
            };
        }

    }

    public async Task<UserDtOs.ActionResult> ChangeUserRole(IdentityUser user , string newRole)
    {
        try
        {
            var role = await _userManager.GetRolesAsync(user);
            if (role.Contains("admin"))
            {
                return new UserDtOs.ActionResult
                {
                    Success = false,
                    Message =
                        $"Пользователь{user.UserName} является Администратором . Изменить его роль может только Создатель , обратитесь к нему "
                };
                // закидка в саспект юзер

            }
            else
            {
                var oldRole = "User"; // костыль 
                 await _userManager.RemoveFromRoleAsync(user, oldRole);
                 await _userManager.AddToRoleAsync(user, newRole);
                return new UserDtOs.ActionResult
                {
                    Success = true,
                    Message = $"Пользователю  {user.UserName} была успешно выдана роль :  {newRole}"
                };
            }
        }
        catch (Exception ex)
        {
            return new UserDtOs.ActionResult
            {
                Success = false,
                Message = "Произошла ошибка . Попробуйте позже"
            };
            // Логгер сюда
            
        }

    }

    public async Task<UserDtOs.ActionResult> UnbanUser(IdentityUser user)
    {
        var Id = user.Id;
        var GetUserData =
            await _adminPanelDBContext.UserDataAdmin.FirstOrDefaultAsync(x => x.Id == Id && x.Status != "active");
        if (GetUserData != null)
        {
            GetUserData.Status = "active";
            _adminPanelDBContext.SaveChanges();
            return new UserDtOs.ActionResult()
            {
                Success = true,
                Message = $"User {user.UserName} has been unbanned"
            };
        }
        else
        {
            return new UserDtOs.ActionResult
            {
                Success = false,
                Message = $"User {user.UserName} has not been unbanned . Try again later"
            };
        }
    }
}