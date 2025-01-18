using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _6.NovaPoshta.Migrations
{
    /// <inheritdoc />
    public partial class Add_tbl_depatrments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ref = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CityRef = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_departments_tbl_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "tbl_cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_departments_CityId",
                table: "tbl_departments",
                column: "CityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_departments");
        }
    }
}
