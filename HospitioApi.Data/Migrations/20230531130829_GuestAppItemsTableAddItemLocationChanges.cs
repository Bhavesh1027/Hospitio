using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GuestAppItemsTableAddItemLocationChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationLocation",
                table: "CustomerGuestAppRoomServiceItems");

            migrationBuilder.DropColumn(
                name: "DestinationLocation",
                table: "CustomerGuestAppReceptionItems");

            migrationBuilder.DropColumn(
                name: "DestinationLocation",
                table: "CustomerGuestAppHousekeepingItems");

            migrationBuilder.DropColumn(
                name: "DestinationLocation",
                table: "CustomerGuestAppConciergeItems");

            migrationBuilder.RenameColumn(
                name: "PickupLocation",
                table: "CustomerGuestAppRoomServiceItems",
                newName: "ItemLocation");

            migrationBuilder.RenameColumn(
                name: "PickupLocation",
                table: "CustomerGuestAppReceptionItems",
                newName: "ItemLocation");

            migrationBuilder.RenameColumn(
                name: "PickupLocation",
                table: "CustomerGuestAppHousekeepingItems",
                newName: "ItemLocation");

            migrationBuilder.RenameColumn(
                name: "PickupLocation",
                table: "CustomerGuestAppConciergeItems",
                newName: "ItemLocation");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CustomerGuestsCheckInFormFields",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FieldType",
                table: "CustomerGuestsCheckInFormFields",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "CustomerGuestsCheckInFormFields",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerPropertyInformationsSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerPropertyInformationId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPropertyInformationsSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPropertyInformationsSections_CustomerPropertyInformations_CustomerPropertyInformationId",
                        column: x => x.CustomerPropertyInformationId,
                        principalTable: "CustomerPropertyInformations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPropertyInformationsSections_CustomerPropertyInformationId",
                table: "CustomerPropertyInformationsSections",
                column: "CustomerPropertyInformationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPropertyInformationsSections");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "CustomerGuestsCheckInFormFields");

            migrationBuilder.RenameColumn(
                name: "ItemLocation",
                table: "CustomerGuestAppRoomServiceItems",
                newName: "PickupLocation");

            migrationBuilder.RenameColumn(
                name: "ItemLocation",
                table: "CustomerGuestAppReceptionItems",
                newName: "PickupLocation");

            migrationBuilder.RenameColumn(
                name: "ItemLocation",
                table: "CustomerGuestAppHousekeepingItems",
                newName: "PickupLocation");

            migrationBuilder.RenameColumn(
                name: "ItemLocation",
                table: "CustomerGuestAppConciergeItems",
                newName: "PickupLocation");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CustomerGuestsCheckInFormFields",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FieldType",
                table: "CustomerGuestsCheckInFormFields",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DestinationLocation",
                table: "CustomerGuestAppRoomServiceItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DestinationLocation",
                table: "CustomerGuestAppReceptionItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DestinationLocation",
                table: "CustomerGuestAppHousekeepingItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DestinationLocation",
                table: "CustomerGuestAppConciergeItems",
                type: "bit",
                nullable: true);
        }
    }
}
