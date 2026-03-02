using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_GetCustomerGuests_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerGuests]    Script Date: 10/10/2023 5:25:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuests]
(
    @CustomerId INT = 0,
    @SearchValue NVARCHAR(50) = '',
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'Firstname',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchValue = LTRIM(RTRIM(@SearchValue));

    WITH CustomerGuests_Results
    AS (SELECT [dbo].[CustomerGuests].[Id],
               [dbo].[CustomerGuests].[CustomerReservationId],
               [dbo].[CustomerGuests].[Firstname],
               [dbo].[CustomerGuests].[Lastname],
               [dbo].[CustomerGuests].[RoomNumber],
			   [dbo].[CustomerGuests].[GuestToken],
			   [dbo].[CustomerReservations].[CheckinDate],
			   [dbo].[CustomerReservations].[CheckoutDate],
               dbo.GetGuestsStatus(dbo.CustomerGuests.Id) AS [GuestStatus],
               COUNT(*) OVER () as [FilteredCount]
        FROM [dbo].[CustomerGuests] WITH (NOLOCK)
            INNER JOIN [dbo].[CustomerReservations] WITH (NOLOCK)
                ON [dbo].[CustomerReservations].[Id] = [dbo].[CustomerGuests].[CustomerReservationId]
        WHERE [dbo].[CustomerGuests].[DeletedAt] IS NULL AND [dbo].[CustomerGuests].[IsActive] = 1
              AND (
                      [dbo].[CustomerReservations].[CustomerId] = @CustomerId
                      OR 0 = @CustomerId
                  )
              AND (
                      [dbo].[CustomerGuests].[Firstname] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[Lastname] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[RoomNumber] LIKE '%' + @SearchValue + '%'
                  )
        ORDER BY CASE
                     WHEN
                     (
                         @SortColumn = 'Firstname'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Firstname
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Firstname'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Firstname
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Lastname'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Lastname
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Lastname'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Lastname
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'RoomNumber'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         RoomNumber
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'RoomNumber'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         RoomNumber
                 END DESC
				 OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [CustomerReservationId],
           [Firstname],
           [Lastname],
           [RoomNumber],
		   [CheckinDate],
		   [CheckoutDate],
           [FilteredCount],
           [GuestStatus],
		   [GuestToken]
    FROM CustomerGuests_Results
    OPTION (RECOMPILE)
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
