using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealerOn.SalesTax.Domain.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class TaxIdFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxExemptCategories_TaxConfigs_TaxConfigEntityId",
                table: "TaxExemptCategories");

            migrationBuilder.AlterColumn<int>(
                name: "TaxConfigEntityId",
                table: "TaxExemptCategories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "TaxExemptCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "TaxConfigEntityId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TaxExemptCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "TaxConfigEntityId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TaxExemptCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "TaxConfigEntityId",
                value: 1);

            migrationBuilder.AddForeignKey(
                name: "FK_TaxExemptCategories_TaxConfigs_TaxConfigEntityId",
                table: "TaxExemptCategories",
                column: "TaxConfigEntityId",
                principalTable: "TaxConfigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxExemptCategories_TaxConfigs_TaxConfigEntityId",
                table: "TaxExemptCategories");

            migrationBuilder.AlterColumn<int>(
                name: "TaxConfigEntityId",
                table: "TaxExemptCategories",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "TaxExemptCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "TaxConfigEntityId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaxExemptCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "TaxConfigEntityId",
                value: null);

            migrationBuilder.UpdateData(
                table: "TaxExemptCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "TaxConfigEntityId",
                value: null);

            migrationBuilder.AddForeignKey(
                name: "FK_TaxExemptCategories_TaxConfigs_TaxConfigEntityId",
                table: "TaxExemptCategories",
                column: "TaxConfigEntityId",
                principalTable: "TaxConfigs",
                principalColumn: "Id");
        }
    }
}
