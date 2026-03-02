using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetGuestWithEnhanceStayRequestsRelatedSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[GetGuestWithEnhanceStayRequests] --  1,1
(
    @CustomerId INT = 0,
    @GuestId INT = 0
)
AS
BEGIN
    -- Fetch names from the foreign key tables for GuestRequests
    SELECT R.[Name], GR.[CreatedAt]
	FROM [dbo].[CustomerGuestAppReceptionItems] R
	INNER JOIN [dbo].[GuestRequests] GR 
		ON R.[Id] = GR.[CustomerGuestAppReceptionItemId]
	WHERE GR.DeletedAt IS NULL AND GR.CustomerId = @CustomerId AND GR.GuestId = @GuestId

    UNION ALL
	SELECT H.[Name], GR.[CreatedAt]
	FROM [dbo].[CustomerGuestAppHousekeepingItems] H
	INNER JOIN [dbo].[GuestRequests] GR 
		ON H.[Id] = GR.[CustomerGuestAppHousekeepingItemId]
	WHERE GR.DeletedAt IS NULL AND GR.CustomerId = @CustomerId AND GR.GuestId = @GuestId

    UNION ALL
    SELECT RS.[Name], GR.[CreatedAt]
	FROM [dbo].[CustomerGuestAppRoomServiceItems] RS
	INNER JOIN [dbo].[GuestRequests] GR 
		ON RS.[Id] = GR.[CustomerGuestAppRoomServiceItemId]
	WHERE GR.DeletedAt IS NULL AND GR.CustomerId = @CustomerId AND GR.GuestId = @GuestId
   
	UNION ALL
    SELECT C.[Name], GR.[CreatedAt]
	FROM [dbo].[CustomerGuestAppConciergeItems] C
	INNER JOIN [dbo].[GuestRequests] GR 
		ON C.[Id] = GR.[CustomerGuestAppConciergeItemId]
	WHERE GR.DeletedAt IS NULL AND GR.CustomerId = @CustomerId AND GR.GuestId = @GuestId

    UNION ALL

		-- Fetch names from the questions in EnhanceStayItemsGuestRequests
	SELECT 
		CASE 
			WHEN ISJSON(EYSCIE.Questions) = 1 THEN JSON_VALUE(EYSCIE.Questions, '$.question')
			ELSE EYSCIE.Questions
		END AS Name,
		(SELECT TOP 1 ES.CreatedAt
		FROM [dbo].[EnhanceStayItemsGuestRequests] ES
		WHERE ES.[CustomerId] = @CustomerId AND ES.[GuestId] = @GuestId AND ES.[DeletedAt] is null
		) AS CreatedAt
	FROM [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras] EYSCIE
	WHERE EYSCIE.[Id] IN (
		SELECT EYSGR.[CustomerGuestAppEnhanceYourStayCategoryItemsExtraId]
		FROM [dbo].[EnhanceStayItemsGuestRequests] ES
		LEFT JOIN [dbo].[EnhanceStayItemExtraGuestRequests] EYSGR
			ON ES.[Id] = EYSGR.[EnhanceStayItemsGuestRequestId]
		WHERE ES.[CustomerId] = @CustomerId AND ES.[GuestId] = @GuestId AND ES.[DeletedAt] is null
	)
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
