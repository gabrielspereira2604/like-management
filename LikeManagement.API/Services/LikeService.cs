using LikeManagement.API.Models;
using LikeManagement.API.Repositories;

namespace LikeManagement.API.Services;

public class LikeService(IPostRepository repository) : ILikeService
{
    public Task<Post?> GetPostAsync(int postId) =>
        repository.GetByIdAsync(postId);

    public Task<IEnumerable<Post>> GetAllPostsAsync() =>
        repository.GetAllAsync();

    public async Task<Post> CreatePostAsync(string title)
    {
        var post = new Post { Title = title };
        await repository.AddAsync(post);
        return post;
    }

    public Task<bool> LikeAsync(int postId) =>
        repository.IncrementLikeAsync(postId);

    public Task<bool> UnlikeAsync(int postId) =>
        repository.DecrementLikeAsync(postId);
}
