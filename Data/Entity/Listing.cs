using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionsApp.Data.Entity;

public class Listing
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Price { get; set; }
    public string ImagePath { get; set; } = default!;
    public bool IsSold { get; set; } = false;

    [Required]
    public string? IdentityUserId { get; set; }
    [ForeignKey("IdentityUserId")]
    public IdentityUser? User { get; set; }

    public List<Bid>? Bids { get; set; }
    public List<Comment>? Comments { get; set; }
}
