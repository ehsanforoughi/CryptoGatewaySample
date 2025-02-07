using Bat.Core;
using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

namespace CryptoGateway.Service.Models.Payout;

public static class PayoutReadModels
{


    public class FiatPayoutListItem
    {
        public int PayoutId { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string ApproverFullName { get; set; }
        public decimal Value { get; set; }
        public byte CType { get; set; }
        public string CurrencyType => CType.Byte2EnumStr<CurrencyType>()!;
        public byte State { get; set; }
        public string ApprovingState => State.Byte2EnumStr<ApprovingState>();
        public byte BType { get; set; }
        public string BankType => BType.Byte2Enum<BankType>().GetDescription();
        public string CardNumber { get; set; }
        //public byte TType { get; set; }
        //public string TransferType => TType.Byte2EnumStr<TransferType>()!;
        public string BankTrackingCode { get; set; }
        public string ApproverDesc { get; set; }
        public string CreatedAt { get; set; }
        public string ModifiedAt { get; set; }
    }

    public class CryptoPayoutListItem
    {
        public int PayoutId { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string ApproverFullName { get; set; }
        public decimal Value { get; set; }
        public byte CType { get; set; }
        public string CurrencyType => CType.Byte2EnumStr<CurrencyType>()!;
        public byte State { get; set; }
        public string? ApprovingState => State.Byte2EnumStr<ApprovingState>();
        public string WalletAddress { get; set; }
        //public byte TType { get; set; }
        //public string TransferType => TType.Byte2EnumStr<TransferType>()!;
        public string TransactionUrl { get; set; }
        public string ApproverDesc { get; set; }
        public string CreatedAt { get; set; }
        public string ModifiedAt { get; set; }
    }

    public class WaitingPayoutsListItem
    {

    }
}