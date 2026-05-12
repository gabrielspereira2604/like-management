using LikeManagement.API.Data;
using LikeManagement.API.Repositories;
using LikeManagement.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=likes.db"));

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ILikeService, LikeService>();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.MapControllers();
app.Run();
