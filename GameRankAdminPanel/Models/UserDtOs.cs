namespace GameRankAdminPanel.Models;

public class UserDtOs
{
    public class Result
    {
       public bool Success { get; set; }
       public IQueryable Message { get; set; }
        
    }
    

    public class UserData
    {
        public string? UserName { get; set; }
        public string? Id { get; set; }
        public string? IPAdress { get; set; }
        public string? Status  { get; set; }
        
        public IList<string>? Role { get; set; }
    }
}