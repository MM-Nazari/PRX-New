using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRX.Migrations
{
    /// <inheritdoc />
    public partial class TicketingMessagesAdminTableConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Admins_AdminId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropIndex(
                name: "IX_Messages_AdminId",
                table: "Messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

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
    }
}
