using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class removeexpirationindaytostoreexpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionExpirationInDay",
                table: "Customers");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionExpirationDate",
                table: "Customers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionExpirationDate",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionExpirationInDay",
                table: "Customers",
                type: "int",
                nullable: true);
        }
    }
}
