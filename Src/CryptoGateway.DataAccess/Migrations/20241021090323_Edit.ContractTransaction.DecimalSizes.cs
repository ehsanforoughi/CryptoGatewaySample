using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoGateway.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class EditContractTransactionDecimalSizes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "GasPrice",
                schema: "Contract",
                table: "ContractTransaction",
                type: "decimal(24,12)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FeeLimit",
                schema: "Contract",
                table: "ContractTransaction",
                type: "decimal(24,12)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "EnergyFee",
                schema: "Contract",
                table: "ContractTransaction",
                type: "decimal(24,12)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BandwidthFee",
                schema: "Contract",
                table: "ContractTransaction",
                type: "decimal(24,12)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "Contract",
                table: "ContractTransaction",
                type: "decimal(24,12)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "GasPrice",
                schema: "Contract",
                table: "ContractTransaction",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(24,12)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FeeLimit",
                schema: "Contract",
                table: "ContractTransaction",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(24,12)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "EnergyFee",
                schema: "Contract",
                table: "ContractTransaction",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(24,12)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BandwidthFee",
                schema: "Contract",
                table: "ContractTransaction",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(24,12)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "Contract",
                table: "ContractTransaction",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(24,12)",
                oldNullable: true);
        }
    }
}
