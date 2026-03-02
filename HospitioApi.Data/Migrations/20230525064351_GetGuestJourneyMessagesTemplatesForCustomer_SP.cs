using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetGuestJourneyMessagesTemplatesForCustomer_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetGuestJourneyMessagesTemplatesForCustomer
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGuestJourneyMessagesTemplates]    Script Date: 25-05-2023 12:02:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create OR ALTER Procedure [dbo].[GetGuestJourneyMessagesTemplatesForCustomer]

as
select * from GuestJourneyMessagesTemplates
where DeletedAt is null
And IsActive = 1");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
