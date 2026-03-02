using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetGuestWithEnhanceStayRequestsRelatedSP_V4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetGuestWithEnhanceStayRequests]    Script Date: 10-08-2023 15:51:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     PROCEDURE [dbo].[GetGuestWithEnhanceStayRequests] --  1,1
(
    @CustomerId INT = 0,
    @GuestId INT = 0
)
AS
BEGIN
    -- Fetch names from the foreign key tables for GuestRequests
    SELECT R.[Name], GR.[CreatedAt], GR.[Status]
	FROM [dbo].[CustomerGuestAppReceptionItems] R
	INNER JOIN [dbo].[GuestRequests] GR 
		ON R.[Id] = GR.[CustomerGuestAppReceptionItemId]
	WHERE GR.DeletedAt IS NULL AND GR.CustomerId = @CustomerId AND GR.GuestId = @GuestId

    UNION ALL
	SELECT H.[Name], GR.[CreatedAt], GR.[Status]
	FROM [dbo].[CustomerGuestAppHousekeepingItems] H
	INNER JOIN [dbo].[GuestRequests] GR 
		ON H.[Id] = GR.[CustomerGuestAppHousekeepingItemId]
	WHERE GR.DeletedAt IS NULL AND GR.CustomerId = @CustomerId AND GR.GuestId = @GuestId

    UNION ALL
    SELECT RS.[Name], GR.[CreatedAt], GR.[Status]
	FROM [dbo].[CustomerGuestAppRoomServiceItems] RS
	INNER JOIN [dbo].[GuestRequests] GR 
		ON RS.[Id] = GR.[CustomerGuestAppRoomServiceItemId]
	WHERE GR.DeletedAt IS NULL AND GR.CustomerId = @CustomerId AND GR.GuestId = @GuestId
   
	UNION ALL
    SELECT C.[Name], GR.[CreatedAt], GR.[Status]
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
    MAX(ES.CreatedAt) AS CreatedAt,
    (SELECT TOP 1 ES2.Status FROM [dbo].[EnhanceStayItemsGuestRequests] ES2 WHERE ES2.[CustomerGuestAppEnhanceYourStayItemId] = ES.[CustomerGuestAppEnhanceYourStayItemId] ORDER BY ES2.CreatedAt DESC) AS Status
	FROM [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras] EYSCIE
	JOIN [dbo].[EnhanceStayItemExtraGuestRequests] EYSGR
		ON EYSCIE.[Id] = EYSGR.[CustomerGuestAppEnhanceYourStayCategoryItemsExtraId]
	JOIN [dbo].[EnhanceStayItemsGuestRequests] ES
		ON ES.[Id] = EYSGR.[EnhanceStayItemsGuestRequestId]
	WHERE ES.[CustomerId] = @CustomerId
	AND ES.[GuestId] = @GuestId
	AND ES.[DeletedAt] IS NULL
	GROUP BY EYSCIE.Questions, ES.[CustomerGuestAppEnhanceYourStayItemId]
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
