using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class guestjourneyspmodification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomersGuestJourneys
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomersGuestJourneys]    Script Date: 27-09-2023 17:15:15 ******/
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
		   [Buttons]
    FROM [dbo].[CustomerGuestJournies] WITH (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [CutomerId] = @CustomerId
    ORDER BY [JourneyStep]
END");
            #endregion

            #region GetCustomersGuestJourneysById
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomersGuestJourneysById]    Script Date: 27-09-2023 17:16:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROC [dbo].[GetCustomersGuestJourneysById] 
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
		   [Buttons]
    FROM [dbo].[CustomerGuestJournies] WITH (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id 
END
");
            #endregion

            #region GetGuestJourneyMessagesTemplates
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGuestJourneyMessagesTemplates]    Script Date: 27-09-2023 17:18:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[GetGuestJourneyMessagesTemplates]
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [TempleteType],
           [Name],
           [TempletMessage],
           [IsActive],
		   [Buttons]
    FROM [dbo].[GuestJourneyMessagesTemplates] (NOLOCK)
    WHERE [DeletedAt] IS NULL
END");
            #endregion

            #region GetGuestJourneyMessagesTemplatesById
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGuestJourneyMessagesTemplatesById]    Script Date: 27-09-2023 17:18:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[GetGuestJourneyMessagesTemplatesById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [TempleteType],
           [Name],
           [TempletMessage],
		   [Buttons],
           [IsActive]
    FROM [dbo].[GuestJourneyMessagesTemplates] (NOLOCK)
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
