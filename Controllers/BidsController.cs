using AuctionsApp.Data.Entity;
using AuctionsApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionsApp.Controllers;

public class BidsController(IBidService bidService, IListingService listingService) : Controller
{
    private readonly IBidService _bidService = bidService;
    private readonly IListingService _listingService = listingService;

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBid([Bind("Id, Price, ListingId, IdentityUserId")] Bid bid)
    {
        if(ModelState.IsValid)
        {
            await _bidService.AddBid(bid);
            var listing = await _listingService.GetListing(bid.ListingId);
            listing!.Price = (int)bid.Price;
            await _listingService.SaveChanges();

            return Redirect("/");
        }

        return BadRequest();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> CloseBidding(int id)
    {
        var listing = await _listingService.GetListing(id);
        listing!.IsSold = true;
        await _listingService.SaveChanges();

        return Redirect("/");
    }
}
