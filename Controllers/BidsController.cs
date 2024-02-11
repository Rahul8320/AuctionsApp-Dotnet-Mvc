using AuctionsApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionsApp.Controllers;

public class BidsController(IBidService bidService) : Controller
{
    private readonly IBidService _bidService = bidService;

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult AddBid()
    {
        return View();
    }
}
