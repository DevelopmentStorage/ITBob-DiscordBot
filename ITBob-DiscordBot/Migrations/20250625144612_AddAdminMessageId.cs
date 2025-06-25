using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITBob_DiscordBot.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminMessageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "AdminMessageId",
                table: "ReactionRoles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminMessageId",
                table: "ReactionRoles");
        }
    }
}
