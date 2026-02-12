using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPortal.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLookupItemTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LookupItem_LookupItem_ParentId",
                table: "LookupItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LookupItem",
                table: "LookupItem");

            migrationBuilder.RenameTable(
                name: "LookupItem",
                newName: "LookupItems");

            migrationBuilder.RenameIndex(
                name: "IX_LookupItem_Type_Code",
                table: "LookupItems",
                newName: "IX_LookupItems_Type_Code");

            migrationBuilder.RenameIndex(
                name: "IX_LookupItem_Type",
                table: "LookupItems",
                newName: "IX_LookupItems_Type");

            migrationBuilder.RenameIndex(
                name: "IX_LookupItem_ParentId_Name",
                table: "LookupItems",
                newName: "IX_LookupItems_ParentId_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LookupItems",
                table: "LookupItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LookupItems_LookupItems_ParentId",
                table: "LookupItems",
                column: "ParentId",
                principalTable: "LookupItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LookupItems_LookupItems_ParentId",
                table: "LookupItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LookupItems",
                table: "LookupItems");

            migrationBuilder.RenameTable(
                name: "LookupItems",
                newName: "LookupItem");

            migrationBuilder.RenameIndex(
                name: "IX_LookupItems_Type_Code",
                table: "LookupItem",
                newName: "IX_LookupItem_Type_Code");

            migrationBuilder.RenameIndex(
                name: "IX_LookupItems_Type",
                table: "LookupItem",
                newName: "IX_LookupItem_Type");

            migrationBuilder.RenameIndex(
                name: "IX_LookupItems_ParentId_Name",
                table: "LookupItem",
                newName: "IX_LookupItem_ParentId_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LookupItem",
                table: "LookupItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LookupItem_LookupItem_ParentId",
                table: "LookupItem",
                column: "ParentId",
                principalTable: "LookupItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
