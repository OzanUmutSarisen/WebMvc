using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMvc.Migrations
{
    public partial class addnewfeathertocourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers_FK_teacherId",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "databseFieldId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "databseId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_FK_teacherId",
                table: "Courses",
                column: "FK_teacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers_FK_teacherId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "databseFieldId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "databseId",
                table: "Courses");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers_FK_teacherId",
                table: "Courses",
                column: "FK_teacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }
    }
}
