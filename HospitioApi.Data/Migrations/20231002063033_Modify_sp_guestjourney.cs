using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_sp_guestjourney : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetGuestJourneyMessagesTemplatesForCustomer
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGuestJourneyMessagesTemplatesForCustomer]    Script Date: 10/02/2023 11:25:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE OR ALTER     PROCEDURE [dbo].[GetGuestJourneyMessagesTemplatesForCustomer]
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [TempleteType],
           [Name],
           [TempletMessage],
		   [Buttons],
		   [VonageTemplateId],
		   [VonageTemplateStatus],
           [IsActive]
    FROM [dbo].[GuestJourneyMessagesTemplates] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [IsActive] = 1
END");
            #endregion

            #region GetCustomersGuestJourneys
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomersGuestJourneys]    Script Date: 10/02/2023 11:35:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetCustomersGuestJourneys] 
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
            #endregion

            #region GetCustomersGuestJourneysById

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomersGuestJourneysById]    Script Date: 10/02/2023 11:44:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetCustomersGuestJourneysById] 
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

            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
