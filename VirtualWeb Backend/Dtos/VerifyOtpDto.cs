using System.ComponentModel.DataAnnotations;

public class VerifyOtpDto 
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "OTP verification code is required")]
    [StringLength(10, MinimumLength = 4, ErrorMessage = "Invalid OTP length")]
    public string Otp { get; set; } = string.Empty;
}