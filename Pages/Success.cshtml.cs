using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LateCheckInApp.Pages
{
  public class SuccessModel : PageModel
  {
    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public void OnGet()
    {
    }
  }
}