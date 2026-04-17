using LateCheckInApp.Data;
using LateCheckInApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LateCheckInApp.Pages;

public class IndexModel : PageModel
{
  private readonly AppDbContext _dbContext;
  private readonly IWebHostEnvironment _environment;
  private const string ServerTermsVersion = "v1";
  private static readonly DateTime ServerTermsEffectiveFromUtc =
      new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc);
  private readonly IConfiguration _configuration;

  public IndexModel(AppDbContext dbContext, IWebHostEnvironment environment, IConfiguration configuration)
  {
    _dbContext = dbContext;
    _environment = environment;
    _configuration = configuration;
  }

  [BindProperty]
  public GuestRegistration Registration { get; set; } = new();
  [BindProperty]
  public string CurrentTermsVersion { get; set; } = ServerTermsVersion;

  [BindProperty]
  public DateTime CurrentTermsEffectiveFromUtc { get; set; } =
            ServerTermsEffectiveFromUtc;

  [BindProperty]
  public string? SignatureData { get; set; }
  [BindProperty]
  public string? PaymentIntentId { get; set; }
  public string StripePublishableKey { get; private set; } = string.Empty;



  [BindProperty]
  public bool PreAuthSucceeded { get; set; }

  public string CurrentTermsContent { get; set; } = @"
Terms and Conditions

Any violation of these Terms and Conditions may result in forfeiture of the security deposit and/or immediate eviction from the property without refund.

1. All rooms are strictly non-smoking. Ashtrays are provided outside for your convenience.
2. Prostitution and any illegal activities are strictly prohibited on the premises. We reserve the right to terminate your stay immediately without refund if such activities are detected.
3. Visitors, parties, and loud music are not permitted.
4. Guests are responsible for their personal belongings and valuables at all times.
5. The motel is not liable for any loss or damage to vehicles parked on the premises.
6. Any damage to the property will be charged to the guest and may be deducted from the security deposit or charged to the provided payment method.
7. Check-out time is 10:00 AM. Late check-out may incur additional charges equivalent to a full day's rate.
8. All payments, including the security deposit, must be paid in full upon check-in.
9. Guests must inspect the room upon arrival and report any existing damage to reception immediately. Failure to do so may result in the guest being held responsible.
10. Lost property will be kept for up to one week. After this period, items may be disposed of without further notice.
11. Early departures or same-day cancellations are non-refundable once the room has been occupied.
12. A fee of $50 will be charged for lost room keys.
13. Additional cleaning fees ranging from $50 to $150 may apply if the room is left in an excessively dirty condition.
14. Reception hours are from 9:00 AM to 10:00 PM. Services requested outside these hours may incur a $50 fee.
15. Guests requiring early check-out before 9:00 AM must notify reception at check-in.
16. Long-term accommodation payments are non-refundable once agreed and paid.
";


  [BindProperty]
  public IFormFile? PhotoIdFile { get; set; }

  public void OnGet()
  {
    Registration = new GuestRegistration
    {
      CheckInDate = DateTime.Now,
      CheckOutDate = DateTime.Now
    };
    StripePublishableKey = _configuration["Stripe:PublishableKey"] ?? string.Empty;

  }

  public async Task<IActionResult> OnPostAsync()
  {

    if (!Registration.TermsAccepted) {
      ModelState.AddModelError("Registration.TermsAccepted",
        "You must accept the terms and conditions."); 
  }

if (!Registration.DepositAuthorizationAccepted)
{
    ModelState.AddModelError("Registration.DepositAuthorizationAccepted",
        "You must authorize the security deposit.");
}

    if (!PreAuthSucceeded || string.IsNullOrWhiteSpace(PaymentIntentId))
    {
      ModelState.AddModelError(string.Empty, "Card pre-authorization is required.");
      return Page();
    }

    if (string.IsNullOrWhiteSpace(SignatureData))
    {
      ModelState.AddModelError("SignatureData", "Signature is required.");
    }

    if (CurrentTermsVersion != ServerTermsVersion ||
    CurrentTermsEffectiveFromUtc != ServerTermsEffectiveFromUtc)
    {
      ModelState.AddModelError(string.Empty,
          "The terms and conditions have been updated. Please review and accept the latest version.");
    }

    if (PhotoIdFile == null || PhotoIdFile.Length == 0)
    {
      ModelState.AddModelError("PhotoIdFile", "Photo ID is required.");
    }

    var now = DateTime.Now;
    if(Registration.CheckInDate < now.Date)
    {
      ModelState.AddModelError("Registration.CheckInDate", "Check-in must be today or later.");
    }

    if (Registration.CheckOutDate <= Registration.CheckInDate)
    {
      ModelState.AddModelError("Registration.CheckOutDate", "Check-out must be later than check-in.");
    }

    if (!ModelState.IsValid)
    {
      CurrentTermsVersion = ServerTermsVersion;
      CurrentTermsEffectiveFromUtc = ServerTermsEffectiveFromUtc;
      return Page();
    }
    Registration.TermsAcceptedAtUtc = DateTime.UtcNow;
    Registration.TermsVersion = ServerTermsVersion;
    Registration.TermsEffectiveFromUtc = ServerTermsEffectiveFromUtc;

    Registration.DepositAuthorizationAcceptedAtUtc = DateTime.UtcNow;


    //save photoid path, sav photo id to uploads
    if (PhotoIdFile != null)
    {
      var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
      var extension = Path.GetExtension(PhotoIdFile.FileName).ToLowerInvariant();
      var allowedContentTypes = new[] { "image/jpeg", "image/png" };


      if (!allowedExtensions.Contains(extension) || !allowedContentTypes.Contains(PhotoIdFile.ContentType))
      {
        ModelState.AddModelError("PhotoIdFile", "Only JPG, and PNG files are allowed.");
        return Page();
      }

      if (PhotoIdFile.Length > 5 * 1024 * 1024)
      {
        ModelState.AddModelError("PhotoIdFile", "File size must be less than 5MB.");
        return Page();
      }

      var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "photoids");
      Directory.CreateDirectory(uploadsFolder);

      var uniqueFileName = $"{Guid.NewGuid()}{extension}";
      var filePath = Path.Combine(uploadsFolder, uniqueFileName);

      await using var stream = new FileStream(filePath, FileMode.Create);
      await PhotoIdFile.CopyToAsync(stream);

      Registration.PhotoIdPath = $"/uploads/photoids/{uniqueFileName}";
      Registration.PhotoIdOriginalFileName = PhotoIdFile.FileName;
      Registration.PhotoIdContentType = PhotoIdFile.ContentType;
      Registration.PhotoIdFileSize = PhotoIdFile.Length;
      Registration.PhotoIdUploadedAt = DateTime.UtcNow;
    }

    // save signature
    if (!string.IsNullOrWhiteSpace(SignatureData))
    {
      try
      {
        var base64Data = SignatureData;

        if (base64Data.Contains(","))
        {
          base64Data = base64Data.Split(',')[1];
        }

        byte[] imageBytes = Convert.FromBase64String(base64Data);

        var signaturesFolder = Path.Combine(_environment.WebRootPath, "uploads", "signatures");
        Directory.CreateDirectory(signaturesFolder);

        var signatureFileName = $"{Guid.NewGuid()}.png";
        var signatureFilePath = Path.Combine(signaturesFolder, signatureFileName);

        await System.IO.File.WriteAllBytesAsync(signatureFilePath, imageBytes);

        Registration.SignaturePath = $"/uploads/signatures/{signatureFileName}";
        Registration.SignedAt = DateTime.UtcNow;
      }
      catch 
      {
        ModelState.AddModelError("SignatureData", "Invalid signature data.");
        return Page();
      }
    }

    //pre-auth-stripe
    var paymentIntentService = new Stripe.PaymentIntentService();
    var intent = await paymentIntentService.GetAsync(PaymentIntentId);

    if (intent == null || intent.Status != "requires_capture")
    {
      ModelState.AddModelError(string.Empty, "Card pre-authorization is invalid.");
      return Page();
    }
    Registration.PreAuthPaymentIntentId = PaymentIntentId;
    Registration.PreAuthStatus = intent.Status;
    Registration.PreAuthAmount = intent.Amount / 100m;
    Registration.PreAuthCreatedAt = DateTime.UtcNow;
    Registration.FinalPaymentStatus = "Authorized";

    Registration.CreatedAt = DateTime.UtcNow;

    _dbContext._guestRegistrations.Add(Registration);
    await _dbContext.SaveChangesAsync();

    return RedirectToPage("/Success", new { id = Registration.Id });
  }
}
