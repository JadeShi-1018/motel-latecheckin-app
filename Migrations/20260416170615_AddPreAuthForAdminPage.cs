using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LateCheckInApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPreAuthForAdminPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminNote",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinalPaymentStatus",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PreAuthCapturedAt",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PreAuthReleasedAt",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminNote",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "FinalPaymentStatus",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "PreAuthCapturedAt",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "PreAuthReleasedAt",
                table: "_guestRegistrations");
        }
    }
}
