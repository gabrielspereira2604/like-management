namespace LikeManagement.API.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public long LikeCount { get; set; }
}
