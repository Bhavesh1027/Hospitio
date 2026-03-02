using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddedNewColumnToCustomerRoomAndCustomerTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Customers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "(newid())");

            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "CustomerRoomNames",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "(newid())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "CustomerRoomNames");
        }
    }
}
