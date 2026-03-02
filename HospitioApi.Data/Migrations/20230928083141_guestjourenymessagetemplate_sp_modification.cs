using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class guestjourenymessagetemplate_sp_modification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGuestJourneyMessagesTemplates]    Script Date: 28-09-2023 14:01:59 ******/
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
		   [Buttons],
		   [VonageTemplateId],
		   [VonageTemplateStatus]
    FROM [dbo].[GuestJourneyMessagesTemplates] (NOLOCK)
    WHERE [DeletedAt] IS NULL
END");

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGuestJourneyMessagesTemplatesById]    Script Date: 28-09-2023 14:02:49 ******/
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
		   [VonageTemplateId],
		   [VonageTemplateStatus],
           [IsActive]
    FROM [dbo].[GuestJourneyMessagesTemplates] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
