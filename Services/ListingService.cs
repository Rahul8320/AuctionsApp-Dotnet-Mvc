using AuctionsApp.Data;
using AuctionsApp.Data.Entity;
using AuctionsApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionsApp.Services;

public class ListingService(ApplicationDbContext context) : IListingService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<bool> AddListing(Listing item)
    {
        _context.Listings.Add(item);
        return await _context.SaveChangesAsync() != 0;
    }

    public IQueryable<Listing> GetAll()
    {
        var allListings = _context.Listings.Include(l => l.User);

        return allListings;
    }

    public async Task<Listing?> GetListing(int id)
    {
        var listing = await _context.Listings
            .Include(l => l.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        return listing;
    }
}
