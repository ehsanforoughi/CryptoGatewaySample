using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditContractAccountCustodyAccountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustodyAccount_ContractAccount_ContractAccountId",
                schema: "Payment",
                table: "CustodyAccount");

            migrationBuilder.DropIndex(
                name: "IX_CustodyAccount_ContractAccountId",
                schema: "Payment",
                table: "CustodyAccount");

            migrationBuilder.DropColumn(
                name: "ContractAccountId",
                schema: "Payment",
                table: "CustodyAccount");

            migrationBuilder.AddColumn<int>(
                name: "CustodyAccountId",
                schema: "Contract",
                table: "ContractAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ContractAccount_CustodyAccountId",
                schema: "Contract",
                table: "ContractAccount",
                column: "CustodyAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractAccount_CustodyAccount_CustodyAccountId",
                schema: "Contract",
                table: "ContractAccount",
                column: "CustodyAccountId",
                principalSchema: "Payment",
                principalTable: "CustodyAccount",
                principalColumn: "CustodyAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractAccount_CustodyAccount_CustodyAccountId",
                schema: "Contract",
                table: "ContractAccount");

            migrationBuilder.DropIndex(
                name: "IX_ContractAccount_CustodyAccountId",
                schema: "Contract",
                table: "ContractAccount");

            migrationBuilder.DropColumn(
                name: "CustodyAccountId",
                schema: "Contract",
                table: "ContractAccount");

            migrationBuilder.AddColumn<int>(
                name: "ContractAccountId",
                schema: "Payment",
                table: "CustodyAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustodyAccount_ContractAccountId",
                schema: "Payment",
                table: "CustodyAccount",
                column: "ContractAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustodyAccount_ContractAccount_ContractAccountId",
                schema: "Payment",
                table: "CustodyAccount",
                column: "ContractAccountId",
                principalSchema: "Contract",
                principalTable: "ContractAccount",
                principalColumn: "ContractAccountId");
        }
    }
}
