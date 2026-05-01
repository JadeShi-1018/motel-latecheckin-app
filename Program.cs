using LateCheckInApp.Data;
using LateCheckInApp.Models;
using LateCheckInApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Stripe;
using QuestPDF.Infrastructure;



SQLitePCL.Batteries.Init();
var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

var stripeSection = builder.Configuration["Stripe:SecretKey"];

if (string.IsNullOrWhiteSpace(stripeSection))
{
  throw new Exception("Stripe:SecretKey is missing from configuration.");
}
StripeConfiguration.ApiKey = stripeSection;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=latecheckin.db"));
builder.Services.AddScoped<DbSeeder>();
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<PDFService>();

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
      options.Password.RequireDigit = true;
      options.Password.RequireLowercase = true;
      options.Password.RequireUppercase = true;
      options.Password.RequireNonAlphanumeric = false;
      options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
  options.LoginPath = "/Admin/Login";
  options.AccessDeniedPath = "/Admin/Login";
  options.ExpireTimeSpan = TimeSpan.FromHours(1);
  options.SlidingExpiration = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
  var seeder = services.GetRequiredService<DbSeeder>();

  await seeder.SeedAdminUserAsync(userManager);
}

app.Run();
