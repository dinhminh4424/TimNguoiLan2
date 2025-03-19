using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimNguoiThan.Migrations
{
    /// <inheritdoc />
    public partial class cangcuoccongdan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Age",
                table: "AspNetUsers",
                newName: "CCCD");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgaySinh",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgaySinh",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CCCD",
                table: "AspNetUsers",
                newName: "Age");
        }
    }
}
