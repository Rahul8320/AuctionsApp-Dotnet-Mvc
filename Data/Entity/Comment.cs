using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuctionsApp.Data.Entity;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; } = default!;

    [Required]
    public string? IdentityUserId { get; set; }
    [ForeignKey("IdentityUserId")]
    public IdentityUser? User { get; set; }

    public int? ListingId { get; set; }
    [ForeignKey("ListingId")]
    public Listing? Listing { get; set; }
}
