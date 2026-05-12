using LikeManagement.API.Models;

namespace LikeManagement.API.Repositories;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(int postId);
    Task<IEnumerable<Post>> GetAllAsync();
    Task AddAsync(Post post);

    // Incremento atômico: deixa o banco fazer likes + 1, evitando race condition
    Task<bool> IncrementLikeAsync(int postId);
    Task<bool> DecrementLikeAsync(int postId);
}
