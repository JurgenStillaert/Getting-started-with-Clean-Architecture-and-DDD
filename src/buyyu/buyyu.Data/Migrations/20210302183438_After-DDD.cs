using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace buyyu.Data.Migrations
{
	public partial class AfterDDD : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_OrderLines_Orders_OrderId",
				table: "OrderLines");

			migrationBuilder.DropForeignKey(
				name: "FK_OrderLines_Products_ProductId",
				table: "OrderLines");

			migrationBuilder.DropForeignKey(
				name: "FK_Payments_Orders_OrderId",
				table: "Payments");

			migrationBuilder.DropIndex(
				name: "IX_Payments_OrderId",
				table: "Payments");

			migrationBuilder.DropIndex(
				name: "IX_OrderLines_ProductId",
				table: "OrderLines");

			migrationBuilder.AddColumn<Guid>(
				name: "OrderId1",
				table: "Payments",
				type: "uniqueidentifier",
				nullable: true);

			migrationBuilder.AlterColumn<decimal>(
				name: "TotalAmount",
				table: "Orders",
				type: "decimal(18,2)",
				nullable: true,
				oldClrType: typeof(decimal),
				oldType: "decimal(18,2)");

			migrationBuilder.AlterColumn<decimal>(
				name: "PaidAmount",
				table: "Orders",
				type: "decimal(18,2)",
				nullable: true,
				oldClrType: typeof(decimal),
				oldType: "decimal(18,2)");

			migrationBuilder.AlterColumn<DateTime>(
				name: "OrderDate",
				table: "Orders",
				type: "datetime2",
				nullable: true,
				oldClrType: typeof(DateTime),
				oldType: "datetime2");

			migrationBuilder.AlterColumn<Guid>(
				name: "ClientId",
				table: "Orders",
				type: "uniqueidentifier",
				nullable: true,
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier");

			migrationBuilder.AlterColumn<Guid>(
				name: "Id",
				table: "Orders",
				type: "uniqueidentifier",
				nullable: false,
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier",
				oldDefaultValueSql: "NEWID()");

			migrationBuilder.AddColumn<string>(
				name: "PaidAmount_Currency",
				table: "Orders",
				type: "nvarchar(max)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "TotalAmount_Currency",
				table: "Orders",
				type: "nvarchar(max)",
				nullable: true);

			migrationBuilder.AlterColumn<int>(
				name: "Qty",
				table: "OrderLines",
				type: "int",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "int");

			migrationBuilder.AlterColumn<Guid>(
				name: "ProductId",
				table: "OrderLines",
				type: "uniqueidentifier",
				nullable: true,
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier");

			migrationBuilder.AlterColumn<decimal>(
				name: "Price",
				table: "OrderLines",
				type: "decimal(18,2)",
				nullable: true,
				oldClrType: typeof(decimal),
				oldType: "decimal(18,2)");

			migrationBuilder.AlterColumn<Guid>(
				name: "OrderId",
				table: "OrderLines",
				type: "uniqueidentifier",
				nullable: true,
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier");

			migrationBuilder.AlterColumn<Guid>(
				name: "Id",
				table: "OrderLines",
				type: "uniqueidentifier",
				nullable: false,
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier",
				oldDefaultValueSql: "NEWID()");

			migrationBuilder.AddColumn<string>(
				name: "Price_Currency",
				table: "OrderLines",
				type: "nvarchar(max)",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Payments_OrderId1",
				table: "Payments",
				column: "OrderId1");

			migrationBuilder.AddForeignKey(
				name: "FK_OrderLines_Orders_OrderId",
				table: "OrderLines",
				column: "OrderId",
				principalTable: "Orders",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Payments_Orders_OrderId1",
				table: "Payments",
				column: "OrderId1",
				principalTable: "Orders",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_OrderLines_Orders_OrderId",
				table: "OrderLines");

			migrationBuilder.DropForeignKey(
				name: "FK_Payments_Orders_OrderId1",
				table: "Payments");

			migrationBuilder.DropIndex(
				name: "IX_Payments_OrderId1",
				table: "Payments");

			migrationBuilder.DropColumn(
				name: "OrderId1",
				table: "Payments");

			migrationBuilder.DropColumn(
				name: "PaidAmount_Currency",
				table: "Orders");

			migrationBuilder.DropColumn(
				name: "TotalAmount_Currency",
				table: "Orders");

			migrationBuilder.DropColumn(
				name: "Price_Currency",
				table: "OrderLines");

			migrationBuilder.AlterColumn<decimal>(
				name: "TotalAmount",
				table: "Orders",
				type: "decimal(18,2)",
				nullable: false,
				defaultValue: 0m,
				oldClrType: typeof(decimal),
				oldType: "decimal(18,2)",
				oldNullable: true);

			migrationBuilder.AlterColumn<decimal>(
				name: "PaidAmount",
				table: "Orders",
				type: "decimal(18,2)",
				nullable: false,
				defaultValue: 0m,
				oldClrType: typeof(decimal),
				oldType: "decimal(18,2)",
				oldNullable: true);

			migrationBuilder.AlterColumn<DateTime>(
				name: "OrderDate",
				table: "Orders",
				type: "datetime2",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
				oldClrType: typeof(DateTime),
				oldType: "datetime2",
				oldNullable: true);

			migrationBuilder.AlterColumn<Guid>(
				name: "ClientId",
				table: "Orders",
				type: "uniqueidentifier",
				nullable: false,
				defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier",
				oldNullable: true);

			migrationBuilder.AlterColumn<Guid>(
				name: "Id",
				table: "Orders",
				type: "uniqueidentifier",
				nullable: false,
				defaultValueSql: "NEWID()",
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier");

			migrationBuilder.AlterColumn<int>(
				name: "Qty",
				table: "OrderLines",
				type: "int",
				nullable: false,
				defaultValue: 0,
				oldClrType: typeof(int),
				oldType: "int",
				oldNullable: true);

			migrationBuilder.AlterColumn<Guid>(
				name: "ProductId",
				table: "OrderLines",
				type: "uniqueidentifier",
				nullable: false,
				defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier",
				oldNullable: true);

			migrationBuilder.AlterColumn<decimal>(
				name: "Price",
				table: "OrderLines",
				type: "decimal(18,2)",
				nullable: false,
				defaultValue: 0m,
				oldClrType: typeof(decimal),
				oldType: "decimal(18,2)",
				oldNullable: true);

			migrationBuilder.AlterColumn<Guid>(
				name: "OrderId",
				table: "OrderLines",
				type: "uniqueidentifier",
				nullable: false,
				defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier",
				oldNullable: true);

			migrationBuilder.AlterColumn<Guid>(
				name: "Id",
				table: "OrderLines",
				type: "uniqueidentifier",
				nullable: false,
				defaultValueSql: "NEWID()",
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier");

			migrationBuilder.CreateIndex(
				name: "IX_Payments_OrderId",
				table: "Payments",
				column: "OrderId");

			migrationBuilder.CreateIndex(
				name: "IX_OrderLines_ProductId",
				table: "OrderLines",
				column: "ProductId");

			migrationBuilder.AddForeignKey(
				name: "FK_OrderLines_Orders_OrderId",
				table: "OrderLines",
				column: "OrderId",
				principalTable: "Orders",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_OrderLines_Products_ProductId",
				table: "OrderLines",
				column: "ProductId",
				principalTable: "Products",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Payments_Orders_OrderId",
				table: "Payments",
				column: "OrderId",
				principalTable: "Orders",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}