using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CustomerPortalMoreTablesV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerGuestAppBuilders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerRoomNameId = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalExperience = table.Column<bool>(type: "bit", nullable: true),
                    Ekey = table.Column<bool>(type: "bit", nullable: true),
                    PropertyInfo = table.Column<bool>(type: "bit", nullable: true),
                    EnhanceYourStay = table.Column<bool>(type: "bit", nullable: true),
                    Reception = table.Column<bool>(type: "bit", nullable: true),
                    Housekeeping = table.Column<bool>(type: "bit", nullable: true),
                    RoomService = table.Column<bool>(type: "bit", nullable: true),
                    Concierge = table.Column<bool>(type: "bit", nullable: true),
                    TransferServices = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGuestAppBuilders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppBuilders_CustomerRoomNames_CustomerRoomNameId",
                        column: x => x.CustomerRoomNameId,
                        principalTable: "CustomerRoomNames",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestAppBuilders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerGuestsCheckInFormFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestsCheckInFormBuilderId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredFields = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGuestsCheckInFormFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestsCheckInFormFields_CustomerGuestsCheckInFormBuilders_CustomerGuestsCheckInFormBuilderId",
                        column: x => x.CustomerGuestsCheckInFormBuilderId,
                        principalTable: "CustomerGuestsCheckInFormBuilders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerGuestsCheckInFormFields_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerPropertyInformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerGuestAppBuilderId = table.Column<int>(type: "int", nullable: true),
                    WifiUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WifiPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Overview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckInPolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Postalcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPropertyInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPropertyInformations_CustomerGuestAppBuilders_CustomerGuestAppBuilderId",
                        column: x => x.CustomerGuestAppBuilderId,
                        principalTable: "CustomerGuestAppBuilders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerPropertyInformations_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerPropertyGalleries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerPropertyInformationId = table.Column<int>(type: "int", nullable: true),
                    PropertyImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPropertyGalleries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPropertyGalleries_CustomerPropertyInformations_CustomerPropertyInformationId",
                        column: x => x.CustomerPropertyInformationId,
                        principalTable: "CustomerPropertyInformations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppBuilders_CustomerId",
                table: "CustomerGuestAppBuilders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestAppBuilders_CustomerRoomNameId",
                table: "CustomerGuestAppBuilders",
                column: "CustomerRoomNameId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestsCheckInFormFields_CustomerGuestsCheckInFormBuilderId",
                table: "CustomerGuestsCheckInFormFields",
                column: "CustomerGuestsCheckInFormBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestsCheckInFormFields_CustomerId",
                table: "CustomerGuestsCheckInFormFields",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPropertyGalleries_CustomerPropertyInformationId",
                table: "CustomerPropertyGalleries",
                column: "CustomerPropertyInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPropertyInformations_CustomerGuestAppBuilderId",
                table: "CustomerPropertyInformations",
                column: "CustomerGuestAppBuilderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPropertyInformations_CustomerId",
                table: "CustomerPropertyInformations",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerGuestsCheckInFormFields");

            migrationBuilder.DropTable(
                name: "CustomerPropertyGalleries");

            migrationBuilder.DropTable(
                name: "CustomerPropertyInformations");

            migrationBuilder.DropTable(
                name: "CustomerGuestAppBuilders");

        }
    }
}
