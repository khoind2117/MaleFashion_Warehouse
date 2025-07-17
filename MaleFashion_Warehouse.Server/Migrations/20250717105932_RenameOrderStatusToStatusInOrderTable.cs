using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaleFashion_Warehouse.Server.Migrations
{
    /// <inheritdoc />
    public partial class RenameOrderStatusToStatusInOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderStatus",
                table: "Order",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Order",
                newName: "OrderStatus");
        }
    }
}
