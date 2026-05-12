using LikeManagement.API.Models;

namespace LikeManagement.API.Services;

public interface ILikeService
{
    Task<Post?> GetPostAsync(int postId);
    Task<IEnumerable<Post>> GetAllPostsAsync();
    Task<Post> CreatePostAsync(string title);
    Task<bool> LikeAsync(int postId);
    Task<bool> UnlikeAsync(int postId);
}
