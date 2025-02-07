using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

namespace CryptoGateway.Service.Models.Wallet;

public static class WalletReadModels
{
    public class WalletListItem
    {
        public int WalletId { get; set; }
        public string UserlId { get; set; }
        public string Title { get; set; }
        public string ApprovedBy { get; set; }
        public string Approver { get; set; }
        public byte State { get; set; }
        public string? ApprovingState => State.Byte2EnumStr<ApprovingState>();
        //public int CurrencyId { get; set; }
        public byte CType { get; set; }
        public string CurrencyType => CType.Byte2EnumStr<CurrencyType>()!;
        public string Network { get; set; }
        public string Address { get; set; }
        public string Memo { get; set; }
        public string Tag { get; set; }
        public string Desc { get; set; }
        public string CreatedAt { get; set; }
        public string ExpiredAt { get; set; }
    }

    public class ApprovedListItem
    {
        public int WalletId { get; set; }
        public string WalletTitle { get; set; }
        public string WalletAddress { get; set; }
        public string CurrencyType { get; set; }
        public string SelectTitle => $"{CurrencyType} - {WalletTitle} - {WalletAddress.Substring(0, 6)}...";
    }
}