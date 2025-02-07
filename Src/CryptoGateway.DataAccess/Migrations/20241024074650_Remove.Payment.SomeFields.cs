using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovePaymentSomeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComCurrencyType",
                schema: "Payment",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ComFixedValue",
                schema: "Payment",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "ComPercentage",
                schema: "Payment",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "Payment",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "Payment",
                table: "Payment");

            migrationBuilder.AddColumn<string>(
                name: "CustomerContact",
                schema: "Payment",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerContact",
                schema: "Payment",
                table: "Payment");

            migrationBuilder.AddColumn<byte>(
                name: "ComCurrencyType",
                schema: "Payment",
                table: "Payment",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<decimal>(
                name: "ComFixedValue",
                schema: "Payment",
                table: "Payment",
                type: "decimal(24,12)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ComPercentage",
                schema: "Payment",
                table: "Payment",
                type: "decimal(24,12)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<byte>(
                name: "State",
                schema: "Payment",
                table: "Payment",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                schema: "Payment",
                table: "Payment",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
