using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace buyyu.Data.Migrations
{
	public partial class Initial : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "OrderStates",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ShortCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_OrderStates", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Products",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
					QtyInStock = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Products", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Orders",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
					ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					OrderStateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
					PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Orders", x => x.Id);
					table.ForeignKey(
						name: "FK_Orders_OrderStates_OrderStateId",
						column: x => x.OrderStateId,
						principalTable: "OrderStates",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "OrderLines",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
					OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
					Qty = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_OrderLines", x => x.Id);
					table.ForeignKey(
						name: "FK_OrderLines_Orders_OrderId",
						column: x => x.OrderId,
						principalTable: "Orders",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_OrderLines_Products_ProductId",
						column: x => x.ProductId,
						principalTable: "Products",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Payments",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
					PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
					PayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Payments", x => x.Id);
					table.ForeignKey(
						name: "FK_Payments_Orders_OrderId",
						column: x => x.OrderId,
						principalTable: "Orders",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.InsertData(
				table: "OrderStates",
				columns: new[] { "Id", "Name", "ShortCode" },
				values: new object[,]
				{
					{ new Guid("bd8be3d2-8028-45e2-a211-bf737a2508c1"), "Initiated", "NEW" },
					{ new Guid("82d9ce01-9f25-48b1-8af3-93f52426676f"), "Confirmed", "CNF" },
					{ new Guid("4b5549bb-b1b2-4964-9818-da984baab4ff"), "Shipped", "SHP" }
				});

			migrationBuilder.InsertData(
				table: "Products",
				columns: new[] { "Id", "Description", "Name", "Price", "QtyInStock" },
				values: new object[,]
				{
					{ new Guid("de679c55-4c13-4fe7-91b4-69cbce3223a2"), "Implement an ergonomic seating solution for your office with this maroon multipurpose chair. The included tilt tension knob lets you calibrate the tilt and recline resistance to your desired configuration, while the adjustable seat and armrests optimize your seating position for correct posture.", "Office Chair Beta", 169m, 213 },
					{ new Guid("32f75bce-16a0-4070-9fac-4289678c191f"), "The Lockland Big & Tall bonded leather managers chair offers top quality comfort, multiple adjustment features.", "Office Chair Manager", 263m, 75 },
					{ new Guid("bcbc1851-6317-4022-be62-53d29c04bcda"), "Carve out a personal workspace with this storage desk. The simple design and classic mid-century modern details make this desk perfect for modern decor themes or casual open office settings, and the rectangular desktop provides space for a laptop and peripherals.", "Vintage Desk", 305m, 179 },
					{ new Guid("5ca659b1-25b1-45c1-9755-3a3cd8591b9e"), "The Techni Mobili Complete Workstation Desk is everything you need in a computer desk and stay organized.", "Desk Techni", 295m, 150 }
				});

			migrationBuilder.CreateIndex(
				name: "IX_OrderLines_OrderId",
				table: "OrderLines",
				column: "OrderId");

			migrationBuilder.CreateIndex(
				name: "IX_OrderLines_ProductId",
				table: "OrderLines",
				column: "ProductId");

			migrationBuilder.CreateIndex(
				name: "IX_Orders_OrderStateId",
				table: "Orders",
				column: "OrderStateId");

			migrationBuilder.CreateIndex(
				name: "IX_Payments_OrderId",
				table: "Payments",
				column: "OrderId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "OrderLines");

			migrationBuilder.DropTable(
				name: "Payments");

			migrationBuilder.DropTable(
				name: "Products");

			migrationBuilder.DropTable(
				name: "Orders");

			migrationBuilder.DropTable(
				name: "OrderStates");
		}
	}
}