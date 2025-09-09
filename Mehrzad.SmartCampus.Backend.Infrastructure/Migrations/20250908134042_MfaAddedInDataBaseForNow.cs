using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mehrzad.SmartCampus.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MfaAddedInDataBaseForNow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OtpExpiry",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PendingOtp",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtpExpiry",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PendingOtp",
                table: "Users");
        }
    }
}
