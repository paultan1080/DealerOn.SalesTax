using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DealerOn.SalesTax.Domain.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ClassifierDefinitions",
                columns: new[] { "Id", "Pattern", "TraitType", "TraitValue", "Type" },
                values: new object[,]
                {
                    { 1, "imported", "ItemIsImported", "", "Keyword" },
                    { 2, "book", "ItemCategory", "Books", "Keyword" },
                    { 3, "chocolate", "ItemCategory", "Food", "Keyword" },
                    { 4, "pills", "ItemCategory", "Medical", "Keyword" }
                });

            migrationBuilder.InsertData(
                table: "TaxConfigs",
                columns: new[] { "Id", "ImportDutyRate", "SalesTaxRate" },
                values: new object[] { 1, 0.05m, 0.10m });

            migrationBuilder.InsertData(
                table: "TaxExemptCategories",
                columns: new[] { "Id", "CategoryName", "TaxConfigEntityId" },
                values: new object[,]
                {
                    { 1, "Food", null },
                    { 2, "Books", null },
                    { 3, "Medical", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ClassifierDefinitions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ClassifierDefinitions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ClassifierDefinitions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ClassifierDefinitions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TaxConfigs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TaxExemptCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TaxExemptCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TaxExemptCategories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
