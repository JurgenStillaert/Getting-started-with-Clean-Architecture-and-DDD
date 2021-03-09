using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace buyyu.Data.Migrations
{
	public partial class AfterDDD4 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "QtyInStock",
				table: "Products");

			migrationBuilder.AlterColumn<decimal>(
				name: "Price",
				table: "Products",
				type: "decimal(18,2)",
				nullable: true,
				oldClrType: typeof(decimal),
				oldType: "decimal(18,2)");

			migrationBuilder.AlterColumn<Guid>(
				name: "Id",
				table: "Products",
				type: "uniqueidentifier",
				nullable: false,
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier",
				oldDefaultValueSql: "NEWID()");

			migrationBuilder.AddColumn<string>(
				name: "Price_Currency",
				table: "Products",
				type: "nvarchar(max)",
				nullable: true);

			migrationBuilder.CreateTable(
				name: "Warehouses",
				columns: table => new
				{
					ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					QtyInStock = table.Column<int>(type: "int", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Warehouses", x => x.ProductId);
				});

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: new Guid("32f75bce-16a0-4070-9fac-4289678c191f"),
				column: "Price_Currency",
				value: "EUR");

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: new Guid("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
				column: "Price_Currency",
				value: "EUR");

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: new Guid("bcbc1851-6317-4022-be62-53d29c04bcda"),
				column: "Price_Currency",
				value: "EUR");

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: new Guid("de679c55-4c13-4fe7-91b4-69cbce3223a2"),
				column: "Price_Currency",
				value: "EUR");

			migrationBuilder.InsertData(
				table: "Warehouses",
				columns: new[] { "ProductId", "QtyInStock" },
				values: new object[,]
				{
					{ new Guid("de679c55-4c13-4fe7-91b4-69cbce3223a2"), 213 },
					{ new Guid("32f75bce-16a0-4070-9fac-4289678c191f"), 75 },
					{ new Guid("bcbc1851-6317-4022-be62-53d29c04bcda"), 179 },
					{ new Guid("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"), 150 }
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Warehouses");

			migrationBuilder.DropColumn(
				name: "Price_Currency",
				table: "Products");

			migrationBuilder.AlterColumn<decimal>(
				name: "Price",
				table: "Products",
				type: "decimal(18,2)",
				nullable: false,
				defaultValue: 0m,
				oldClrType: typeof(decimal),
				oldType: "decimal(18,2)",
				oldNullable: true);

			migrationBuilder.AlterColumn<Guid>(
				name: "Id",
				table: "Products",
				type: "uniqueidentifier",
				nullable: false,
				defaultValueSql: "NEWID()",
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier");

			migrationBuilder.AddColumn<int>(
				name: "QtyInStock",
				table: "Products",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: new Guid("32f75bce-16a0-4070-9fac-4289678c191f"),
				column: "QtyInStock",
				value: 75);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: new Guid("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"),
				column: "QtyInStock",
				value: 150);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: new Guid("bcbc1851-6317-4022-be62-53d29c04bcda"),
				column: "QtyInStock",
				value: 179);

			migrationBuilder.UpdateData(
				table: "Products",
				keyColumn: "Id",
				keyValue: new Guid("de679c55-4c13-4fe7-91b4-69cbce3223a2"),
				column: "QtyInStock",
				value: 213);
		}
	}
}