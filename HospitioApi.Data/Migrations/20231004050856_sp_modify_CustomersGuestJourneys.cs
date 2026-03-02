using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class sp_modify_CustomersGuestJourneys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomersGuestJourneys] 
(
	@CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CutomerId],
           [JourneyStep],
           [Name],
           [SendType],
           [TimingOption1],
           [TimingOption2],
           [TimingOption3],
           [Timing],
           [NotificationDays],
           [NotificationTime],
           [GuestJourneyMessagesTemplateId],
           [TempletMessage],
           [IsActive],
           [Buttons],
           [VonageTemplateId],
	       [VonageTemplateStatus]
    FROM [dbo].[CustomerGuestJournies] WITH (NOLOCK)
    WHERE [DeletedAt] IS NULL
		  AND [VonageTemplateStatus] = 'APPROVED'
          AND [CutomerId] = @CustomerId
    ORDER BY [JourneyStep]
END");

            migrationBuilder.Sql(@"ALTER     PROC [dbo].[GetCustomersGuestJourneysById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON
    SELECT [Id],
           [CutomerId],
           [JourneyStep],
           [Name],
           [SendType],
           [TimingOption1],
           [TimingOption2],
           [TimingOption3],
           [Timing],
           [NotificationDays],
           [NotificationTime],
           [GuestJourneyMessagesTemplateId],
           [TempletMessage],
		   [Buttons],
           [VonageTemplateId],
           [VonageTemplateStatus]
    FROM [dbo].[CustomerGuestJournies] WITH (NOLOCK)
    WHERE [DeletedAt] IS NULL
		  AND [VonageTemplateStatus] = 'APPROVED'
          AND [Id] = @Id 
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
