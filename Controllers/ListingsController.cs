using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionsApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using AuctionsApp.Models;
using Microsoft.IdentityModel.Tokens;
using AuctionsApp.Data.Entity;

namespace AuctionsApp.Controllers;

public class ListingsController(IListingService listingService, IWebHostEnvironment webHostEnvironment) : Controller
{
    private readonly IListingService _listingService = listingService;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    // GET: Listings
    [HttpGet]
    public async Task<IActionResult> Index(int? pageNumber, string searchString)
    {
        try
        {
            var allListings = _listingService.GetAll();
            int pageSize = 1;

            if (!searchString.IsNullOrEmpty())
            {
                allListings = allListings.Where(u => u.Title.ToLower().Contains(searchString.ToLower()));
            }

            return View(await PaginatedList<Listing>.CreateAsync(allListings.Where(u => u.IsSold == false).AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET: Listings/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var listing = await _listingService.GetListing(id);
            if (listing == null)
            {
                return NotFound();
            }

            return View(listing);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET: Listings/Create
    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Listings/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ListingViewModel listing)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(listing);
            }

            if (listing.Image != null)
            {
                await _listingService.AddListing(listing);

                return RedirectToAction("Index");
            }

            return View(listing);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET: Listings/Edit/5
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var listing = await _listingService.GetListing(id);
        if (listing == null)
        {
            return NotFound();
        }
        return View(listing);
    }

    // POST: Listings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,ImagePath,IsSold,IdentityUserId")] Listing listing)
    //{
    //    if (id != listing.Id)
    //    {
    //        return NotFound();
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            _context.Update(listing);
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!ListingExists(listing.Id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }
    //        return RedirectToAction(nameof(Index));
    //    }
    //    ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", listing.IdentityUserId);
    //    return View(listing);
    //}

    // GET: Listings/Delete/5
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var listing = await _listingService.GetListing(id);
        if (listing == null)
        {
            return NotFound();
        }

        return View(listing);
    }

    // POST: Listings/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var listing = await _listingService.DeleteListing(id);
            if (listing == null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
