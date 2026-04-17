using System.ComponentModel.DataAnnotations;


 

namespace LateCheckInApp.Models
  {
    public class GuestRegistration
    {
      public int Id { get; set; }

      [Required]
      public string FullName { get; set; } = string.Empty;

      [Required]
      [Phone]
      public string PhoneNumber { get; set; } = string.Empty;

      [EmailAddress]
      public string? Email { get; set; }

      [Required]
      public DateTime CheckInDate { get; set; }

      [Required]
      public DateTime CheckOutDate { get; set; }

    public string? CarRego { get; set; }

    //photoid
    public string? PhotoIdPath { get; set; }
    public string? PhotoIdOriginalFileName { get; set; }
    public string? PhotoIdContentType { get; set; }
    public long? PhotoIdFileSize { get; set; }
    public DateTime? PhotoIdUploadedAt { get; set; }

    //Terms
    public bool TermsAccepted { get; set; }
    public DateTime? TermsAcceptedAtUtc { get; set; }

    public string TermsVersion { get; set; } = "v1";

    public DateTime TermsEffectiveFromUtc { get; set; }


    //  Deposit
    public bool DepositAuthorizationAccepted { get; set; }
    public DateTime? DepositAuthorizationAcceptedAtUtc { get; set; }

    //e-Signature
    public string? SignaturePath { get; set; }
    public DateTime? SignedAt { get; set; }

    //Pre-Auth-Stripe
    public string? PreAuthPaymentIntentId { get; set; }
    public string? PreAuthStatus { get; set; }
    public decimal? PreAuthAmount { get; set; }
    //public string? CardBrand { get; set; }
    //public string? CardLast4 { get; set; }
    public DateTime? PreAuthCreatedAt { get; set; }

    //For Admin Page - for reception to capture/release bond
    public string? FinalPaymentStatus { get; set; }   // Authorized / Captured / Released
    public DateTime? PreAuthCapturedAt { get; set; }
    public DateTime? PreAuthReleasedAt { get; set; }
    public string? AdminNote { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
  }

