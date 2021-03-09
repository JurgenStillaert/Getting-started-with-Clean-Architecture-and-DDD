using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace buyyu.Data.Migrations
{
	public partial class AfterCQRS1 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Shipments",
				columns: table => new
				{
					OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ShipmentDate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Shipments", x => x.OrderId);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Shipments");
		}
	}
}