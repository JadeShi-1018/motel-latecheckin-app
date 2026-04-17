using LateCheckInApp.Data;
using LateCheckInApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace LateCheckInApp.Pages.Admin;

[Authorize]
public class RegistrationDetailsModel : PageModel
{
  private readonly AppDbContext _dbContext;

  public RegistrationDetailsModel(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  [BindProperty]
  public GuestRegistration Registration { get; set; } = default!;

  public async Task<IActionResult> OnGetAsync(int id)
  {
    var registration = await _dbContext._guestRegistrations.FirstOrDefaultAsync(x => x.Id == id);
    if (registration == null)
    {
      return NotFound();
    }

    Registration = registration;
    return Page();
  }

  public async Task<IActionResult> OnPostCaptureAsync(int id)
  {
    var registration = await _dbContext._guestRegistrations.FirstOrDefaultAsync(x => x.Id == id);
    if (registration == null)
    {
      return NotFound();
    }

    if (registration.FinalPaymentStatus != "Authorized" ||
        string.IsNullOrWhiteSpace(registration.PreAuthPaymentIntentId))
    {
      TempData["ErrorMessage"] = "This bond cannot be captured.";
      return RedirectToPage(new { id });
    }

    var service = new PaymentIntentService();
    var intent = await service.CaptureAsync(registration.PreAuthPaymentIntentId);

    registration.PreAuthStatus = intent.Status;
    registration.FinalPaymentStatus = "Captured";
    registration.PreAuthCapturedAt = DateTime.UtcNow;

    await _dbContext.SaveChangesAsync();

    TempData["SuccessMessage"] = "Bond captured successfully.";
    return RedirectToPage(new { id });
  }

  public async Task<IActionResult> OnPostReleaseAsync(int id)
  {
    var registration = await _dbContext._guestRegistrations.FirstOrDefaultAsync(x => x.Id == id);
    if (registration == null)
    {
      return NotFound();
    }

    if (registration.FinalPaymentStatus != "Authorized" ||
        string.IsNullOrWhiteSpace(registration.PreAuthPaymentIntentId))
    {
      TempData["ErrorMessage"] = "This bond cannot be released.";
      return RedirectToPage(new { id });
    }

    var service = new PaymentIntentService();
    var intent = await service.CancelAsync(registration.PreAuthPaymentIntentId);

    registration.PreAuthStatus = intent.Status;
    registration.FinalPaymentStatus = "Released";
    registration.PreAuthReleasedAt = DateTime.UtcNow;

    await _dbContext.SaveChangesAsync();

    TempData["SuccessMessage"] = "Bond released successfully.";
    return RedirectToPage(new { id });
  }
}