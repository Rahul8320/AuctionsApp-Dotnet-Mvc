using AuctionsApp.Data.Entity;
using AuctionsApp.Models;

namespace AuctionsApp.Services.Interfaces;

public interface IListingService
{
    IQueryable<Listing> GetAll();
    Task<Listing?> GetListing(int? id);
    Task<bool> AddListing(ListingViewModel item);
    Task<bool?> DeleteListing(int id);
    Task SaveChanges();
}
