﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharedServices.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ECommerceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECommerceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECommerceProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShopFavorites = table.Column<bool>(type: "bit", nullable: false),
                    CustomerFavorites = table.Column<bool>(type: "bit", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECommerceProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ECommerceProducts_ECommerceCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ECommerceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ECommerceProductPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    MyProperty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECommerceProductPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ECommerceProductPrices_ECommerceProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ECommerceProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ECommerceProductPrices_ProductId",
                table: "ECommerceProductPrices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ECommerceProducts_CategoryId",
                table: "ECommerceProducts",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ECommerceProductPrices");

            migrationBuilder.DropTable(
                name: "ECommerceProducts");

            migrationBuilder.DropTable(
                name: "ECommerceCategories");
        }
    }
}
