using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project0.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountNo",
                table: "CustomerAccounts",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "AccountPassword",
                table: "CustomerAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "CustomerAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AccountCity",
                table: "CustomerAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CustomerAccounts_AccountName",
                table: "CustomerAccounts",
                column: "AccountName");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AdminAccounts_AccountName",
                table: "AdminAccounts",
                column: "AccountName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_CustomerAccounts_AccountName",
                table: "CustomerAccounts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AdminAccounts_AccountName",
                table: "AdminAccounts");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CustomerAccounts",
                newName: "AccountNo");

            migrationBuilder.AlterColumn<string>(
                name: "AccountPassword",
                table: "CustomerAccounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "CustomerAccounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "AccountCity",
                table: "CustomerAccounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
