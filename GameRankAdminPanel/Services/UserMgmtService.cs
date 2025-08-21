using System.Net;
using GameRankAdminPanel.Models;
using Microsoft.AspNetCore.Identity;
using GameRankAdminPanel.Data;
using GameRankAdminPanel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;


namespace GameRankAdminPanel.Services;

public class UserMgmtService : IUserMgmtService
{
    
    private UserManager<IdentityUser> _userManager;
    private ApplicationDbContext _context;
    private readonly AdminPanelDBContext _adminPanelDBContext;

    public UserMgmtService(UserManager<IdentityUser> userManager, AdminPanelDBContext adminPanelDBContext , ApplicationDbContext context)
    {
        _context =  context;
        _adminPanelDBContext = adminPanelDBContext;
        _userManager = userManager;
    }

    public async Task<List<UserDtOs.UserData>> GetUsers(string users)
    {
        var FindsUsers = await _context.Users
            .Where(x => x.UserName.StartsWith(users))
            .Select(x => new {x.UserName, x.Id, x.Email})
            .ToListAsync();

        var GetIDs = FindsUsers.Select(x => x.Id).ToList();


        var GetRoles = await _context.UserRoles
            .Where(x => GetIDs.Contains(x.UserId))
            .ToListAsync();
        var roleIds = GetRoles.Select(x => x.RoleId).Distinct().ToList();

        var roles = await _context.Roles
            .Where(r => roleIds.Contains(r.Id))
            .ToDictionaryAsync(r => r.Id, r => r.Name);

        var GetAdminData = await _adminPanelDBContext.UserDataAdmin
            .Where(x => GetIDs.Contains(x.Id))
            .ToListAsync(); 

        var result = FindsUsers
            .Select(user => new UserDtOs.UserData  
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                
                Roles = GetRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Select(ur => roles[ur.RoleId]) 
                    .ToList(),
                IPAddress = GetAdminData.FirstOrDefault(a => a.Id == user.Id)?.IPAdress,
                Status = GetAdminData.FirstOrDefault(a => a.Id == user.Id)?.Status
            })
            .ToList();
            Console.WriteLine(GetAdminData.Count);
        return result;

    }

    public async Task<UserDtOs.Result> GetBannedUsers()
    {
        var getBannedUsers = _adminPanelDBContext.UserDataAdmin.Where(x => x.Status == "banned")
            .Select(x => new { x.Id, x.IPAdress , x.UserName });

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
            await _userManager.AddClaimAsync(user, new Claim("Banned", "true"));
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

    public async Task<UserDtOs.ActionResult> ChangeUserRole(IdentityUser user , string newRole , string senderId , string senderName)
    {
        try
        {
            var role = await _userManager.GetRolesAsync(user);
            
            if (role.Contains("Admin"))
            {
                Console.WriteLine("дошли");
                var senderip = _adminPanelDBContext.UserDataAdmin.Where(x => x.Id == senderId).Select(x => x.IPAdress)
                    .FirstOrDefault();
                Console.WriteLine($" АЙПИИИИИИИ {senderId}");
                SuspectUsers suspectUsers = new SuspectUsers
                {
                    Id = senderId,
                    IpAdress = senderip,
                    cause = "Превышение полномочий",
                    Username = senderName,
                };
                _adminPanelDBContext.Add(suspectUsers);
                _adminPanelDBContext.SaveChanges();
                Console.WriteLine("дошли");
                return new UserDtOs.ActionResult
                {
                    Success = false,
                    Message =
                        $"Пользователь{user.UserName} является Администратором . Изменить его роль может только Создатель , обратитесь к нему "
                };
                
                

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
            Console.WriteLine(ex.Message);
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
            await _userManager.RemoveClaimAsync(user, new Claim("Banned", "true"));
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