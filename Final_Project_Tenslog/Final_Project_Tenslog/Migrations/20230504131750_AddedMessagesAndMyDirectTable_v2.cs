using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Project_Tenslog.Migrations
{
    public partial class AddedMessagesAndMyDirectTable_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyDirects_AspNetUsers_WriteingWithUserId",
                table: "MyDirects");

            migrationBuilder.AlterColumn<string>(
                name: "WriteingWithUserId",
                table: "MyDirects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "MyDirects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MyDirects_AppUserId",
                table: "MyDirects",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyDirects_AspNetUsers_AppUserId",
                table: "MyDirects",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MyDirects_AspNetUsers_WriteingWithUserId",
                table: "MyDirects",
                column: "WriteingWithUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyDirects_AspNetUsers_AppUserId",
                table: "MyDirects");

            migrationBuilder.DropForeignKey(
                name: "FK_MyDirects_AspNetUsers_WriteingWithUserId",
                table: "MyDirects");

            migrationBuilder.DropIndex(
                name: "IX_MyDirects_AppUserId",
                table: "MyDirects");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "MyDirects");

            migrationBuilder.AlterColumn<string>(
                name: "WriteingWithUserId",
                table: "MyDirects",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_MyDirects_AspNetUsers_WriteingWithUserId",
                table: "MyDirects",
                column: "WriteingWithUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
