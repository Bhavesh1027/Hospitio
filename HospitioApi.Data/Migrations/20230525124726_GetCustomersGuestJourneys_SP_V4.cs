using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomersGuestJourneys_SP_V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetCustomersGuestJourneys
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomersGuestJourneys]    Script Date: 25-05-2023 17:49:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create OR ALTER PROC [dbo].[GetCustomersGuestJourneys] --2
(	
	@CustomerId Int=1
)
AS BEGIN
	SET NOCOUNT ON;	
	 SELECT [Id]
      ,[CutomerId]
      ,[JourneyStep]
      ,[Name]
      ,[SendType]
      ,[TimingOption1]
      ,[TimingOption2]
      ,[TimingOption3]
      ,[Timing]
      ,[NotificationDays]
      ,[NotificationTime]
      ,[GuestJourneyMessagesTemplateId]
      ,[TempletMessage]
      ,[IsActive]	  
	  FROM CustomerGuestJournies WITH (NOLOCK)
      WHERE DeletedAt is null AND CutomerId = @CustomerId
	   ORDER BY  [JourneyStep]
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
