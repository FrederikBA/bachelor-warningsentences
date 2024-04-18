using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WarningPictograms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pictogram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarningPictograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarningSignalWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SignalWordText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarningSignalWords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarningTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarningTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarningCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarningTypeId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarningCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarningCategories_WarningTypes_WarningTypeId",
                        column: x => x.WarningTypeId,
                        principalTable: "WarningTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarningSentences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarningCategoryId = table.Column<int>(type: "int", nullable: false),
                    WarningPictogramId = table.Column<int>(type: "int", nullable: false),
                    WarningSignalWordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarningSentences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarningSentences_WarningCategories_WarningCategoryId",
                        column: x => x.WarningCategoryId,
                        principalTable: "WarningCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarningSentences_WarningPictograms_WarningPictogramId",
                        column: x => x.WarningPictogramId,
                        principalTable: "WarningPictograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarningSentences_WarningSignalWords_WarningSignalWordId",
                        column: x => x.WarningSignalWordId,
                        principalTable: "WarningSignalWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarningCategories_WarningTypeId",
                table: "WarningCategories",
                column: "WarningTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarningSentences_WarningCategoryId",
                table: "WarningSentences",
                column: "WarningCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WarningSentences_WarningPictogramId",
                table: "WarningSentences",
                column: "WarningPictogramId");

            migrationBuilder.CreateIndex(
                name: "IX_WarningSentences_WarningSignalWordId",
                table: "WarningSentences",
                column: "WarningSignalWordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarningSentences");

            migrationBuilder.DropTable(
                name: "WarningCategories");

            migrationBuilder.DropTable(
                name: "WarningPictograms");

            migrationBuilder.DropTable(
                name: "WarningSignalWords");

            migrationBuilder.DropTable(
                name: "WarningTypes");
        }
    }
}
