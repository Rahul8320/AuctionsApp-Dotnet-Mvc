using AuctionsApp.Data.Entity;

namespace AuctionsApp.Services.Interfaces;

public interface IBidService
{
    Task AddBid(Bid bid);
}
