using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovePayInSomeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerContact",
                schema: "Payment",
                table: "PayIn");

            migrationBuilder.DropColumn(
                name: "ExpiredAt",
                schema: "Payment",
                table: "PayIn");

            migrationBuilder.DropColumn(
                name: "PayInDesc",
                schema: "Payment",
                table: "PayIn");

            migrationBuilder.DropColumn(
                name: "State",
                schema: "Payment",
                table: "PayIn");

            migrationBuilder.DropColumn(
                name: "TransferType",
                schema: "Payment",
                table: "PayIn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerContact",
                schema: "Payment",
                table: "PayIn",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredAt",
                schema: "Payment",
                table: "PayIn",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PayInDesc",
                schema: "Payment",
                table: "PayIn",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "State",
                schema: "Payment",
                table: "PayIn",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "TransferType",
                schema: "Payment",
                table: "PayIn",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
