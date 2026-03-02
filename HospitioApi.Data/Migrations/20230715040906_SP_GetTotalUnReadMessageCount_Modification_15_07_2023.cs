using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_GetTotalUnReadMessageCount_Modification_15_07_2023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[SP_GetTotalUnReadMessageCount]
(
	 @UserId INT = 0
)
AS
BEGIN
	
	SET NOCOUNT ON
    SET XACT_ABORT ON
	
	SELECT COUNT([C].[Id]) AS [TotalUnreadCount]
	FROM [dbo].[Channels] C (NOLOCK)
		INNER JOIN [dbo].[ChannelUsers] CU 
			ON [C].[Id] = [CU].[ChannelId] 
				AND [CU].[UserId] = @UserId
		INNER JOIN [dbo].[ChannelMessages] CM  
			ON  [CM].[ChannelId] = [C].[Id] 
				AND [CM].[MessageSenderId] <> @UserId 
				AND [CM].[Id] > ISNULL([CU].[LastMessageReadId],0) 

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
