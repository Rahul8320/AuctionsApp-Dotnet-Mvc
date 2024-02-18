using AuctionsApp.Data;
using AuctionsApp.Data.Entity;
using AuctionsApp.Models;
using AuctionsApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionsApp.Services;

public class ListingService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) : IListingService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    public async Task<bool> AddListing(ListingViewModel listing)
    {
        try
        {
            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(); // Milliseconds since Unix epoch
            string fileName = timestamp.ToString() + "_" + listing.Image.FileName;
            string filePath = Path.Combine(uploadDir, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            listing.Image.CopyTo(fileStream);

            var listingObj = new Listing
            {
                Title = listing.Title,
                Description = listing.Description,
                Price = listing.Price,
                IdentityUserId = listing.IdentityUserId,
                ImagePath = fileName,
            };

            _context.Listings.Add(listingObj);
            return await _context.SaveChangesAsync() != 0;
        }
        catch (Exception)
        {
            throw;
        }
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

    public async Task<Listing?> GetListing(int? id)
    {
        try
        {
            if(id == null) return null;

            var listing = await _context.Listings
           .Include(l => l.User)
           .Include(l => l.Comments)
           .Include(l => l.Bids)
           .ThenInclude(l => l.User)
           .FirstOrDefaultAsync(m => m.Id == id);

            return listing;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task SaveChanges()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
