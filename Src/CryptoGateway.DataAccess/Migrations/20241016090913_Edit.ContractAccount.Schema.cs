using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditContractAccountSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ContractAccount",
                schema: "Payment",
                newName: "ContractAccount",
                newSchema: "Contract");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ContractAccount",
                schema: "Contract",
                newName: "ContractAccount",
                newSchema: "Payment");
        }
    }
}
