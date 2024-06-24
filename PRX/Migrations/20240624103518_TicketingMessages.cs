using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRX.Migrations
{
    /// <inheritdoc />
    public partial class TicketingMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Admins_SenderId",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AdminId",
                table: "Messages",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Admins_AdminId",
                table: "Messages",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Admins_AdminId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_AdminId",
                table: "Messages");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Admins_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
