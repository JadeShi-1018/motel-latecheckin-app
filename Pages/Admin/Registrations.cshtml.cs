using LateCheckInApp.Data;
using LateCheckInApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LateCheckInApp.Pages.Admin;
[Authorize]
public class RegistrationsModel : PageModel
{
  private readonly AppDbContext _dbContext;

  public RegistrationsModel(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public List<GuestRegistration> Registrations { get; set; } = new();
  public int CurrentPage { get; set; }
  public int PageSize { get; set; } = 10;
  public int TotalPages { get; set; }

  public async Task OnGetAsync(int pageNumber = 1)
  {
    CurrentPage = pageNumber < 1 ? 1 : pageNumber;

    var latest50 = await _dbContext._guestRegistrations
        .OrderByDescending(r => r.CreatedAt)
        .Take(50)
        .ToListAsync();

    TotalPages = (int)Math.Ceiling(latest50.Count / (double)PageSize);
    if (TotalPages > 0 && CurrentPage > TotalPages)
    {
      CurrentPage = TotalPages;
    }

    Registrations = latest50
        .Skip((CurrentPage - 1) * PageSize)
        .Take(PageSize)
        .ToList();
  }
}