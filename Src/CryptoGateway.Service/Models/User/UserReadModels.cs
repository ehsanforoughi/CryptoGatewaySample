using CryptoGateway.Domain.Extensions;
using CryptoGateway.Domain.Entities.Shared.ValueObjects;

namespace CryptoGateway.Service.Models.User;

public static class UserReadModels
{
    public class UserListItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal RealBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public byte CType { get; set; }
        public string CurrencyType => CType.Byte2EnumStr<CurrencyType>()!;
        public string CreatedAt { get; set; }
        public string ModifiedAt { get; set; }
    }
}