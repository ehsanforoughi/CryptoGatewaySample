using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPayInCommission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "ComCurrencyType",
                schema: "Payment",
                table: "PayIn",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<decimal>(
                name: "ComFixedValue",
                schema: "Payment",
                table: "PayIn",
                type: "decimal(24,12)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ComPercentage",
                schema: "Payment",
                table: "PayIn",
                type: "decimal(24,12)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComCurrencyType",
                schema: "Payment",
                table: "PayIn");

            migrationBuilder.DropColumn(
                name: "ComFixedValue",
                schema: "Payment",
                table: "PayIn");

            migrationBuilder.DropColumn(
                name: "ComPercentage",
                schema: "Payment",
                table: "PayIn");
        }
    }
}
