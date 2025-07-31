using Microsoft.EntityFrameworkCore;
using GameRankAdminPanel.Models;
namespace GameRankAdminPanel.Data;

public class AdminPanelDBContext:  DbContext
{
    public AdminPanelDBContext(DbContextOptions<AdminPanelDBContext> options) : base(options) { }
    public DbSet<UsersStatus> UserDataAdmin { get; set; }
    
    public DbSet<SuspectUsers> SuspectUsers{ get; set; }
}