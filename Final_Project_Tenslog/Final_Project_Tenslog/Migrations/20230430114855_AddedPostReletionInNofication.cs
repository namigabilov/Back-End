using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Project_Tenslog.Migrations
{
    public partial class AddedPostReletionInNofication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Nofications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nofications_PostId",
                table: "Nofications",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nofications_Posts_PostId",
                table: "Nofications",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nofications_Posts_PostId",
                table: "Nofications");

            migrationBuilder.DropIndex(
                name: "IX_Nofications_PostId",
                table: "Nofications");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Nofications");
        }
    }
}
