namespace GameRankAdminPanel.Models;

public class UserDtOs
{
    public class Result
    {
        bool Success { get; set; }
        string Message { get; set; }
        
    }

    public class UserData
    {
        public string? UserName { get; set; }
        public string? userId { get; set; }
        public string? IPAddress { get; set; }
        public string? Status  { get; set; }
    }
}