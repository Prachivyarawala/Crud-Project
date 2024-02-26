
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
public class User
{
    public int? c_userid { get; set; }

    // [Required(ErrorMessage = "Username is required.")]
    // [StringLength(255, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 255 characters.")]
    public string? c_username { get; set; }

    // [Required(ErrorMessage = "Email address is required.")]
    // [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? c_useremail { get; set; }

    // [Required(ErrorMessage = "Password is required.")]
    // [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    public string? c_userpassword { get; set; }

    // [Compare("c_userpassword", ErrorMessage = "Passwords do not match.")]
    public string? confirmPassword { get; set; }
}
}
