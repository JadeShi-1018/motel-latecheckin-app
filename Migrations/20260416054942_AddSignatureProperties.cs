using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LateCheckInApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSignatureProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoIdContentType",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PhotoIdFileSize",
                table: "_guestRegistrations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoIdOriginalFileName",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PhotoIdUploadedAt",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignaturePath",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SignedAt",
                table: "_guestRegistrations",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoIdContentType",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "PhotoIdFileSize",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "PhotoIdOriginalFileName",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "PhotoIdUploadedAt",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "SignaturePath",
                table: "_guestRegistrations");

            migrationBuilder.DropColumn(
                name: "SignedAt",
                table: "_guestRegistrations");
        }
    }
}
