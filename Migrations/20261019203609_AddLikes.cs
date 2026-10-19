using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Anonim.Migrations
{
    /// <inheritdoc />
    public partial class AddLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Itiraflar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Hikayeler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HikayeBegeniler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HikayeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HikayeBegeniler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HikayeBegeniler_Hikayeler_HikayeId",
                        column: x => x.HikayeId,
                        principalTable: "Hikayeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HikayeBegeniler_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItirafBegeniler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItirafId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItirafBegeniler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItirafBegeniler_Itiraflar_ItirafId",
                        column: x => x.ItirafId,
                        principalTable: "Itiraflar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItirafBegeniler_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HikayeBegeniler_HikayeId_UserId",
                table: "HikayeBegeniler",
                columns: new[] { "HikayeId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HikayeBegeniler_UserId",
                table: "HikayeBegeniler",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItirafBegeniler_ItirafId_UserId",
                table: "ItirafBegeniler",
                columns: new[] { "ItirafId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItirafBegeniler_UserId",
                table: "ItirafBegeniler",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HikayeBegeniler");

            migrationBuilder.DropTable(
                name: "ItirafBegeniler");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Itiraflar");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Hikayeler");
        }
    }
}
