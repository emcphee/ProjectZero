using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project0.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration264037 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnixTimestamp = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    sourceAccountId = table.Column<int>(type: "int", nullable: false),
                    destinationAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_CustomerAccounts_destinationAccountId",
                        column: x => x.destinationAccountId,
                        principalTable: "CustomerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_CustomerAccounts_sourceAccountId",
                        column: x => x.sourceAccountId,
                        principalTable: "CustomerAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_destinationAccountId",
                table: "Transactions",
                column: "destinationAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_sourceAccountId",
                table: "Transactions",
                column: "sourceAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
