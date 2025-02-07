using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddContractTransactionContractAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContractAddress",
                schema: "Contract",
                table: "ContractTransaction",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractAddress",
                schema: "Contract",
                table: "ContractTransaction");
        }
    }
}
