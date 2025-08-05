using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using GameRankAdminPanel.Data;
using GameRankAdminPanel.Interfaces;
using GameRankAdminPanel.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using GameRankAdminPanel.Services.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<AdminPanelDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AdminDBConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GameRank Admin API", Version = "v1" });
});

builder.Services.AddScoped<RabbitMQService>();
builder.Services.AddScoped<IUserMgmtService, UserMgmtService>();
// CORS Settings --------------------------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost" , "http://192.168.0.103").AllowAnyHeader().AllowAnyMethod().AllowCredentials(); 
    });
});
//  --------------------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GameRank Admin API V1");
        c.RoutePrefix = "swagger"; 
    });
}

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();