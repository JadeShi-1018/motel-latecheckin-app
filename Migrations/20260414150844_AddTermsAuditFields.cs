using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LateCheckInApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTermsAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DepositAuthorizationAcceptedAtUtc",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TermsAcceptedAtUtc",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TermsEffectiveFromUtc",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TermsVersion",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepositAuthorizationAcceptedAtUtc",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "TermsAcceptedAtUtc",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "TermsEffectiveFromUtc",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "TermsVersion",
                table: "_guestRegistrations");
        }
    }
}
