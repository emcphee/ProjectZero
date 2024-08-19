using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project0.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration235529 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResponderAccountID = table.Column<int>(type: "int", nullable: true),
                    SenderAccountID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerTickets_AdminAccounts_ResponderAccountID",
                        column: x => x.ResponderAccountID,
                        principalTable: "AdminAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CustomerTickets_CustomerAccounts_SenderAccountID",
                        column: x => x.SenderAccountID,
                        principalTable: "CustomerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTickets_ResponderAccountID",
                table: "CustomerTickets",
                column: "ResponderAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTickets_SenderAccountID",
                table: "CustomerTickets",
                column: "SenderAccountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerTickets");
        }
    }
}
