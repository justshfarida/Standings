using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Standings.Persistence.Migrations
{
    public partial class firstVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "39644e73-869c-4351-b0f8-cefdaa08ce13");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "55209a98-013c-4d58-8d5f-81930a20f16c");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "bc72f051-4a37-42aa-9fb7-043c33b4fbe6");

            migrationBuilder.AddColumn<int>(
                name: "ResultId",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "AverageGrade",
                table: "Average",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenEndTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "851fdfac-e227-4687-86e1-560ecf7133f3", "db7aee5a-2c9c-4835-abe0-6d3c39d229ee", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "90feef2e-126c-4290-9f15-eaaae56b37c3", "0eca7d4a-93a4-4386-85cc-c53dbc926ca6", "Student", "STUDENT" });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c21d0b80-b549-4e83-a852-6628b16ac3e8", "a6fb36b3-927d-428d-9627-72ad7d52511a", "Moderator", "MODERATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "851fdfac-e227-4687-86e1-560ecf7133f3");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "90feef2e-126c-4290-9f15-eaaae56b37c3");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "c21d0b80-b549-4e83-a852-6628b16ac3e8");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "AverageGrade",
                table: "Average");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenEndTime",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "39644e73-869c-4351-b0f8-cefdaa08ce13", "10dd421d-d97e-461f-af4f-4c0a6417495d", "Moderator", "MODERATOR" });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "55209a98-013c-4d58-8d5f-81930a20f16c", "bd4502ab-9ed4-43eb-96e2-4db3072d699b", "User", "USER" });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bc72f051-4a37-42aa-9fb7-043c33b4fbe6", "2cb24a12-253c-41a6-a40b-645966c4a90d", "Admin", "ADMIN" });
        }
    }
}
