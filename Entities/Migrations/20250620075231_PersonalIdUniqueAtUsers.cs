using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class PersonalIdUniqueAtUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonalId",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PersonalId",
                table: "AspNetUsers",
                column: "PersonalId",
                unique: true,
                filter: "[PersonalId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PersonalId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PersonalId",
                table: "AspNetUsers");
        }
    }
}
