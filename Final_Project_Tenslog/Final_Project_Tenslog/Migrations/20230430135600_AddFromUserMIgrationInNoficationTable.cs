using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Project_Tenslog.Migrations
{
    public partial class AddFromUserMIgrationInNoficationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromUserId",
                table: "Nofications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nofications_FromUserId",
                table: "Nofications",
                column: "FromUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nofications_AspNetUsers_FromUserId",
                table: "Nofications",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nofications_AspNetUsers_FromUserId",
                table: "Nofications");

            migrationBuilder.DropIndex(
                name: "IX_Nofications_FromUserId",
                table: "Nofications");

            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "Nofications");
        }
    }
}
