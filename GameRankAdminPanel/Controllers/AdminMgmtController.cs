using GameRankAdminPanel.Services.RabbitMQ;
using Microsoft.AspNetCore.Mvc;

namespace GameRankAdminPanel.Controllers;

[ApiController]

[Route("api/admin")]
public class AdminMgmtController:ControllerBase
{
    private readonly RabbitMQService _rabbitMQService;
    public AdminMgmtController(RabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }
    [HttpPost("get")]
    public async Task<IActionResult> GetRule()
    {
        _rabbitMQService.Get();
        return Ok();
    }
}