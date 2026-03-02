using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class modify_sp_SP_Get_Communication_CustomerGuest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/*** Object:  StoredProcedure [dbo].[SP_Get_Communication_CustomerGuest]    Script Date: 23-10-2023 12:33:19 ***/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER       PROCEDURE [dbo].[SP_Get_Communication_CustomerGuest] 
(
	@CustomerId INT = 0,
	@SearchString NVARCHAR(50) = NULL,
	@PageNo INT = 1,
    @PageSize INT = 10
	
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

	SET @SearchString = LTRIM(RTRIM(@SearchString));

	IF OBJECT_ID('tempdb..#TempTables') IS NOT NULL
        DROP TABLE #TempTables;

    CREATE TABLE #TempTables
    (
        [Id] INT,
		[CustomerId] INT,
        [CustomerReservationId] NVARCHAr(100),
        [FirstName] NVARCHAR(50),
        [LastName] NVARCHAR(50),
        [Email] NVARCHAR(100),
        [Picture] NVARCHAR(500),
        [PhoneCountry] NVARCHAR(3),
        [PhoneNumber] NVARCHAR(20),
        [Country] NVARCHAR(10),
        [Language] NVARCHAR(10),
		[UserType] NVARCHAR(20),
		[ChatType] NVARCHAR(20)
   )

   INSERT INTO #TempTables ([Id],[CustomerId],[CustomerReservationId],[FirstName],[LastName],[Email],[Picture],[PhoneCountry],[PhoneNumber],[Country],[Language],[UserType],[ChatType])
    SELECT [CG].[Id],
           [CR].[CustomerId],
		   [CG].[CustomerReservationId],
           [CG].[Firstname],
           [CG].[Lastname],
           [CG].[Email],
           [CG].[Picture],
           [CG].[PhoneCountry],
           [CG].[PhoneNumber],
           [CG].[Country],
           [CG].[Language],
		   'CustomerGuest',
		   (CASE
				WHEN (
						SELECT COUNT(CM.Id)
						FROM ChannelMessages CM
						WHERE ChannelId = CU1.ChannelId
						AND MessageSender = 3
						AND MessageSenderId = CG.Id
					) > 0
				THEN 'inbox'
				WHEN 
					(SELECT COUNT(CM.Id)
						FROM ChannelMessages CM
						WHERE ChannelId = CU1.ChannelId
						AND MessageSender = 3
						AND MessageSenderId = CG.Id
					) = 0
					AND (CONVERT(DATE, CR.CheckinDate) <= CONVERT(DATE, GETDATE())
					AND CONVERT(DATE, CR.CheckoutDate) >= CONVERT(DATE, GETDATE()))
				
				THEN 'in-bound'
				WHEN (
						(
						SELECT COUNT(CM.Id)
						FROM ChannelMessages CM
						WHERE ChannelId = CU1.ChannelId
						AND MessageSender = 3
						AND MessageSenderId = CG.Id
						) = 0
						AND
						(
							(CONVERT(DATE, CR.CheckinDate) > CONVERT(DATE, GETDATE())
							AND CONVERT(DATE, CR.CheckoutDate) > CONVERT(DATE, GETDATE()))
						)
				)
				THEN 'contacted'
				ELSE ''
			END) AS ChatType
    FROM [dbo].[CustomerGuests] CG (NOLOCK)
        INNER JOIN [dbo].[CustomerReservations] CR (NOLOCK) ON [CR].[Id] = [CG].[CustomerReservationId] AND [CG].[DeletedAt] IS NULL
		LEFT JOIN [dbo].[ChannelUsers] CU1 ON CU1.UserId = CG.Id AND CU1.UserType = 'CustomerGuest'
		LEFT JOIN [dbo].[ChannelMessages] CM ON [CM].[ChannelId] = [CU1].[ChannelId] 
    WHERE [CR].[DeletedAt] IS NULL 
		AND [CR].[CustomerId] = @CustomerId
    UNION
    SELECT [U].[Id],
           NULL AS [CustomerId],
		   NULL AS [CustomerReservationId],
           [U].[FirstName],
           [U].[LastName],
           [U].[Email],
           [U].[ProfilePicture],
           [U].[PhoneCountry],
           [U].[PhoneNumber],
           NULL AS [Country],
           NULL AS [Language],
		   'HospitioUser',
		   '' AS [ChatType]
    FROM [dbo].[Users] U (NOLOCK)
    WHERE [U].[DeletedAt] IS NULL AND UserLevelId = 1

            SELECT	[Id],
					[CustomerId],
					[CustomerReservationId],
					[FirstName],
					[LastName],
					[Email],
					[Picture],
					[PhoneCountry],
					[PhoneNumber],
					[Country],
					[Language],
					[UserType],
					[ChatType]
            FROM #TempTables
			WHERE 
			(
				[FirstName] LIKE '%' + ISNULL(@SearchString,'') + '%'
				OR 
				[LastName] LIKE '%' + ISNULL(@SearchString,'') + '%'
			)
			ORDER BY [CustomerId] ASC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY

END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
