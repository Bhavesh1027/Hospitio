using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_sp_GetExtradetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetExtradetails]    Script Date: 10/10/2023 11:24:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetExtradetails]
(
    @CustomerAppBuliderId INT = 0,
    @CustomerId iNT = 0,
    @GuestService NVARCHAR(100) = ''
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT CASE
               WHEN @GuestService = 'Reception' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppReceptionItems] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
				   And DeletedAt IS NULL
               )
               WHEN @GuestService = 'Enhance your Stay' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppEnhanceYourStayItems]
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
				   And DeletedAt IS NULL
               )
               WHEN @GuestService = 'Room Service' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppRoomServiceItems] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
				   And DeletedAt IS NULL
               )
               WHEN @GuestService = 'Concierge' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppConciergeItems]
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
				   And DeletedAt IS NULL
               )
               WHEN @GuestService = 'Housekeeping' Then
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppHousekeepingItems]
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
				   And DeletedAt IS NULL
               )
           END AS [Items],
           CASE
               WHEN @GuestService = 'Reception' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppReceptionCategories] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
				   And DeletedAt IS NULL
               )
               WHEN @GuestService = 'Enhance your Stay' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppEnhanceYourStayCategories] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
				   And DeletedAt IS NULL
               )
               WHEN @GuestService = 'Room Service' THEN
               (
                   Select Count(*)
                   FROM [dbo].[CustomerGuestAppRoomServiceCategories] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
				    And DeletedAt IS NULL
               )
               WHEN @GuestService = 'Concierge' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppConciergeCategories] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
				    And DeletedAt IS NULL
               )
               WHEN @GuestService = 'Housekeeping' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppHousekeepingCategories] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
				   And DeletedAt IS NULL
               )
           END AS [Categories]
    FROM [dbo].[CustomerGuestAppBuilders] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [dbo].[CustomerGuestAppBuilders].[Id] = @CustomerAppBuliderId
          AND [dbo].[CustomerGuestAppBuilders].[CustomerId] = @CustomerId
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
