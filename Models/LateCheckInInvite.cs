namespace LateCheckInApp.Models
{
  public class LateCheckInInvite
  {
    public int Id { get; set; }
    public string AccessToken { get; set; } = default!;
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsUsed { get; set; }

    public string? GuestName { get; set; }   
    public DateTime CreatedAtUtc { get; set; }
  }
}
