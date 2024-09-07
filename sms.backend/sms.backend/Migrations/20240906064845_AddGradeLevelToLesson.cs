using Microsoft.EntityFrameworkCore.Migrations;

public partial class AddGradeLevelToLesson : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "GradeLevel",
            table: "Lessons",
            nullable: false,
            defaultValue: 0);

        // Ensure the sp_rename command is correct
        migrationBuilder.Sql("EXEC sp_rename N'[Marks].[Value]', N'MarkValue', N'COLUMN';");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "GradeLevel",
            table: "Lessons");

        // Revert the sp_rename command if necessary
        migrationBuilder.Sql("EXEC sp_rename N'[Marks].[MarkValue]', N'Value', N'COLUMN';");
    }
}