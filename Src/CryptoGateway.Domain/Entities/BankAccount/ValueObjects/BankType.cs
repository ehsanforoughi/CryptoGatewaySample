using System.ComponentModel;

namespace CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

public enum BankType : byte
{
    [Description("بانک سپه")]
    Sepah = 1,

    [Description("بانک ملی ایران")]
    Melli = 2,

    [Description("بانک ملت")]
    Mellat = 3,

    [Description("بانک دی")]
    Dey = 4,

    [Description("بانک شهر")]
    Shahr = 5,

    [Description("بانک توسعه تعاون")]
    Tose_eTaavon = 6,

    [Description("بانک ایران زمین")]
    IranZamin = 7,

    [Description("بانک کشاورزی")]
    Keshavarzi = 8,

    [Description("بانک رفاه کارگران")]
    Refah = 9,

    [Description("بانک اقتصاد نوین")]
    EghtesadNovin = 10,

    [Description("بانک مهر اقتصاد")]
    MehreEghtesad = 11,

    [Description("بانک سرمایه")]
    Sarmayeh = 12,

    [Description("بانک آینده")]
    Ayandeh = 13,

    [Description("بانک کارآفرین")]
    Karafarin = 14,

    [Description("بانک پارسیان")]
    Parsian = 15,

    [Description("بانک گردشگری")]
    Gardeshgari = 16,

    [Description("بانک توسعه صادرات ایران")]
    Tose_eSaderat = 17,

    [Description("بانک سینا")]
    Sina = 18,

    [Description("بانک صادرات ایران")]
    Saderat = 19,

    [Description("بانک مسکن")]
    Maskan = 20,

    [Description("پست بانک ایران")]
    PostBank = 21,

    [Description("بانک انصار")]
    Ansar = 22,

    [Description("بانک سامان")]
    Saman = 23,

    [Description("بانک تجارت")]
    Tejarat = 24,

    [Description("بانک پاسارگاد")]
    Pasargad = 25,

    [Description("زرین پال")]
    ZarinPal = 26,

    [Description("بانک قرض الحسنه مهر ایران")]
    Mehre_Iran = 27,

    //[Description("پارس دید")]
    //ParsDid = 28,

    [Description("هیلاپی")]
    HillaPay = 40,

    [Description("بانک صنعت و معدن")]
    SanatoMadan = 41,

    [Description("بانک خاورمیانه")]
    Khavarmiane = 42,

    [Description("بانک قوامین")]
    Ghavamin = 43,

    //[Description("مشترک ایران-ونزوئلا")]
    //IranVenezuela = 44,

    [Description("بانک قرض الحسنه رسالت")]
    Resalat = 45,

    [Description("موسسه اعتباری کوثر")]
    Kosar = 46,

    [Description("موسسه اعتباری نور")]
    Noor = 47,

    [Description("موسسه اعتباری ملل")]
    Melal = 48,

    [Description("موسسه اعتباری توسعه")]
    EtebariEToseE = 49,

    [Description("بانک حکمت ایرانیان")]
    Hekmat = 50,

    [Description("پی پینگ")]
    PayPing = 51,

    [Description("هیلاپی دات نت")]
    HillaPayDotNet = 52,
}