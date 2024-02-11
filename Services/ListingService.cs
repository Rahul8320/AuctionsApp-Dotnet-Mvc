using AuctionsApp.Data;
using AuctionsApp.Data.Entity;
using AuctionsApp.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace AuctionsApp.Services;

public class ListingService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) : IListingService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    public async Task<bool> AddListing(Listing item)
    {
        _context.Listings.Add(item);
        return await _context.SaveChangesAsync() != 0;
    }

    public async Task<bool?> DeleteListing(int id)
    {
        try
        {
            var listing = await GetListing(id);

            if (listing != null)
            {
                _context.Listings.Remove(listing);
                var result = await _context.SaveChangesAsync() != 0;

                if (result && listing.ImagePath != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    string filePath = Path.Combine(uploadDir, listing.ImagePath);
                    bool fileExists = File.Exists(filePath);

                    if (fileExists)
                    {
                        try
                        {
                            File.Delete(filePath);
                            Console.WriteLine("Image deleted successfully!");
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine($"Error deleting image: {ex.Message}");
                        }
                    }
                }

                return result;
            }

            return null;
        }
        catch (Exception)
        {
            throw;
        }
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
