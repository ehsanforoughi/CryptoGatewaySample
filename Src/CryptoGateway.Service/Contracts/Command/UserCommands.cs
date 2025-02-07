namespace CryptoGateway.Service.Contracts.Command;

public static class UserCommands
{
    public static class V1
    {
        public class RegisterUser
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }

        public class EditUserProfile
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string NationalCode { get; set; }
            public DateTime Birthdate { get; set; }
        }

        public class EditPassword
        {
            public string UserId { get; set; }
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }

        public class SetMobileNumber
        {
            public string UserId { get; set; }
            public long MobileNumber { get; set; }
        }
    }
}