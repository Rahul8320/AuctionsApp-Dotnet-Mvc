using AuctionsApp.Models;

namespace AuctionsApp.Services.Interfaces;

public interface IListingService
{
    IQueryable<Listing> GetAll();
}
