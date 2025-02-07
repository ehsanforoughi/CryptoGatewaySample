using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditPayInCustomerIdIsRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                schema: "Payment",
                table: "PayIn",
                type: "decimal(24,12)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(24,12)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                schema: "Payment",
                table: "PayIn",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "CurrencyType",
                schema: "Payment",
                table: "PayIn",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                schema: "Payment",
                table: "PayIn",
                type: "decimal(24,12)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(24,12)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                schema: "Payment",
                table: "PayIn",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte>(
                name: "CurrencyType",
                schema: "Payment",
                table: "PayIn",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)0);
        }
    }
}
