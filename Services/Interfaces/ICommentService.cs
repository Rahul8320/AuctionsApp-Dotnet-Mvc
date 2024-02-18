using AuctionsApp.Data.Entity;

namespace AuctionsApp.Services.Interfaces;

public interface ICommentService
{
    Task AddComment(Comment comment);
}
