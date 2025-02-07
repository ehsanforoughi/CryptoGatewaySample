using CryptoGateway.Framework;

namespace CryptoGateway.Domain.Entities.Shared.ValueObjects
{
    public class Network : Value<Network>
    {
        // Satisfy the serialization requirements
        protected Network() { }
        protected Network(string network) => Value = network;

        public static Network FromString(string network)
        {
            CheckValidity(network);
            return new Network(network);
        }

        public static implicit operator string(Network self) => self.Value;
        public string Value { get; internal set; }
        public static Network NoNetwork => new();
        private static void CheckValidity(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(Network), "Network cannot be empty");

            if (value.Length > 10)
                throw new ArgumentOutOfRangeException(nameof(Network), "Network cannot be longer that 10 characters");
        }
    }
}
