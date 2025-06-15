using System.ComponentModel.DataAnnotations;

namespace BasicBlogWithAdminPanel.Models   // ← keep identical to the @model line
{
    /// <summary>
    /// Simple DTO used by /Account/Register.
    /// </summary>
    public class RegisterViewModel
    {
        [Required, EmailAddress, Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}
