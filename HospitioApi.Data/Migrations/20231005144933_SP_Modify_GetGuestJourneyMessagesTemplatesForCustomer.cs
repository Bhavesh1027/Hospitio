using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_Modify_GetGuestJourneyMessagesTemplatesForCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"/****** Object:  StoredProcedure [dbo].[GetGuestJourneyMessagesTemplatesForCustomer]    Script Date: 05-10-2023 20:17:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER       PROCEDURE [dbo].[GetGuestJourneyMessagesTemplatesForCustomer]
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
		  AND VonageTemplateStatus = 'APPROVED'
          AND [IsActive] = 1
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
