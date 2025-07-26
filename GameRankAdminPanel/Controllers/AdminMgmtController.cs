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
    public AdminMgmtController(RabbitMQService rabbitMQService , UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
        _rabbitMQService = rabbitMQService;
    }

    [HttpPost("get")]

    public async Task<UserDtOs.Result> GetUsers([FromBody] string Username)
    {
        var  user = await _userManager.FindByNameAsync(Username);
        
        // вызываем метод поиска 
    }

}