using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuctionsApp.Data;
using AuctionsApp.Services.Interfaces;
using AuctionsApp.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using AuctionsApp.Models;

namespace AuctionsApp.Controllers;

public class ListingsController(IListingService listingService, IWebHostEnvironment webHostEnvironment) : Controller
{
    private readonly IListingService _listingService = listingService;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    // GET: Listings
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var allListings = _listingService.GetAll();
        return View(await allListings.ToListAsync());
    }

    // GET: Listings/Details/5
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var listing = await _listingService.GetListing(id);
        if (listing == null)
        {
            return NotFound();
        }

        return View(listing);
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
        if(!ModelState.IsValid)
        {
            return View(listing);
        }

        if (listing.Image != null)
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

            await _listingService.AddListing(listingObj);

            return RedirectToAction("Index");
        }

        return View(listing);
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

    //private bool ListingExists(int id)
    //{
    //    return _context.Listings.Any(e => e.Id == id);
    //}
}
