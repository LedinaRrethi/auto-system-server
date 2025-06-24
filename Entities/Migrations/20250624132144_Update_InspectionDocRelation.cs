using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Update_InspectionDocRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auto_InspectionDocs_Auto_Inspections_IDFK_InspectionRequest",
                table: "Auto_InspectionDocs");

            migrationBuilder.RenameColumn(
                name: "IDFK_InspectionRequest",
                table: "Auto_InspectionDocs",
                newName: "IDFK_Inspection");

            migrationBuilder.RenameIndex(
                name: "IX_Auto_InspectionDocs_IDFK_InspectionRequest",
                table: "Auto_InspectionDocs",
                newName: "IX_Auto_InspectionDocs_IDFK_Inspection");

            migrationBuilder.AddForeignKey(
                name: "FK_Auto_InspectionDocs_Auto_Inspections_IDFK_Inspection",
                table: "Auto_InspectionDocs",
                column: "IDFK_Inspection",
                principalTable: "Auto_Inspections",
                principalColumn: "IDPK_Inspection",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auto_InspectionDocs_Auto_Inspections_IDFK_Inspection",
                table: "Auto_InspectionDocs");

            migrationBuilder.RenameColumn(
                name: "IDFK_Inspection",
                table: "Auto_InspectionDocs",
                newName: "IDFK_InspectionRequest");

            migrationBuilder.RenameIndex(
                name: "IX_Auto_InspectionDocs_IDFK_Inspection",
                table: "Auto_InspectionDocs",
                newName: "IX_Auto_InspectionDocs_IDFK_InspectionRequest");

            migrationBuilder.AddForeignKey(
                name: "FK_Auto_InspectionDocs_Auto_Inspections_IDFK_InspectionRequest",
                table: "Auto_InspectionDocs",
                column: "IDFK_InspectionRequest",
                principalTable: "Auto_Inspections",
                principalColumn: "IDPK_Inspection",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
