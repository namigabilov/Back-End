using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Project_Tenslog.Migrations
{
    public partial class FollowTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Follows",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Follows",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Follows",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Follows",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Follows",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Follows",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Follows",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Follows");
        }
    }
}
