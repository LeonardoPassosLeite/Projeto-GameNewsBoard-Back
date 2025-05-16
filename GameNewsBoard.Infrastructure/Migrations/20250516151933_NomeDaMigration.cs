using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GameNewsBoard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NomeDaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Platform = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CoverImage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Rating = table.Column<double>(type: "double precision", nullable: false),
                    Released = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploadedImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameStatuses_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameStatuses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TierLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TierLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TierLists_UploadedImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "UploadedImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TierLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TierListEntry",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TierListId = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    Tier = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TierListEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TierListEntry_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TierListEntry_TierLists_TierListId",
                        column: x => x.TierListId,
                        principalTable: "TierLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameStatuses_GameId",
                table: "GameStatuses",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameStatuses_UserId_GameId",
                table: "GameStatuses",
                columns: new[] { "UserId", "GameId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TierListEntry_GameId",
                table: "TierListEntry",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_TierListEntry_TierListId_GameId",
                table: "TierListEntry",
                columns: new[] { "TierListId", "GameId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TierLists_ImageId",
                table: "TierLists",
                column: "ImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TierLists_UserId",
                table: "TierLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameStatuses");

            migrationBuilder.DropTable(
                name: "TierListEntry");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "TierLists");

            migrationBuilder.DropTable(
                name: "UploadedImages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
