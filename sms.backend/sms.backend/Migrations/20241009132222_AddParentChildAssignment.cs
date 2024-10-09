using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sms.backend.Migrations
{
    /// <inheritdoc />
    public partial class AddParentChildAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Classes_Teachers_TeacherId",
            //    table: "Classes");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Teachers_Users_UserId",
            //    table: "Teachers");

            //migrationBuilder.DropIndex(
            //    name: "IX_Teachers_UserId",
            //    table: "Teachers");

            //migrationBuilder.DropIndex(
            //    name: "IX_Classes_TeacherId",
            //    table: "Classes");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "EntityId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubjectExpertise",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GradeLevel",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentContactInfo",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ParentChildAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ChildId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentChildAssignments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParentChildAssignments");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "SubjectExpertise",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "GradeLevel",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ParentContactInfo",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "Users",
                newName: "Username");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Teachers_UserId",
            //    table: "Teachers",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Classes_TeacherId",
            //    table: "Classes",
            //    column: "TeacherId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Classes_Teachers_TeacherId",
            //    table: "Classes",
            //    column: "TeacherId",
            //    principalTable: "Teachers",
            //    principalColumn: "TeacherId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Teachers_Users_UserId",
            //    table: "Teachers",
            //    column: "UserId",
            //    principalTable: "Users",
            //    principalColumn: "UserId",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
