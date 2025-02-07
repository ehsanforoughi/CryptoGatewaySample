using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPayInPayInExternalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayInExternalId",
                schema: "Payment",
                table: "PayIn",
                type: "nvarchar(27)",
                maxLength: 27,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PayIn_PayInExternalId",
                schema: "Payment",
                table: "PayIn",
                column: "PayInExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PayIn_PayInExternalId",
                schema: "Payment",
                table: "PayIn");

            migrationBuilder.DropColumn(
                name: "PayInExternalId",
                schema: "Payment",
                table: "PayIn");
        }
    }
}
