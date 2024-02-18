using AuctionsApp.Data.Entity;
using AuctionsApp.Models;
using AuctionsApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> MyBids(int? pageNumber)
    {
        var allBids = _bidService.GetAllBids();
        int pageSize = 3;

        return View(await PaginatedList<Bid>.CreateAsync(allBids.Where(l => l.IdentityUserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).AsNoTracking(), pageNumber ?? 1, pageSize));
    }
}
