using AuctionsApp.Data;
using AuctionsApp.Data.Entity;
using AuctionsApp.Services.Interfaces;

namespace AuctionsApp.Services;

public class CommentService(ApplicationDbContext context) : ICommentService
{
    private readonly ApplicationDbContext _context = context;
    public async Task AddComment(Comment comment)
    {
        try
		{
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }
		catch (Exception)
		{
			throw;
		}
    }
}
