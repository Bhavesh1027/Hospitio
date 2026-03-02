using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerUsersByCustomerIdRelatedSP_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
/****** Object:  StoredProcedure [dbo].[GetCustomerUsersByCustomerId]    Script Date: 03-01-2024 17:03:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER     PROCEDURE [dbo].[GetCustomerUsersByCustomerId]
(
	@CustomerId INT = 0,
	@UserLevel int = 0,
	@UserId int = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

	IF @UserLevel = 1 
    BEGIN
        -- For usertype 1 
        SELECT [Id],
            ISNULL([FirstName], '') + SPACE(1) + ISNULL([LastName], '') AS [Name],
            [PhoneCountry],
            [PhoneNumber]
        FROM [dbo].[CustomerUsers] (NOLOCK)
        WHERE [DeletedAt] IS NULL AND [IsActive] = 1 AND CustomerLevelId > 1 AND CustomerId = @CustomerId
    END
	IF @UserLevel = 2
    BEGIN
        -- For usertype 2, userlevel > 2
        SELECT [Id],
            ISNULL([FirstName], '') + SPACE(1) + ISNULL([LastName], '') AS [Name],
            [PhoneCountry],
            [PhoneNumber]
        FROM [dbo].[CustomerUsers] (NOLOCK)
        WHERE [DeletedAt] IS NULL AND [IsActive] = 1 AND CustomerLevelId > 2 AND CustomerId = @CustomerId
    END
    ELSE IF @UserLevel = 3
    BEGIN
        -- For usertype 3, userlevel > 3 and filter by @userid
        DECLARE @CurrentUserId INT

        DECLARE UserCursor CURSOR FOR
        SELECT [Id]
        FROM [dbo].[CustomerUsers] (NOLOCK)
        WHERE [DeletedAt] IS NULL AND [IsActive] = 1 AND [CustomerLevelId] > 3 AND [SupervisorId] = @UserId AND CustomerId = @CustomerId

        OPEN UserCursor
        FETCH NEXT FROM UserCursor INTO @CurrentUserId

        WHILE @@FETCH_STATUS = 0
        BEGIN
            SELECT [Id],
                ISNULL([FirstName], '') + SPACE(1) + ISNULL([LastName], '') AS [Name],
                [PhoneCountry],
                [PhoneNumber]
            FROM [dbo].[CustomerUsers] (NOLOCK)
            WHERE [SupervisorId] = @CurrentUserId AND [DeletedAt] IS NULL AND [IsActive] = 1 AND CustomerId = @CustomerId

            FETCH NEXT FROM UserCursor INTO @CurrentUserId
        END

        CLOSE UserCursor
        DEALLOCATE UserCursor
    END
    ELSE IF @UserLevel = 4
    BEGIN
        -- For usertype 4, userlevel > 4 and filter by @userid
        SELECT [Id],
            ISNULL([FirstName], '') + SPACE(1) + ISNULL([LastName], '') AS [Name],
            [PhoneCountry],
            [PhoneNumber]
        FROM [dbo].[CustomerUsers] (NOLOCK)
        WHERE [DeletedAt] IS NULL AND [IsActive] = 1 AND [CustomerLevelId] > 4 AND [SupervisorId] = @UserId AND CustomerId = @CustomerId
    END
    -- Add more conditions as needed for other usertypes

END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
