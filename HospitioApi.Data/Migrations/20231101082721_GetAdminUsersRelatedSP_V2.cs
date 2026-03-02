using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetAdminUsersRelatedSP_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[GetAdminUsers]
(
    @UserId int = 0,
    @UserLevel int = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    IF @UserLevel = 1 OR @UserLevel = 2
    BEGIN
        -- For usertype 1 or 2, userlevel > 2
        SELECT [Id],
            ISNULL([FirstName], '') + SPACE(1) + ISNULL([LastName], '') AS [Name],
            [PhoneCountry],
            [PhoneNumber]
        FROM [dbo].[Users] (NOLOCK)
        WHERE [DeletedAt] IS NULL AND [IsActive] = 1 AND UserLevelId > 2
    END
    ELSE IF @UserLevel = 3
    BEGIN
        -- For usertype 3, userlevel > 3 and filter by @userid
        DECLARE @CurrentUserId INT

        DECLARE UserCursor CURSOR FOR
        SELECT [Id]
        FROM [dbo].[Users] (NOLOCK)
        WHERE [DeletedAt] IS NULL AND [IsActive] = 1 AND [UserLevelId] > 3 AND [SupervisorId] = @UserId

        OPEN UserCursor
        FETCH NEXT FROM UserCursor INTO @CurrentUserId

        WHILE @@FETCH_STATUS = 0
        BEGIN
            SELECT [Id],
                ISNULL([FirstName], '') + SPACE(1) + ISNULL([LastName], '') AS [Name],
                [PhoneCountry],
                [PhoneNumber]
            FROM [dbo].[Users] (NOLOCK)
            WHERE [SupervisorId] = @CurrentUserId AND [DeletedAt] IS NULL AND [IsActive] = 1

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
        FROM [dbo].[Users] (NOLOCK)
        WHERE [DeletedAt] IS NULL AND [IsActive] = 1 AND [UserLevelId] > 4 AND [SupervisorId] = @UserId
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
