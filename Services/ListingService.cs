using AuctionsApp.Data;
using AuctionsApp.Data.Entity;
using AuctionsApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionsApp.Services;

public class ListingService(ApplicationDbContext context) : IListingService
{
    private readonly ApplicationDbContext _context = context;

    public IQueryable<Listing> GetAll()
    {
        var allListings = _context.Listings.Include(l => l.User);

        return allListings;
    }
}
