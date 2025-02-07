namespace CryptoGateway.Domain.Entities.User;

public static class UserEvents
{
    public static class V1
    {
        public class UserRegistered
        {
            public string UserExternalId { get; set; }
            public string Email { get; set; }
        }

        public class UserProfileUpdated
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string NationalCode { get; set; }
            public DateTime? Birthdate { get; set; }
        }

        //public class PasswordUpdated
        //{
        //    //public int UserId { get; set; }
        //    //public string UserExternalId { get; set; }
        //    public string Password { get; set; }
        //}

        public class MobileNumberChanged
        {
            public long? MobileNumber { get; set; }
        }

        public class UserCreditAllocated
        {
            public int UserCreditId { get; set; }
            public string CurrencyCode { get; set; }
            public decimal AvailableAmount { get; set; }
        }
    }
}