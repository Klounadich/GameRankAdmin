namespace GameRankAdminPanel.Models;

public class UserDtOs
{
    public class Result
    {
       public bool Success { get; set; }
       public IQueryable Message { get; set; }
        
    }

    public class ActionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class UserData
    {
        public string? UserName { get; set; }
        public string? Id { get; set; }
        public string? IPAdress { get; set; }
        public string? Status  { get; set; }
        public string? Email { get; set; }
        
        public IList<string>? Role { get; set; }
    }


    public class ChangeRoleRequest
    {
        public string UserName { get; set; }
        public string newRole { get; set; }
        
    }
}