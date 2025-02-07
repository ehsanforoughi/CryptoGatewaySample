using Bat.Core;
using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;
using CryptoGateway.Domain.Entities.BankAccount.ValueObjects;

namespace CryptoGateway.Service.Models.BankAccount;

public static class BankAccountReadModels
{
    public class BankAccountListItem
    {
        public int BankAccountId { get; set; }
        public string UserlId { get; set; }
        public string Title { get; set; }
        public string ApprovedBy { get; set; }
        public byte State { get; set; }
        public string? ApprovingState => State.Byte2EnumStr<ApprovingState>();
        public byte BankType { get; set; }
        public string? BankName => BankType.Byte2Enum<BankType>().GetDescription();
        public string CardNumber { get; set; }
        public string Sheba { get; set; }
        public string AccountNumber { get; set; }
        public string Desc { get; set; }
        public string CreatedAt { get; set; }
        public string ExpiredAt { get; set; }
    }

    public class ApprovedListItem
    {
        public int BankAccountId { get; set; }
        public byte BankType { get; set; }
        public string? BankName => BankType.Byte2Enum<BankType>().GetDescription();
        public string CardNumber { get; set; }
        public string SelectTitle => $"{BankName} - {CardNumber.Substring(CardNumber.Length - 4)}";
    }

    public class BankTypeListItem
    {
        public int BankTypeId { get; set; }
        public string BankName { get; set; }
        public string BankType { get; set; }
    }
}