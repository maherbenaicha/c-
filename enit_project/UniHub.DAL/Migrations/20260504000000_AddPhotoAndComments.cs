using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniHub.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoAndComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ajouter colonnes photo à la table Activity
            migrationBuilder.AddColumn<byte[]>(
                name: "EventPhoto",
                schema: "Identity",
                table: "Activities",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventPhotoContentType",
                schema: "Identity",
                table: "Activities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            // Ajouter colonne ContentType pour ProfilePicture dans User
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureContentType",
                schema: "Identity",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            // Créer la table ActivityComments
            migrationBuilder.CreateTable(
                name: "ActivityComments",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityComments_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalSchema: "Identity",
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityComments_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityComments_ActivityId",
                schema: "Identity",
                table: "ActivityComments",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityComments_UserId",
                schema: "Identity",
                table: "ActivityComments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ActivityComments", schema: "Identity");

            migrationBuilder.DropColumn(name: "EventPhoto", schema: "Identity", table: "Activities");
            migrationBuilder.DropColumn(name: "EventPhotoContentType", schema: "Identity", table: "Activities");
            migrationBuilder.DropColumn(name: "ProfilePictureContentType", schema: "Identity", table: "User");
        }
    }
}
