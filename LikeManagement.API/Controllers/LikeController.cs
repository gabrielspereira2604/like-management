using LikeManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LikeManagement.API.Controllers;

[ApiController]
[Route("api/posts")]
public class LikeController(ILikeService likeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await likeService.GetAllPostsAsync());

    [HttpGet("{postId:int}")]
    public async Task<IActionResult> GetById(int postId)
    {
        var post = await likeService.GetPostAsync(postId);
        return post is null ? NotFound() : Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
    {
        var post = await likeService.CreatePostAsync(request.Title);
        return CreatedAtAction(nameof(GetById), new { postId = post.Id }, post);
    }

    [HttpPost("{postId:int}/like")]
    public async Task<IActionResult> Like(int postId)
    {
        var success = await likeService.LikeAsync(postId);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{postId:int}/like")]
    public async Task<IActionResult> Unlike(int postId)
    {
        var success = await likeService.UnlikeAsync(postId);
        return success ? NoContent() : NotFound();
    }
}

public record CreatePostRequest(string Title);
