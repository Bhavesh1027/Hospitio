using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_SP_GetUserProfile_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region SP_GetUserProfile
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[SP_GetUserProfile]    Script Date: 28/03/2024 3:34:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SP_GetUserProfile] 
    @UserId INT,
    @UserType VARCHAR(50)
AS
BEGIN
    -- Check the @UserType to determine which table to query
    IF @UserType = 'Hospitio'
    BEGIN
        -- Retrieve user data from the Users table
        SELECT 
            U.[Id],
            U.[FirstName],
            U.[LastName],
            U.[Email],
            U.[Title],
            U.[ProfilePicture],
            U.[PhoneCountry],
            U.[PhoneNumber],
            U.[UserName],
			(select TOP(1) IncomingTranslationLangage from [HospitioOnboardings]) AS IncomingTranslationLangage,
			(select TOP(1) NoTranslateWords from [HospitioOnboardings]) AS NoTranslateWords,
			(select TOP(1) TaxiTransCommission from [HospitioOnboardings]) AS TaxiTransferCommission,
            ISNULL(UD.[Name],null) AS DepartmentName,
            ISNULL(GD.[Name],null) AS GroupName,
			ISNULL(CD.[FirstName] +' '+ CD.[LastName],null) As SupervisorName,
			ISNULL(UL.[LevelName],null) AS LevelName
        FROM [dbo].[Users] AS U
        LEFT JOIN [dbo].[Departments] AS UD ON U.[DepartmentId] = UD.[Id]
        LEFT JOIN [dbo].[Groups] AS GD ON U.[GroupId] = GD.[Id]
		LEFT Join [dbo].[Users] AS CD ON U.[SupervisorId] = CD.[Id]
		LEFT Join [dbo].[UserLevels] AS UL ON U.[UserLevelId] = UL.[Id]
	
        WHERE U.[DeletedAt] IS NULL
            AND U.[Id] = @UserId;
    END
    ELSE IF @UserType = 'Customer'
    BEGIN
        -- Retrieve customer data from the CustomerUsers table
        SELECT 
            CU.[Id],
            CU.[FirstName],
            CU.[LastName],
            CU.[Email],
            CU.[Title],
            CU.[ProfilePicture],
            CU.[PhoneCountry],
            CU.[PhoneNumber],       
            CU.[UserName],
            CU.[IsActive],
            ISNULL(CD.[Name],null) AS DepartmentName,
			ISNULL(CG.[Name],null) AS GroupName,
			ISNULL(SI.[FirstName] +' '+ SI.[LastName],null) As SupervisorName,
			ISNULL(CL.[LevelName],null) AS LevelName,
			CONVERT(NVARCHAR(36), C.[Guid]) As UserUniqueId
        FROM [dbo].[CustomerUsers] AS CU
           LEFT JOIN [dbo].[CustomerDepartments] AS CD ON CU.[CustomerDepartmentId] = CD.[Id]
		   LEFT JOIN [dbo].[CustomerGroups] AS CG ON CU.[CustomerGroupId] = CG.[Id]
		   LEFT Join [dbo].[Users] AS SI ON CU.[SupervisorId] = SI.[Id]
		   LEFT Join [dbo].[CustomerLevels] AS CL ON CU.[CustomerLevelId] = CL.[Id]
		   LEFT JOIN [dbo].[Customers] AS C ON CU.[CustomerId] = C.[Id]
		
        WHERE CU.[DeletedAt] IS NULL
            AND CU.[Id] = @UserId;
    END
   
   
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
