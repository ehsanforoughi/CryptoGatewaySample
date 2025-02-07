using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditCustodyAccountOnoToOneContractAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContractAccount_CustodyAccountId",
                schema: "Contract",
                table: "ContractAccount");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAccount_CustodyAccountId",
                schema: "Contract",
                table: "ContractAccount",
                column: "CustodyAccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContractAccount_CustodyAccountId",
                schema: "Contract",
                table: "ContractAccount");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAccount_CustodyAccountId",
                schema: "Contract",
                table: "ContractAccount",
                column: "CustodyAccountId");
        }
    }
}
