using System.ComponentModel;

namespace CryptoGateway.Service.Strategies.Notification.SmsGateway;

public enum KavenegarTemplate
{
    //[Description("Verify-Hilla")]
    //Otp_HillaPay = 1,

    [Description("Verify-Ramzplus")]
    Verify_Ramzplus = 1,

    [Description("KYC-Issue")]
    KYC_Issue = 2,

    [Description("KYC-One-Done")]
    KYC_One_Done = 3,

    [Description("KYC-Two-Done")]
    KYC_Two_Done = 4,

    [Description("Referral")]
    Referral = 5,

    [Description("Nobitex-Token-Expire")]
    Nobitex_Token_Expire = 6,

    [Description("Nobitex-Buy-Error")]
    Nobitex_Buy_Error = 7,

    [Description("User-Level")]
    User_Level = 8,

    [Description("Crypto-Deposit")]
    Crypto_Deposit = 9,

    [Description("Withdrawal-Fee-Exceeded")]
    Withdrawal_Fee_Exceeded = 10
}