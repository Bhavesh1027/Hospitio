using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetGuestWithEnhanceStayRequestsRelatedSP_V5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetGuestWithEnhanceStayRequests]    Script Date: 05-09-2023 10:34:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER       PROCEDURE [dbo].[GetGuestWithEnhanceStayRequests] -- 1,1
(
    @CustomerId INT = 0,
    @GuestId INT = 0
)
AS
BEGIN
    -- Fetch names from the foreign key tables for GuestRequests
    SELECT R.[Name], GR.[CreatedAt], GR.[Status], GR.[RequestType]
	FROM [dbo].[CustomerGuestAppReceptionItems] R
	INNER JOIN [dbo].[GuestRequests] GR 
		ON R.[Id] = GR.[CustomerGuestAppReceptionItemId]
	WHERE GR.DeletedAt IS NULL AND GR.CustomerId = @CustomerId AND GR.GuestId = @GuestId

    UNION ALL
	SELECT H.[Name], GR.[CreatedAt], GR.[Status], GR.[RequestType]
	FROM [dbo].[CustomerGuestAppHousekeepingItems] H
	INNER JOIN [dbo].[GuestRequests] GR 
		ON H.[Id] = GR.[CustomerGuestAppHousekeepingItemId]
	WHERE GR.DeletedAt IS NULL AND GR.CustomerId = @CustomerId AND GR.GuestId = @GuestId

    UNION ALL
    SELECT RS.[Name], GR.[CreatedAt], GR.[Status], GR.[RequestType]
	FROM [dbo].[CustomerGuestAppRoomServiceItems] RS
	INNER JOIN [dbo].[GuestRequests] GR 
		ON RS.[Id] = GR.[CustomerGuestAppRoomServiceItemId]
	WHERE GR.DeletedAt IS NULL AND GR.CustomerId = @CustomerId AND GR.GuestId = @GuestId
   
	UNION ALL
    SELECT C.[Name], GR.[CreatedAt], GR.[Status], GR.[RequestType]
	FROM [dbo].[CustomerGuestAppConciergeItems] C
	INNER JOIN [dbo].[GuestRequests] GR 
		ON C.[Id] = GR.[CustomerGuestAppConciergeItemId]
	WHERE GR.DeletedAt IS NULL AND GR.CustomerId = @CustomerId AND GR.GuestId = @GuestId

    UNION ALL

		-- Fetch names from the questions in EnhanceStayItemsGuestRequests
	SELECT
    REPLACE(REPLACE(CI.[ShortDescription], '<p>', ''), '</p>', '') AS ShortDescription,
    ER.[CreatedAt],
    ER.[Status],
	1 AS RequestType
FROM
    EnhanceStayItemsGuestRequests ER
INNER JOIN
    CustomerGuestAppEnhanceYourStayItems CI ON ER.[CustomerGuestAppEnhanceYourStayItemId] = CI.[Id]
WHERE
    ER.guestid = @GuestId
    AND ER.customerid = @CustomerId
	AND ER.[DeletedAt] IS NULL
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
