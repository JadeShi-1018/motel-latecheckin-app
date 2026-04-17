using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LateCheckInApp.Pages.Admin;

public class LoginModel : PageModel
{
  private readonly SignInManager<IdentityUser> _signInManager;

  public LoginModel(SignInManager<IdentityUser> signInManager)
  {
    _signInManager = signInManager;
  }

  [BindProperty]
  public InputModel Input { get; set; } = new();

  public string? ReturnUrl { get; set; }

  public class InputModel
  {
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
  }

  public void OnGet(string? returnUrl = null)
  {
    ReturnUrl = returnUrl;
  }

  public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
  {
    ReturnUrl = returnUrl ?? "/Admin/Registrations";

    if (!ModelState.IsValid)
    {
      return Page();
    }

    var result = await _signInManager.PasswordSignInAsync(
        Input.Email,
        Input.Password,
        Input.RememberMe,
        lockoutOnFailure: false);

    if (result.Succeeded)
    {
      return LocalRedirect(ReturnUrl);
    }

    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    return Page();
  }
}