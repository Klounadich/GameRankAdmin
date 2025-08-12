using GameRankAdminPanel.Interfaces;
using GameRankAdminPanel.Services.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using GameRankAdminPanel.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using GameRankAdminPanel.Data;
using Microsoft.AspNetCore.Authorization;

namespace GameRankAdminPanel.Controllers;

[ApiController]

[Route("api/admin")]
public class AdminMgmtController:ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RabbitMQService _rabbitMQService;
    private readonly IUserMgmtService  _userMgmtService;
    private readonly AdminPanelDBContext _adminPanelDBContext;
    public AdminMgmtController(RabbitMQService rabbitMQService , UserManager<IdentityUser> userManager ,
        IUserMgmtService userMgmtService ,  AdminPanelDBContext adminPanelDBContext)
    {
        _adminPanelDBContext= adminPanelDBContext;
        _userMgmtService = userMgmtService;
        _userManager = userManager;
        _rabbitMQService = rabbitMQService;
    }

    [HttpPost("get")]

    public async Task<IActionResult> GetUsers([FromBody] string Username)
    {
        
        
        if (Username != null)
        {
            var result = await _userMgmtService.GetUsers(Username);
            

            return Ok(new
            {
                result

            });
        }
        else
        { 
            return BadRequest(new { Message = "Пользователь не найден . Обратитесь в поддержку" });
        }
    }

    [HttpGet("get-ban")]
    public async Task<IActionResult> GetBanUsers()
    {
        var result = await _userMgmtService.GetBannedUsers();
        if (result.Success)
        {
            return Ok(new
            {
                BanList = result.Message
            });
        }
        return BadRequest();
    }
    
    [HttpGet("get-suspect")]
    public async Task<IActionResult> GetSuspectUsers()
    {
        var result = await _userMgmtService.SuspectUser();
        if (result.Success)
        {
            return Ok(new
            {
                SuspectList = result.Message 
            });
        }
        return BadRequest();
    }

    [HttpPost("ignore-suspect")]
    public async Task<IActionResult> IgnoreSuspectUser([FromBody] string IPadress)
    {
        var suspects =  _adminPanelDBContext.SuspectUsers
            .Where(x => x.IpAdress == IPadress)
            .ToList();

        if (suspects.Any())
        {
            _adminPanelDBContext.SuspectUsers.RemoveRange(suspects);
            await _adminPanelDBContext.SaveChangesAsync();
            return Ok();
        }
        return NotFound();
    }


    [HttpPost("ban-user")]
    public async Task<IActionResult> BanUser([FromBody] string Username)
    {
        var user = await _userManager.FindByNameAsync(Username);
        if (user != null)
        {
            var result = await _userMgmtService.BanUser(user);
            if (result.Success)
            {
                return Ok(new
                {
                    Success = true, 
                    result.Message
                });
            }
            else
            {
                return BadRequest(new
                {
                    Success = false, 
                    result.Message
                });
            }
        }
        else
        {
            return BadRequest(new { Message = "Пользователь не найден . Обратитесь в поддержку" });
        }
    }
    [HttpPost("unban-user")]
    public async Task<IActionResult> UnBanUser([FromBody] string Username)
    {
        var user = await _userManager.FindByNameAsync(Username);
        if (user != null)
        {
            var result = await _userMgmtService.UnbanUser(user);
            if (result.Success)
            {
                return Ok(new
                {
                    Success = true, 
                    result.Message
                });
            }
            else
            {
                return BadRequest(new
                {
                    Success = false, 
                    result.Message
                });
            }
        }
        else
        {
            return BadRequest(new { Message = "Пользователь не найден . Обратитесь в поддержку" });
        }
    }

    [HttpPost("change-role")]
    [Authorize]
    public async Task<IActionResult> ChangeRole([FromBody] UserDtOs.ChangeRoleRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
        var senderName = User.FindFirstValue(ClaimTypes.Name).ToString();
        if (user != null)
        {
            var result = await _userMgmtService.ChangeUserRole(user, request.newRole , senderId , senderName);
            if (result.Success)
            {
                return Ok(new
                {
                    Success = true,
                    result.Message
                });
            }
            
            return BadRequest(new
            {
                success = false,
                result.Message
                
            });
        }
        else
        {
            return BadRequest(new { Message = "Пользователь не найден " });
        }
    }

}