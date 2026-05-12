using LikeManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LikeManagement.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Post> Posts => Set<Post>();
}
