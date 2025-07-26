using GameRankAdminPanel.Models;

namespace GameRankAdminPanel.Interfaces;

public interface IDataAnalyticsService
{
    public Task<UserDtOs.Result> GetLogs();
    
}