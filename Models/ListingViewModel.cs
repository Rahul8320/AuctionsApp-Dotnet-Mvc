using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionsApp.Models;

public class ListingViewModel
{
    public int Id { get; set; }
    [Required]
    [Length(5, 100)]
    public string Title { get; set; } = default!;
    [Required]
    [Length(10,500)]
    public string Description { get; set; } = default!;
    [Required]
    [Range(0, int.MaxValue)]
    public int Price { get; set; }
    public IFormFile Image { get; set; } = default!;
    public bool IsSold { get; set; } = false;

    [Required]
    public string? IdentityUserId { get; set; }
    [ForeignKey("IdentityUserId")]
    public IdentityUser? User { get; set; }
}
