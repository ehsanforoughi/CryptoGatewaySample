using System.ComponentModel.DataAnnotations;

namespace CryptoGateway.Service.Contracts.Command;

public static class AuthCommands
{
    public static class V1
    {
        public class Registration
        {
            public string Email { get; set; }
            public string Password { get; set; }
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public string? ClientURI { get; set; }
        }
        public class Login
        {
            [Required(ErrorMessage = "Email is required.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required.")]
            public string Password { get; set; }

            public string? ClientURI { get; set; }
        }

        public class ForgotPassword
        {
            [Required]
            [EmailAddress]
            public string? Email { get; set; }

            [Required]
            public string? ClientURI { get; set; }
        }

        public class EmailConfirmation
        {
            [Required]
            [EmailAddress]
            public string? Email { get; set; }
            public string? Token { get; set; }
        }
        public class ResetPassword
        {
            [Required(ErrorMessage = "Password is required")]
            public string? Password { get; set; }

            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string? ConfirmPassword { get; set; }

            public string? Email { get; set; }
            public string? Token { get; set; }
        }

        public class VerifyTwoStep
        {
            [Required]
            public string? Email { get; set; }
            [Required]
            public string? Provider { get; set; }
            [Required]
            public string? Token { get; set; }
        }

        public class ExternalLogin
        {
            public string? Provider { get; set; }
            public string? IdToken { get; set; }
        }
    }
}