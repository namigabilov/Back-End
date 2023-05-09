using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Project_Tenslog.Migrations
{
    public partial class UpdatedSupporttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReasonFromAdmin",
                table: "VerificationRequests");

            migrationBuilder.AddColumn<string>(
                name: "AdminAnswer",
                table: "Supports",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminAnswer",
                table: "Supports");

            migrationBuilder.AddColumn<string>(
                name: "ReasonFromAdmin",
                table: "VerificationRequests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
