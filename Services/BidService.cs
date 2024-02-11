using AuctionsApp.Data;
using AuctionsApp.Data.Entity;
using AuctionsApp.Services.Interfaces;

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
}
