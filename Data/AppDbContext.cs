using LateCheckInApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LateCheckInApp.Data
{
  public class AppDbContext : IdentityDbContext<IdentityUser>
  {
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<GuestRegistration> _guestRegistrations => Set<GuestRegistration>();

    
  }
}
