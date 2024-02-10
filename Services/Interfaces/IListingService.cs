using AuctionsApp.Data.Entity;

namespace AuctionsApp.Services.Interfaces;

public interface IListingService
{
    IQueryable<Listing> GetAll();
}
