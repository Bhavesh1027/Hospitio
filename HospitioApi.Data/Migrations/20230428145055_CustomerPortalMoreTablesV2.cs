using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class CustomerPortalMoreTablesV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerGuestJournies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CutomerId = table.Column<int>(type: "int", nullable: false),
                    JourneyStep = table.Column<byte>(type: "tinyint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SendType = table.Column<byte>(type: "tinyint", nullable: true),
                    TimingOption1 = table.Column<byte>(type: "tinyint", nullable: true),
                    TimingOption2 = table.Column<byte>(type: "tinyint", nullable: true),
                    TimingOption3 = table.Column<byte>(type: "tinyint", nullable: true),
                    Timing = table.Column<int>(type: "int", nullable: true),
                    NotificationDays = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NotificationTime = table.Column<TimeSpan>(type: "time(0)", nullable: true),
                    GuestJourneyMessagesTemplateId = table.Column<int>(type: "int", nullable: true),
                    TempletMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGuestJournies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestJournies_Customers_CutomerId",
                        column: x => x.CutomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerGuestJournies_GuestJourneyMessagesTemplates_GuestJourneyMessagesTemplateId",
                        column: x => x.GuestJourneyMessagesTemplateId,
                        principalTable: "GuestJourneyMessagesTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerGuestsCheckInFormBuilders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Stars = table.Column<byte>(type: "tinyint", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AppImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SplashScreen = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsOnlineCheckInFormEnable = table.Column<bool>(type: "bit", nullable: true),
                    IsRedirectToGuestAppEnable = table.Column<bool>(type: "bit", nullable: true),
                    SubmissionMail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TermsLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGuestsCheckInFormBuilders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuestsCheckInFormBuilders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    Uuid = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    ReservationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Source = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NoOfGuestAdults = table.Column<int>(type: "int", nullable: true),
                    NoOfGuestChildrens = table.Column<int>(type: "int", nullable: true),
                    CheckinDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CheckoutDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerReservations_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerGuests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerReservationId = table.Column<int>(type: "int", nullable: true),
                    Firstname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhoneCountry = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IdProof = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdProofType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdProofNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BlePinCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Pin = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StreetNumber = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Postalcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ArrivalFlightNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DepartureAirline = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DepartureFlightNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TermsAccepted = table.Column<bool>(type: "bit", nullable: true),
                    FirstJourneyStep = table.Column<byte>(type: "tinyint", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerGuests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerGuests_CustomerReservations_CustomerReservationId",
                        column: x => x.CustomerReservationId,
                        principalTable: "CustomerReservations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestJournies_CutomerId",
                table: "CustomerGuestJournies",
                column: "CutomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestJournies_GuestJourneyMessagesTemplateId",
                table: "CustomerGuestJournies",
                column: "GuestJourneyMessagesTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuests_CustomerReservationId",
                table: "CustomerGuests",
                column: "CustomerReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerGuestsCheckInFormBuilders_CustomerId",
                table: "CustomerGuestsCheckInFormBuilders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReservations_CustomerId",
                table: "CustomerReservations",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerGuestJournies");

            migrationBuilder.DropTable(
                name: "CustomerGuests");

            migrationBuilder.DropTable(
                name: "CustomerGuestsCheckInFormBuilders");

            migrationBuilder.DropTable(
                name: "CustomerReservations");
        }
    }
}
