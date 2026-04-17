using Microsoft.AspNetCore.Identity;

namespace LateCheckInApp.Data
{
  public class DbSeeder
  {

    private readonly IConfiguration _configuration;
    public DbSeeder(IConfiguration configuration)
    {
      _configuration = configuration;
    }
    public async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager)
    {
      

      var adminEmail = _configuration["AdminUser:Email"];
      var adminPassword = _configuration["AdminUser:Password"];

      var existingUser = await userManager.FindByEmailAsync(adminEmail);
      if (existingUser != null)
      {
        return;
      }

      var user = new IdentityUser
      {
        UserName = adminEmail,
        Email = adminEmail,
        EmailConfirmed = true
      };

      var result = await userManager.CreateAsync(user, adminPassword);

      if (!result.Succeeded)
      {
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        throw new Exception($"Failed to seed admin user: {errors}");
      }
    }
  }
}
