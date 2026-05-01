using LateCheckInApp.Data;
using LateCheckInApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LateCheckInApp.Pages.Admin;

[Authorize]
public class GenerateLinkModel : PageModel
{
  private readonly AppDbContext _dbContext;

  public GenerateLinkModel(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  [BindProperty]
  public int ExpiryHours { get; set; } = 48;

  public string? GeneratedLink { get; set; }

  public void OnGet()
  {
  }

  public async Task<IActionResult> OnPostAsync()
  {
    if (ExpiryHours <= 0)
    {
      ModelState.AddModelError(nameof(ExpiryHours), "Expiry hours must be greater than 0.");
      return Page();
    }

    var invite = new LateCheckInInvite
    {
      AccessToken = Guid.NewGuid().ToString("N"),
      ExpiresAtUtc = DateTime.UtcNow.AddHours(ExpiryHours),
      IsUsed = false,
      CreatedAtUtc = DateTime.UtcNow
    };

    _dbContext._lateCheckInInvites.Add(invite);
    await _dbContext.SaveChangesAsync();

    GeneratedLink = $"{Request.Scheme}://{Request.Host}/?token={invite.AccessToken}";

    return Page();
  }
}