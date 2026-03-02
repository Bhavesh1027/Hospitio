using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class AddSPForGuestJourneys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"/****** Object:  StoredProcedure [dbo].[GetCustomersGuestJourneys]    Script Date: 18-10-2023 17:51:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROC [dbo].[GetCustomersGuestJourneys] 
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
          AND [CutomerId] = @CustomerId
    ORDER BY [JourneyStep]
END");

            migrationBuilder.Sql(@"/****** Object:  StoredProcedure [dbo].[GetCustomersGuestJourneysById]    Script Date: 18-10-2023 17:52:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER     PROC [dbo].[GetCustomersGuestJourneysById] 
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
          AND [Id] = @Id 
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
