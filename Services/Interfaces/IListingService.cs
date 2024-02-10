using AuctionsApp.Data.Entity;

namespace AuctionsApp.Services.Interfaces;

public interface IListingService
{
    IQueryable<Listing> GetAll();
    Task<Listing?> GetListing(int id);
    Task<bool> AddListing(Listing item);
}
