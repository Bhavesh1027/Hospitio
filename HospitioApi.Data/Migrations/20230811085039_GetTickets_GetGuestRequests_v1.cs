using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetTickets_GetGuestRequests_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetTickets]
(
    @Status INT = 0,
    @Priority INT = 0,
    @CustomerId INT = 0,
    @CSAgentId INT = 0,
    @FromCreate DATETIME = NULL,
    @ToCreate DATETIME = NULL,
    @FromClose DATETIME = NULL,
    @ToClose DATETIME = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10,
    @ShortBy TINYINT = 0,     -- on CreateAt 1=Short by Date,2=Short By Month,3=Short By Year (modify in feture if need)
    @CreatedFrom TINYINT = 0, -- 1 Fro Radfy ,2 for Customer,
    @ApplyPagination BIT = 0
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

    DECLARE @query NVARCHAR(MAX) = ''
	
	SET @query += ' SELECT [Ticket].[Id],[Ticket].[CustomerId],[dbo].[Customers].[BusinessName],[dbo].[Customers].[Cname] AS [CustomerName],[dbo].[Customers].[Email],[Ticket].[Title],[Ticket].[Details],[Ticket].[Priority],[Ticket].[Duedate],[Ticket].[Status],[Ticket].[CloseDate], [Ticket].[CreatedFrom],CASE WHEN [Ticket].[CreatedFrom] = 2 AND [Ticket].[CSAgentId] IS NULL THEN NULL ELSE CONCAT(ISNULL([dbo].[Users].[FirstName],''''),SPACE(1),ISNULL([dbo].[Users].[LastName],'''')) END AS [CSAgentName],[Ticket].[IsActive],[Ticket].[CreatedAt],[dbo].[CustomerGuestsCheckInFormBuilders].[Logo] AS [ProfilePicture],COUNT(*) OVER () AS [FilteredCount],
							(SELECT COUNT([TR].[Id]) AS [TotalUnReadCount] FROM [dbo].[TicketReplies] TR (NOLOCK)
									WHERE [Ticket].[Id] = [TR].[TicketId]
										AND [TR].[DeletedAt] IS NULL
										AND [TR].[CreatedFrom] <> '+CAST(@CreatedFrom AS VARCHAR(50))+'
										AND ISNULL([TR].[IsRead], 0) = 0
							) AS [TotalUnReadCount]
					FROM (
							SELECT	[dbo].[Tickets].[Id],[dbo].[Tickets].[CustomerId],MAX(ISNULL(TR.CreatedAt, [dbo].[Tickets].[CreatedAt])) AS MaxCreatedDate,[dbo].[Tickets].[Title],[dbo].[Tickets].[Details],
									[dbo].[Tickets].[CSAgentId],[dbo].[Tickets].CreatedFrom,[dbo].[Tickets].[DeletedAt],[dbo].[Tickets].[Status],[dbo].[Tickets].[Priority],[dbo].[Tickets].[Duedate],[dbo].[Tickets].[CloseDate],
									[dbo].[Tickets].[IsActive],[dbo].[Tickets].[CreatedAt]
							FROM Tickets
								LEFT JOIN [TicketReplies] TR ON [dbo].[Tickets].[Id] = TR.TicketId AND [TR].[DeletedAt] IS NULL AND [Tickets].[DeletedAt] IS NULL
							GROUP BY [dbo].[Tickets].[Id],[dbo].[Tickets].[CustomerId],[dbo].[Tickets].[Title],[dbo].[Tickets].[Details],[dbo].[Tickets].[CSAgentId],[dbo].[Tickets].CreatedFrom,[dbo].[Tickets].[DeletedAt],[dbo].[Tickets].[Status],[dbo].[Tickets].[Priority],
									[dbo].[Tickets].[Duedate],[dbo].[Tickets].[CloseDate],[dbo].[Tickets].[IsActive],[dbo].[Tickets].[CreatedAt]
						) Ticket
							INNER JOIN [dbo].[Customers] WITH (NOLOCK)
								ON [Ticket].[CustomerId] = [dbo].[Customers].[Id] AND [dbo].[Customers].[DeletedAt] IS NULL
							LEFT JOIN [dbo].[CustomerGuestsCheckInFormBuilders] (NOLOCK)
								ON [dbo].[Customers].[Id] = [dbo].[CustomerGuestsCheckInFormBuilders].[CustomerId] AND [dbo].[CustomerGuestsCheckInFormBuilders].[DeletedAt] IS NULL
							LEFT JOIN [dbo].[Users] WITH (NOLOCK)
								ON [Ticket].[CSAgentId] = [dbo].[Users].[Id] 
									AND [dbo].[Users].[DeletedAt] IS NULL AND [Ticket].[CreatedFrom] <> 2
					WHERE ( [Ticket].[DeletedAt] IS NULL )
						AND ( [Ticket].[Status] = '''+ CAST(@Status AS VARCHAR(50)) +''' OR '''+ CAST(@Status AS VARCHAR(50)) +''' = 0 )
						AND ( [Ticket].[Priority] = '''+ CAST(@Priority AS VARCHAR(50)) +''' OR '''+ CAST(@Priority AS VARCHAR(50)) +''' = 0 )
						AND ( [Ticket].[CustomerId] = '''+ CAST(@CustomerId AS VARCHAR(50)) +''' OR '''+ CAST(@CustomerId AS VARCHAR(50)) +''' = 0 )
						AND ( [Ticket].[CSAgentId] = '''+ CAST(@CSAgentId AS VARCHAR(50)) +''' OR '''+ CAST(@CSAgentId AS VARCHAR(50)) +''' = 0 )
						AND ( [Ticket].[CreatedFrom] = '''+ CAST(@CreatedFrom AS VARCHAR(50)) +''' OR '''+ CAST(@CreatedFrom AS VARCHAR(50)) +''' = 0 )
					'
					IF @FromCreate IS NOT NULL
						SET @query += ' AND  CAST([Ticket].[CreatedAt] AS DATE) >= '''+ CAST(CAST(@FromCreate AS DATE) AS NVARCHAR(30))+''''
	
					IF @ToCreate IS NOT NULL
						SET @query += ' AND  CAST([Ticket].[CreatedAt] AS DATE) <= '''+ CAST(CAST(@ToCreate AS DATE) AS NVARCHAR(30))+''''
	
					IF @FromClose IS NOT NULL
						SET @query += ' AND  CAST([Ticket].[CloseDate] AS DATE) >= '''+ CAST(CAST(@FromClose AS DATE) AS NVARCHAR(30))+''''
	
					IF @ToClose IS NOT NULL
						SET @query += ' AND  CAST([Ticket].[CloseDate] AS DATE) <= '''+ CAST(CAST(@ToClose AS DATE) AS NVARCHAR(30))+''''
	
					IF (@ApplyPagination = 1)
						BEGIN
							DECLARE @orderASC VARCHAR(5) = ''
							DECLARE @pagingNo INT = 0
							SET @pagingNo  = @PageSize * (@PageNo - 1)

							IF (@ShortBy = 0)
								SET @orderASC = 'ASC'
							ELSE
								SET @orderASC = 'DESC'
	
							SET @query +=' ORDER BY '
							
							IF(CAST(@ShortBy AS VARCHAR(50)) = '1')    
								SET @query += ' [Ticket].[MaxCreatedDate] '    
							ELSE IF(CAST(@ShortBy AS VARCHAR(50)) = '2')    
								SET @query += ' MONTH([Ticket].[MaxCreatedDate]) ' 
							ELSE IF(cast(@ShortBy AS VARCHAR(50)) = '3')    
								SET @query += ' YEAR([Ticket].[MaxCreatedDate]) '
							ELSE
								SET @query += '[Ticket].[MaxCreatedDate]'
	
							SET @query += ' '+@orderASC+' OFFSET '+ CAST(@pagingNo AS NVARCHAR(10)) +' ROWS FETCH NEXT '+ CAST(@PageSize AS NVARCHAR(10)) +' ROWS ONLY '

						END
	EXEC(@query)   

END");
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
        [Department] VARCHAR(50),
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
		  CASE
			WHEN [GR].[RequestType] = 1 THEN 'EnhanceYourStay' 
		    WHEN [GR].[RequestType] = 2 THEN 'Reception' 
			WHEN [GR].[RequestType] = 3 THEN 'Housekeeping' 
			WHEN [GR].[RequestType] = 4 THEN 'RoomService' 
			WHEN [GR].[RequestType] = 5 THEN 'Concierge' 
			ELSE ''
		   END AS [Department],
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
				'EnhanceYourStay' AS [Department],
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
