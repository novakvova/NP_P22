using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _6.NovaPoshta.Migrations
{
    /// <inheritdoc />
    public partial class Add_tbl_cities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ref = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    AreaRef = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    TypeDescription = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    AreaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_cities_tbl_areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "tbl_areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cities_AreaId",
                table: "tbl_cities",
                column: "AreaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_cities");
        }
    }
}
