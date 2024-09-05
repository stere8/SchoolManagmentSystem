using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddTeacherIdToClasses : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "TeacherId",
            table: "Classes",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Classes_TeacherId",
            table: "Classes",
            column: "TeacherId");

        migrationBuilder.AddForeignKey(
            name: "FK_Classes_Teachers_TeacherId",
            table: "Classes",
            column: "TeacherId",
            principalTable: "Teachers",
            principalColumn: "TeacherId",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Classes_Teachers_TeacherId",
            table: "Classes");

        migrationBuilder.DropIndex(
            name: "IX_Classes_TeacherId",
            table: "Classes");

        migrationBuilder.DropColumn(
            name: "TeacherId",
            table: "Classes");
    }
}