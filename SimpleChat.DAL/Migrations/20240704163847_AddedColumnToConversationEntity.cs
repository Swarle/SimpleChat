using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleChat.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnToConversationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Conversations",
                newName: "Title");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAd",
                table: "Conversations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAd",
                table: "Conversations");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Conversations",
                newName: "Name");
        }
    }
}
