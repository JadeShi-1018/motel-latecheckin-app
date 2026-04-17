using Microsoft.AspNetCore.Identity;

namespace LateCheckInApp.Data
{
  public static class DbSeeder
  {

    public static async Task SeedAdminUserAsync(IServiceProvider services)
    {
      var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

      var adminEmail = "info@essendonmotorinn.com.au";
      var adminPassword = "Bulla93#";

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
