using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace buyyu.Data.Migrations
{
	public partial class AfterDDD2 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Payments_Orders_OrderId1",
				table: "Payments");

			migrationBuilder.DropIndex(
				name: "IX_Payments_OrderId1",
				table: "Payments");

			migrationBuilder.DropColumn(
				name: "OrderId1",
				table: "Payments");

			migrationBuilder.AlterColumn<Guid>(
				name: "OrderId",
				table: "Payments",
				type: "uniqueidentifier",
				nullable: true,
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier");

			migrationBuilder.CreateIndex(
				name: "IX_Payments_OrderId",
				table: "Payments",
				column: "OrderId");

			migrationBuilder.AddForeignKey(
				name: "FK_Payments_Orders_OrderId",
				table: "Payments",
				column: "OrderId",
				principalTable: "Orders",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Payments_Orders_OrderId",
				table: "Payments");

			migrationBuilder.DropIndex(
				name: "IX_Payments_OrderId",
				table: "Payments");

			migrationBuilder.AlterColumn<Guid>(
				name: "OrderId",
				table: "Payments",
				type: "uniqueidentifier",
				nullable: false,
				defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier",
				oldNullable: true);

			migrationBuilder.AddColumn<Guid>(
				name: "OrderId1",
				table: "Payments",
				type: "uniqueidentifier",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Payments_OrderId1",
				table: "Payments",
				column: "OrderId1");

			migrationBuilder.AddForeignKey(
				name: "FK_Payments_Orders_OrderId1",
				table: "Payments",
				column: "OrderId1",
				principalTable: "Orders",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}
	}
}