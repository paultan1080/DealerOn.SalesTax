using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealerOn.SalesTax.Domain.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassifierDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Pattern = table.Column<string>(type: "TEXT", nullable: false),
                    TraitType = table.Column<string>(type: "TEXT", nullable: false),
                    TraitValue = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassifierDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SalesTaxRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    ImportDutyRate = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxExemptCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false),
                    TaxConfigEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxExemptCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxExemptCategories_TaxConfigs_TaxConfigEntityId",
                        column: x => x.TaxConfigEntityId,
                        principalTable: "TaxConfigs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxExemptCategories_TaxConfigEntityId",
                table: "TaxExemptCategories",
                column: "TaxConfigEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassifierDefinitions");

            migrationBuilder.DropTable(
                name: "TaxExemptCategories");

            migrationBuilder.DropTable(
                name: "TaxConfigs");
        }
    }
}
