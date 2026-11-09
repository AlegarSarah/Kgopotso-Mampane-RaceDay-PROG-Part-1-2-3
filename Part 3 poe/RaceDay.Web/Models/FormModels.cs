using System.ComponentModel.DataAnnotations;
namespace RaceDay.Web.Models;

public class LoginVm
{
    [Required(ErrorMessage = "Please enter your email."), EmailAddress] public string Email { get; set; } = "";
    [Required(ErrorMessage = "Please enter your password.")] public string Password { get; set; } = "";
}
public class RegisterVm
{
    [Required, Display(Name = "First name")] public string FirstName { get; set; } = "";
    [Required, Display(Name = "Last name")] public string LastName { get; set; } = "";
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required, MinLength(8, ErrorMessage = "Password must be at least 8 characters.")] public string Password { get; set; } = "";
    [Required, Compare(nameof(Password), ErrorMessage = "Passwords do not match."), Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = "";
    [Required(ErrorMessage = "Please choose a role.")] public string Role { get; set; } = "";
    [Display(Name = "Phone number")] public string? PhoneNumber { get; set; }
}
public class ProfileVm
{
    public string Email { get; set; } = "";
    [Required, Display(Name = "First name")] public string FirstName { get; set; } = "";
    [Required, Display(Name = "Last name")] public string LastName { get; set; } = "";
    [Display(Name = "Phone number")] public string? PhoneNumber { get; set; }
    public string? ProfilePictureUrl { get; set; }
}
public class EventFormVm
{
    public int EventId { get; set; }
    [Required, StringLength(150)] public string Name { get; set; } = "";
    [Required, StringLength(1000)] public string Description { get; set; } = "";
    [Required, Display(Name = "Date and time")] public DateTime Date { get; set; } = DateTime.Today.AddDays(30).AddHours(7);
    [Required, StringLength(200)] public string Location { get; set; } = "";
    [Range(0.1, 1000, ErrorMessage = "Distance must be between 0.1 and 1000 km."), Display(Name = "Distance (km)")]
    public decimal DistanceKm { get; set; } = 10;
    [Required, Display(Name = "Event type")] public string EventType { get; set; } = "Run";
    public string? BannerUrl { get; set; }
    [Display(Name = "Banner image")] public IFormFile? Banner { get; set; }
}