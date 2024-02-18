using AuctionsApp.Data;
using AuctionsApp.Data.Entity;
using AuctionsApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionsApp.Services;

public class BidService(ApplicationDbContext context) : IBidService
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddBid(Bid bid)
    {
		try
		{
			_context.Bids.Add(bid);
			await _context.SaveChangesAsync();
		}
		catch (Exception)
		{
			throw;
		}
    }

    public IQueryable<Bid> GetAllBids()
    {
		var query = from a in _context.Bids.Include(l => l.Listing).ThenInclude(u => u!.User) select a;

		return query;
    }
}
