using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetGuestRequests_v1_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER  PROCEDURE [dbo].[GetGuestRequests_v1]
(
    @CustomerId INT = 0,
    @SortColumn NVARCHAR(20) = 'TaskStatus',
    @SortOrder NVARCHAR(5) = 'ASC',
    @PageNo INT = 1,
    @PageSize INT = 10
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

    CREATE TABLE #TempTables
    (
        [Id] INT,
		[GuestId] INT,
        [GuestName] NVARCHAR(50),
        [GuestStatus] NVARCHAR(30),
        --[Room] NVARCHAR(50),
        [Department] INT,
        [TaskItem] NVARCHAR(100),
        [Charge] DECIMAL(18, 2),
        [TimeStamp] DATETIME,
        [TaskStatus] NVARCHAR(20),
        [Rating] INT,
		[GuestRequest] NVARCHAR(30)
    )

    INSERT INTO #TempTables
    (
        [Id],
		[GuestId],
        [GuestName],
        [GuestStatus],
       -- [Room],
        [Department],
        [TaskItem],
        [Charge],
        [TimeStamp],
        [TaskStatus],
        [Rating],
		[GuestRequest]
    )
    SELECT [GR].[Id],[GR].[GuestId],[CG].[Firstname] + ' ' + [CG].[Lastname] AS [GuestName],
           CASE
               WHEN
               (
                   CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE())
                   AND CONVERT(DATE, [CR].[CheckoutDate]) >= CONVERT(DATE, GETDATE())
               ) THEN
                   'In-House'
               WHEN
               (
                   CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE())
                   AND CONVERT(DATE, [CR].[CheckoutDate]) < CONVERT(DATE, GETDATE())
               ) THEN
                   'Checked-out'
           END AS [GuestsStatus],
          --[CRN].[Name] as [Room],
           [GR].[RequestType] AS [Department],
           CASE
               WHEN [GR].[RequestType] = 1 THEN [EYS].[ShortDescription]
               WHEN [GR].[RequestType] = 2 THEN [RC].[Name]
               WHEN [GR].[RequestType] = 3 THEN [HK].[Name]
               WHEN [GR].[RequestType] = 4 THEN [RS].[Name]
               WHEN [GR].[RequestType] = 5 THEN [CC].[Name]
               ELSE  ''
           END AS [TaskItem],
           CASE
               WHEN [GR].[RequestType] = 1 THEN [EYS].[Price]
               WHEN [GR].[RequestType] = 2 THEN [RC].[Price]
               WHEN [GR].[RequestType] = 3 THEN [HK].[Price]
               WHEN [GR].[RequestType] = 4 THEN [RS].[Price]
               WHEN [GR].[RequestType] = 5 THEN [CC].[Price]
               ELSE  0.00
           END AS [Charge],
           [GR].[CreatedAt] AS [TimeStamp],
           [GR].[Status] AS [TaskStatus],
           [CG].[Rating],
		   'Guest' AS [GuestRequest]
    FROM [dbo].[GuestRequests] GR (NOLOCK)
        INNER JOIN [dbo].[CustomerGuests] CG WITH (NOLOCK)
            ON [CG].[Id] = [GR].[GuestId]
               AND [CG].[DeletedAt] IS NULL
        --INNER JOIN [dbo].[CustomerRoomNames] CRN WITH (NOLOCK)
        --    ON [CRN].[CustomerId] = [GR].[CustomerId]
        --       AND [CRN].[DeletedAt] IS NULL
        INNER JOIN [dbo].[CustomerReservations] (NOLOCK) CR
            ON [CR].[Id] = [CG].[CustomerReservationId]
               AND [CR].[DeletedAt] IS NULL
        LEFT JOIN [dbo].[CustomerGuestAppEnhanceYourStayItems] EYS WITH (NOLOCK)
            ON [EYS].[Id] = [GR].[CustomerGuestAppEnhanceYourStayItemId]
               AND [EYS].[DeletedAt] IS NULL
        LEFT JOIN [dbo].[CustomerGuestAppHousekeepingItems] HK WITH (NOLOCK)
            ON [HK].[Id] = [GR].[CustomerGuestAppHousekeepingItemId]
               AND [HK].[DeletedAt] IS NULL
        LEFT JOIN [dbo].[CustomerGuestAppConciergeItems] CC WITH (NOLOCK)
            ON [CC].[Id] = [GR].[CustomerGuestAppConciergeItemId]
               AND [CC].[DeletedAt] IS NULL
        LEFT JOIN [dbo].[CustomerGuestAppReceptionItems] RC WITH (NOLOCK)
            ON [RC].[Id] = [GR].[CustomerGuestAppReceptionItemId]
               AND [RC].[DeletedAt] IS NULL
        LEFT JOIN [dbo].[CustomerGuestAppRoomServiceItems] RS WITH (NOLOCK)
            ON [RS].[Id] = [GR].[CustomerGuestAppRoomServiceItemId]
               AND [RS].[DeletedAt] IS NULL
    WHERE [GR].[CustomerId] = @CustomerId
          AND [GR].[DeletedAt] IS NULL

	UNION 
	
	SELECT [ESIGR].[Id],[ESIGR].[GuestId],[CG].[Firstname] + ' ' + [CG].[Lastname] AS [GuestName],
		   CASE WHEN  (
						CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE())
						AND CONVERT(DATE, [CR].[CheckoutDate]) >= CONVERT(DATE, GETDATE())
					  ) 
				THEN  'In-House'
				WHEN  (
						CONVERT(DATE, [CR].[CheckinDate]) <= CONVERT(DATE, GETDATE())
						AND CONVERT(DATE, [CR].[CheckoutDate]) < CONVERT(DATE, GETDATE())
					  ) 
				THEN  'Checked-out' END AS [GuestsStatus],
				1 AS [Department],
				[CGP].[ShortDescription] AS [TaskItem],
				[CGP].[Price] AS [Charge],
				[ESIGR].[CreatedAt] AS [TimeStamp],	
				[ESIGR].[Status] AS [TaskStatus],
				[CG].[Rating],
				'EnhanceStayGuestRequest' AS GuestRequest
		FROM EnhanceStayItemsGuestRequests ESIGR (NOLOCK)
			 INNER JOIN [dbo].[CustomerGuests] CG (NOLOCK)  
				ON [CG].[Id] = [ESIGR].[GuestId] 
					AND [CG].[DeletedAt] IS NULL
			 INNER JOIN [dbo].[CustomerReservations] (NOLOCK) CR 
				ON [CR].[Id] = [CG].[CustomerReservationId] 
					AND [CR].[DeletedAt] IS NULL
			 INNER JOIN CustomerGuestAppEnhanceYourStayItems CGP (NOLOCK) 
				ON [CGP].[Id] = [ESIGR].[CustomerGuestAppEnhanceYourStayItemId] 
					AND [CGP].[DeletedAt] IS NULL
		WHERE [ESIGR].[CustomerId] = @CustomerId 
			  AND [ESIGR].[DeletedAt] IS NULL
		 
    SELECT [Id],[GuestId],[GuestName],[GuestStatus],
         --  [Room],
           [Department],[TaskItem],[Charge],[TimeStamp],[TaskStatus],[Rating],[GuestRequest],COUNT(*) OVER () AS [TotalCount]
    FROM #TempTables
    ORDER BY CASE
                 WHEN
                 (
                     @SortColumn = 'TaskStatus'
                     AND @SortOrder = 'ASC'
                 ) THEN
                     [TaskStatus]
             END ASC,
             CASE
                 WHEN
                 (
                     @SortColumn = 'TaskStatus'
                     AND @SortOrder = 'DESC'
                 ) THEN
                     [TaskStatus]
             END DESC,
             --CASE
             --    WHEN
             --    (
             --        @SortColumn = 'Room'
             --        AND @SortOrder = 'ASC'
             --    ) THEN
             --        [Room]
             --END ASC,
             --CASE
             --    WHEN
             --    (
             --        @SortColumn = 'Room'
             --        AND @SortOrder = 'DESC'
             --    ) THEN
             --        [Room]
             --END DESC,
             CASE
                 WHEN
                 (
                     @SortColumn = 'Department'
                     AND @SortOrder = 'ASC'
                 ) THEN
                     [Department]
             END ASC,
             CASE
                 WHEN
                 (
                     @SortColumn = 'Department'
                     AND @SortOrder = 'DESC'
                 ) THEN
                     [Department]
             END DESC,
			 CASE
                 WHEN
                 (
                     @SortColumn = 'TaskItem'
                     AND @SortOrder = 'ASC'
                 ) THEN
                     [Department]
             END ASC,
             CASE
                 WHEN
                 (
                     @SortColumn = 'TaskItem'
                     AND @SortOrder = 'DESC'
                 ) THEN
                     [Department]
             END DESC,
             CASE
                 WHEN
                 (
                     @SortColumn = 'TimeStamp'
                     AND @SortOrder = 'ASC'
                 ) THEN
                     [TimeStamp]
             END ASC,
             CASE
                 WHEN
                 (
                     @SortColumn = 'TimeStamp'
                     AND @SortOrder = 'DESC'
                 ) THEN
                     [TimeStamp]
             END DESC,
             CASE
                 WHEN
                 (
                     @SortColumn = 'GuestStatus'
                     AND @SortOrder = 'ASC'
                 ) THEN
                     [GuestStatus]
             END ASC,
             CASE
                 WHEN
                 (
                     @SortColumn = 'GuestStatus'
                     AND @SortOrder = 'DESC'
                 ) THEN
                     [GuestStatus]
             END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
