using LateCheckInApp.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LateCheckInApp.Services
{
  public class PDFService
  {
    private readonly IWebHostEnvironment _environment;

    public PDFService(IWebHostEnvironment environment)
    {
      _environment = environment;
    }

    public Task<string> GenerateLateCheckInSummaryPdfAsync(GuestRegistration registration)
    {
      var pdfFolder = Path.Combine(_environment.WebRootPath, "generated-pdfs");
      Directory.CreateDirectory(pdfFolder);

      var fileName = $"LateCheckIn-{registration.Id}.pdf";
      var filePath = Path.Combine(pdfFolder, fileName);

      Document.Create(container =>
      {
        container.Page(page =>
        {
          page.Size(PageSizes.A4);
          page.Margin(2, Unit.Centimetre);
          page.PageColor(Colors.White);

          page.Header()
              .Text("Late Check-In Summary")
              .SemiBold()
              .FontSize(20);

          page.Content()
              .PaddingVertical(20)
              .Column(column =>
              {
                column.Spacing(8);

                column.Item().Text($"Guest Name: {registration.FullName}");
                column.Item().Text($"Phone: {registration.PhoneNumber}");
                column.Item().Text($"Email: {registration.Email}");
                column.Item().Text($"Car Rego: {registration.CarRego}");
                column.Item().Text($"Check-In: {registration.CheckInDate:dd MMM yyyy HH:mm}");
                column.Item().Text($"Check-Out: {registration.CheckOutDate:dd MMM yyyy HH:mm}");
                column.Item().Text($"Terms Accepted: {(registration.TermsAccepted ? "Yes" : "No")}");
                column.Item().Text($"Deposit Authorization Accepted: {(registration.DepositAuthorizationAccepted ? "Yes" : "No")}");
                column.Item().Text($"Bond Amount: {registration.PreAuthAmount:C}");
                column.Item().Text($"Stripe Status: {registration.PreAuthStatus}");
                column.Item().Text($"Final Payment Status: {registration.FinalPaymentStatus}");
                column.Item().Text($"Submitted At: {registration.CreatedAt.ToLocalTime():dd MMM yyyy HH:mm}");
              });

          page.Footer()
              .AlignCenter()
              .Text(x =>
              {
                x.Span("Page ");
                x.CurrentPageNumber();
              });
        });
      }).GeneratePdf(filePath);

      return Task.FromResult(filePath);
    }
  }
}
