namespace CryptoGateway.Infrastructure;

public static class GlobalVariables
{
    public static class EmailGatewaySettings
    {
        public static class Pakat
        {
            //public static string Url = "https://api.pakat.net/v3/smtp/email";
            public static string Url = "https://api.brevo.com/v3/smtp/email";
            public static string ApiKey = "123";
        }
    }

    public static class SmsGatewaySettings
    {
        public static class KaveNegar
        {
            public static string ApiKey = "123";
            public static string Sender = "123";
        }

        public static class NiazPardaz
        {
            public static string Username = "123";
            public static string Password = "123";
            public static string Sender = "123";
        }

        public static class LinePayamak
        {
            public static string Username = "123";
            public static string Password = "123";
            public static string Sender = "123";
        }

        public static class Asanak
        {
            public static string Username = "123";
            public static string Password = "123";
            public static string Sender = "123";
            public static string Source = "123";
        }
    }
}