using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LateCheckInApp.Migrations
{
    /// <inheritdoc />
    public partial class AddStripeProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PreAuthAmount",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PreAuthCreatedAt",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreAuthPaymentIntentId",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreAuthStatus",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreAuthAmount",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "PreAuthCreatedAt",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "PreAuthPaymentIntentId",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "PreAuthStatus",
                table: "_guestRegistrations");
        }
    }
}
