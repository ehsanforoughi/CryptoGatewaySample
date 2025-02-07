using System.ComponentModel;

namespace CryptoGateway.Domain.Entities.Notification.ValueObjects;

public enum NotificationActionType : byte
{

    [Description("ثبت نام")]
    Register = 1,

    [Description("ورود به سیستم")]
    Login = 2,

    [Description("خروج")]
    LogOut = 3,

    [Description("بازیابی کلمه عبور")]
    RecoverPassword = 4,

    [Description("تایید ایمیل")]
    VerifyEmail = 5,

    [Description("تایید احراز هویت")]
    VerifiedAuthentication = 6,

    [Description("تایید مرحله اول احراز هویت")]
    VerifiedAuthLevelOne = 41,

    [Description("تایید مرحله دوم احراز هویت")]
    VerifiedAuthLevelTwo = 42,

    [Description("عدم تایید احراز هویت")]
    UnVerifiedAuthentication = 7,

    [Description("تغییر پسوورد")]
    UpdatePassword = 8,


    [Description("واریز")]
    Deposit = 10,

    [Description("برداشت")]
    Withdraw = 11,

    [Description("خرید")]
    Buy = 12,

    [Description("فروش")]
    Sell = 13,


    [Description("ریفرال")]
    Referral = 15,

    [Description("تغییر سطح کاربری")]
    UpdateUserLevel = 16,


    [Description("رویدادهای پشتیبانی")]
    BackOfficeEvents = 20,

    [Description("انقضاء توکن نوبیتکس")]
    NobitexTokenExpire = 21,

    [Description("خطا در خرید نوبیتکس")]
    NobitexBuyError = 22,

    [Description("تغییر موجودی کاربر")]
    ChangeBalance = 30,

    [Description("پیامک آزاد و بدون فرمت")]
    FreeTemplate = 100
}