using LateCheckInApp.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace LateCheckInApp.Services
{
  public class EmailService
  {
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> options)
    {
      _settings = options.Value;
    }

    public async Task SendLateCheckInEmailAsync(GuestRegistration registration,string? pdfFilePath, string? photoIdPhysicalPath,
    string? signaturePhysicalPath)
    {
      using var message = new MailMessage();
      message.From = new MailAddress(_settings.FromEmail);
      message.To.Add(_settings.HotelRecipientEmail);
      message.Subject = $"New Late Check-In Submission - {registration.FullName}";
      message.Body =
  $@"A new late check-in form has been submitted.

Guest: {registration.FullName}
Phone: {registration.PhoneNumber}
Email: {registration.Email}
Car Rego: {registration.CarRego}
Check-In: {registration.CheckInDate}
Check-Out: {registration.CheckOutDate}
Bond Amount: {registration.PreAuthAmount:C}
Stripe Status: {registration.PreAuthStatus}
Final Payment Status: {registration.FinalPaymentStatus}
Submitted At: {registration.CreatedAt.ToLocalTime()}";

      if (!string.IsNullOrWhiteSpace(pdfFilePath) && System.IO.File.Exists(pdfFilePath))
      {
        message.Attachments.Add(new Attachment(pdfFilePath));
      }

      if (!string.IsNullOrWhiteSpace(photoIdPhysicalPath) && File.Exists(photoIdPhysicalPath))
      {
        message.Attachments.Add(new Attachment(photoIdPhysicalPath));
      }

      if (!string.IsNullOrWhiteSpace(signaturePhysicalPath) && File.Exists(signaturePhysicalPath))
      {
        message.Attachments.Add(new Attachment(signaturePhysicalPath));
      }

      using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
      {
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(_settings.Username, _settings.Password),
        EnableSsl = true
      };

      await client.SendMailAsync(message);
    }
  }
}
