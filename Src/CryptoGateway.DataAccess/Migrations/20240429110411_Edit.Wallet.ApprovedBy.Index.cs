using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditWalletApprovedByIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ApprovedBy",
                schema: "Base",
                table: "Wallet",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_ApprovedBy",
                schema: "Base",
                table: "Wallet",
                column: "ApprovedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_User_ApprovedBy",
                schema: "Base",
                table: "Wallet",
                column: "ApprovedBy",
                principalSchema: "Base",
                principalTable: "User",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_User_ApprovedBy",
                schema: "Base",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_ApprovedBy",
                schema: "Base",
                table: "Wallet");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovedBy",
                schema: "Base",
                table: "Wallet",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
