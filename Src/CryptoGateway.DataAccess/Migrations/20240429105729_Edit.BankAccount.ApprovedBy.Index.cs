using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditBankAccountApprovedByIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ApprovedBy",
                schema: "Base",
                table: "BankAccount",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_ApprovedBy",
                schema: "Base",
                table: "BankAccount",
                column: "ApprovedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccount_User_ApprovedBy",
                schema: "Base",
                table: "BankAccount",
                column: "ApprovedBy",
                principalSchema: "Base",
                principalTable: "User",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_User_ApprovedBy",
                schema: "Base",
                table: "BankAccount");

            migrationBuilder.DropIndex(
                name: "IX_BankAccount_ApprovedBy",
                schema: "Base",
                table: "BankAccount");

            migrationBuilder.AlterColumn<int>(
                name: "ApprovedBy",
                schema: "Base",
                table: "BankAccount",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
