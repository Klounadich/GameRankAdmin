using GameRankAdminPanel.Interfaces;
using GameRankAdminPanel.Services.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using GameRankAdminPanel.Models;
using Microsoft.AspNetCore.Identity;

namespace GameRankAdminPanel.Controllers;

[ApiController]

[Route("api/admin")]
public class AdminMgmtController:ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RabbitMQService _rabbitMQService;
    private readonly IUserMgmtService  _userMgmtService;
    public AdminMgmtController(RabbitMQService rabbitMQService , UserManager<IdentityUser> userManager ,
        IUserMgmtService userMgmtService)
    {
        _userMgmtService = userMgmtService;
        _userManager = userManager;
        _rabbitMQService = rabbitMQService;
    }

    [HttpPost("get")]

    public async Task<IActionResult> GetUsers([FromBody] string Username)
    {
        
        var  user = await _userManager.FindByNameAsync(Username);
        if (user != null)
        {
            var result = await _userMgmtService.GetUsers(user);

            return Ok(new
            {
                Username = result.UserName,
                Role = result.Role,
                Status = result.Status,
                IpAdress = result.IPAdress

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
    public async Task<IActionResult> ChangeRole([FromBody] UserDtOs.ChangeRoleRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        var senderId = user.Id;
        if (user != null)
        {
            var result = await _userMgmtService.ChangeUserRole(user, request.newRole , senderId);
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