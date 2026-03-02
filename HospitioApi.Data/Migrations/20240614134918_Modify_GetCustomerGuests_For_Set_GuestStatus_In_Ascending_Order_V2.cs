using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_GetCustomerGuests_For_Set_GuestStatus_In_Ascending_Order_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			#region GetCustomerGuests
			migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerGuests]    Script Date: 14-06-2024 18:26:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomerGuests]
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
    AS (
        SELECT [dbo].[CustomerGuests].[Id],
               [dbo].[CustomerGuests].[CustomerReservationId],
               [dbo].[CustomerGuests].[Firstname],
               [dbo].[CustomerGuests].[Lastname],
               [dbo].[CustomerGuests].[RoomNumber],
               [dbo].[CustomerGuests].[GuestURL] AS [GuestToken],
               [dbo].[CustomerReservations].[CheckinDate],
               [dbo].[CustomerReservations].[CheckoutDate],
               dbo.GetGuestsStatus(dbo.CustomerGuests.Id) AS [GuestStatus],
               COUNT(*) OVER () as [FilteredCount]
        FROM [dbo].[CustomerGuests] WITH (NOLOCK)
        INNER JOIN [dbo].[CustomerReservations] WITH (NOLOCK)
            ON [dbo].[CustomerReservations].[Id] = [dbo].[CustomerGuests].[CustomerReservationId]
        WHERE [dbo].[CustomerGuests].[DeletedAt] IS NULL
          AND [dbo].[CustomerGuests].[IsActive] = 1
          AND (
              [dbo].[CustomerReservations].[CustomerId] = @CustomerId
              OR 0 = @CustomerId
          )
          AND (
              [dbo].[CustomerGuests].[Firstname] LIKE '%' + @SearchValue + '%'
              OR [dbo].[CustomerGuests].[Lastname] LIKE '%' + @SearchValue + '%'
              OR [dbo].[CustomerGuests].[RoomNumber] LIKE '%' + @SearchValue + '%'
          )
		  AND (dbo.GetGuestsStatus(dbo.CustomerGuests.Id) != 3)
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
    ORDER BY CASE
                 WHEN [GuestStatus] = 2 THEN 1
                 WHEN [GuestStatus] = 1 THEN 2
                 WHEN [GuestStatus] = 3 THEN 3
             END ASC,
             CASE
                 WHEN @SortColumn = 'Firstname' AND @SortOrder = 'ASC' THEN Firstname
             END ASC,
             CASE
                 WHEN @SortColumn = 'Firstname' AND @SortOrder = 'DESC' THEN Firstname
             END DESC,
             CASE
                 WHEN @SortColumn = 'Lastname' AND @SortOrder = 'ASC' THEN Lastname
             END ASC,
             CASE
                 WHEN @SortColumn = 'Lastname' AND @SortOrder = 'DESC' THEN Lastname
             END DESC,
             CASE
                 WHEN @SortColumn = 'RoomNumber' AND @SortOrder = 'ASC' THEN RoomNumber
             END ASC,
             CASE
                 WHEN @SortColumn = 'RoomNumber' AND @SortOrder = 'DESC' THEN RoomNumber
             END DESC,
             CASE
                 WHEN @SortColumn = 'Id' AND @SortOrder = 'ASC' THEN Id
             END ASC,
             CASE
                 WHEN @SortColumn = 'Id' AND @SortOrder = 'DESC' THEN Id
             END DESC
    OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
    OPTION (RECOMPILE)
END
");
			#endregion
		}

		protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
