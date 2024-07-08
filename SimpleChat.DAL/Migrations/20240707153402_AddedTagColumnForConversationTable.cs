using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleChat.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedTagColumnForConversationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Conversations",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Conversations");
        }
    }
}
