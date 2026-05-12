using LikeManagement.API.Data;
using LikeManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LikeManagement.API.Repositories;

public class PostRepository(AppDbContext db) : IPostRepository
{
    public Task<Post?> GetByIdAsync(int postId) =>
        db.Posts.FindAsync(postId).AsTask();

    public async Task<IEnumerable<Post>> GetAllAsync() =>
        await db.Posts.ToListAsync();

    public async Task AddAsync(Post post)
    {
        db.Posts.Add(post);
        await db.SaveChangesAsync();
    }

    public async Task<bool> IncrementLikeAsync(int postId)
    {
        // UPDATE Posts SET LikeCount = LikeCount + 1 WHERE Id = @postId
        // O banco faz o incremento atomicamente — sem race condition
        var affected = await db.Posts
            .Where(p => p.Id == postId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.LikeCount, p => p.LikeCount + 1));

        return affected > 0;
    }

    public async Task<bool> DecrementLikeAsync(int postId)
    {
        var affected = await db.Posts
            .Where(p => p.Id == postId && p.LikeCount > 0)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.LikeCount, p => p.LikeCount - 1));

        return affected > 0;
    }
}
