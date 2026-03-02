using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_AddPropertyInfo_AND_GetUsers_SPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region AddPropertyInfo
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[AddPropertyInfo]    Script Date: 3/10/2023 5:49:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[AddPropertyInfo]
(
	@AppBuilderId int = 0,
	@CustomerId int  = 0,
	@UserType INT = 0
)
AS BEGIN
	SET NOCOUNT ON;
	Declare @CustomerAppBuliderId int
	Declare @DisplayOrder int 
	Declare @PropertyId int
	DECLARE @RoomName NVARCHAR(100)
	Declare @ISDELETE BIT = 0;

	Select @CustomerAppBuliderId = COUNT(CustomerGuestAppBuilderId)
	From CustomerPropertyInformations With (NOLOCK)
	Where CustomerId = @CustomerId And
		CustomerGuestAppBuilderId = @AppBuilderId and DeletedAt is null

	SELECT @RoomName = crn.Name
	FROM CustomerGuestAppBuilders cga
	JOIN CustomerRoomNames crn ON cga.CustomerRoomNameId = crn.Id
	WHERE cga.Id = @AppBuilderId

	IF(@CustomerAppBuliderId = 0)
	BEGIN 
		INSERT INTO CustomerPropertyInformations (
	   [CustomerId]
      ,[CustomerGuestAppBuilderId]
      ,[WifiUsername]
      ,[WifiPassword]
      ,[Overview]
      ,[CheckInPolicy]
      ,[TermsAndConditions]
      ,[Street]
      ,[StreetNumber]
      ,[City]
      ,[Postalcode]
      ,[Country])
	   Values
	   (@CustomerId ,@AppBuilderId ,null,null,null,null,null,null,null,null,null,null)
	End

	Select @PropertyId=Id 
	From CustomerPropertyInformations With (NOLOCK)
	Where CustomerId = @CustomerId And
		CustomerGuestAppBuilderId = @AppBuilderId

	Select @DisplayOrder = COUNT(RefrenceId)
	From ScreenDisplayOrderAndStatuses With (NOLOCK)
	where RefrenceId = @CustomerAppBuliderId
	AND ScreenName = 2

	IF(@DisplayOrder = 0)
	BEGIN
		INSERT INTO dbo.ScreenDisplayOrderAndStatuses(ScreenName,JsonData,RefrenceId,IsActive)
		Values(1,'{
    ""Address"": {
      ""IsActive"": false
    },
    ""Wifi"": {
      ""IsActive"": false
    },
    ""Service"": {
      ""IsActive"": false
    },
    ""Overview"": {
      ""IsActive"": false
    },
    ""Gallary"": {
      ""IsActive"": false
    },
    ""EmergencyNumber"": {
      ""IsActive"": false,
      ""Names"": [
        {
          ""Id"": 0,
          ""Name"": """"
        }
      ]
    },
    ""RecommendedforYou"": {
      ""IsActive"": false,
      ""Names"": [
        {
          ""Id"": 0,
          ""Name"": """"
        }
      ]
    },
    ""AroundYou"": {
      ""IsActive"": false,
      ""Names"": [
        {
          ""Id"": 0,
          ""Name"": """"
        }
      ]
    },
    ""Checkin&CheckoutPolicies"": {
      ""IsActive"": false
    },
    ""Terms&Conditions"": {
      ""IsActive"": false
    }
  }',@PropertyId,1)
	END

	BEGIN

	WITH Property_Information AS (
		SELECT
			CI.Id,
			CI.CustomerId,
			MAX(CASE WHEN [key] = 'CustomerGuestAppBuilderId' THEN [value] END) AS [CustomerGuestAppBuilderId],
            MAX(CASE WHEN [key] = 'WifiUsername' THEN [value] END) AS [WifiUsername],
            MAX(CASE WHEN [key] = 'WifiPassword' THEN [value] END) AS [WifiPassword],
            MAX(CASE WHEN [key] = 'Overview' THEN [value] END) AS [Overview],
            MAX(CASE WHEN [key] = 'CheckInPolicy' THEN [value] END) AS [CheckInPolicy],
            MAX(CASE WHEN [key] = 'TermsAndConditions' THEN [value] END) AS [TermsAndConditions],
            MAX(CASE WHEN [key] = 'Street' THEN [value] END) AS [Street],
            MAX(CASE WHEN [key] = 'StreetNumber' THEN [value] END) AS [StreetNumber],
            MAX(CASE WHEN [key] = 'City' THEN [value] END) AS [City],
            MAX(CASE WHEN [key] = 'Postalcode' THEN [value] END) AS [Postalcode],
            MAX(CASE WHEN [key] = 'Country' THEN [value] END) AS [Country],
			@RoomName AS RoomName
		FROM CustomerPropertyInformations CI
		CROSS APPLY OPENJSON(JsonData)
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)
	    GROUP BY CI.Id, CI.CustomerId
		
		UNION ALL
		
		SELECT
			CI.Id,
			CI.CustomerId,
			CI.[CustomerGuestAppBuilderId],
			CI.[WifiUsername],
			CI.[WifiPassword],
			CI.[Overview],
			CI.[CheckInPolicy],
			CI.[TermsAndConditions],
			CI.[Street],
			CI.[StreetNumber],
			CI.[City],
			CI.[Postalcode],
			CI.[Country],
			@RoomName AS RoomName
		FROM CustomerPropertyInformations CI
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	), 
	Property_Extra AS (
		SELECT [Id],
			[CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.ExtraType') AS [ExtraType],
			JSON_VALUE(JsonData, '$.Name') AS [Name],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyExtras CE 
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)

		UNION ALL

		SELECT CE.[Id]
		  ,CE.[CustomerPropertyInformationId]
		  ,CE.[ExtraType]
		  ,CE.[Name]
		  ,@ISDELETE As [IsDeleted]
		FROM CustomerPropertyExtras CE 
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	),
	Property_Extra_Detail AS
    (
		SELECT 
			[Id],
            [CustomerPropertyExtraId],
            JSON_VALUE(JsonData, '$.Description') AS [Description],
            JSON_VALUE(JsonData, '$.Link') AS [Link],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyExtraDetails CED
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)

		UNION ALL

		SELECT  
			CED.[Id],
			CED.[CustomerPropertyExtraId],
			CED.[Description],
			CED.[Link],
			@ISDELETE As [IsDeleted]
		FROM CustomerPropertyExtraDetails CED
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	),
	
	Emergency_Numbers AS (
		SELECT 
			[Id],
			[CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.Name') AS [Name],
			JSON_VALUE(JsonData, '$.PhoneCountry') AS [PhoneCountry],
			JSON_VALUE(JsonData, '$.PhoneNumber') AS [PhoneNumber],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyEmergencyNumbers CEN
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)

		UNION ALL

		SELECT 
			CEN.[Id],
			CEN.[CustomerPropertyInformationId],
			CEN.[Name],
			CEN.[PhoneCountry],
			CEN.[PhoneNumber],
			@ISDELETE As [IsDeleted]
		FROM CustomerPropertyEmergencyNumbers CEN
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	),
	Property_Gallery AS (
		SELECT 
			[Id],
			[CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.PropertyImage') AS [PropertyImage],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyGalleries CG
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)

		UNION ALL

		SELECT CG.[Id]
			  ,CG.[CustomerPropertyInformationId]
			  ,CG.[PropertyImage],
			  @ISDELETE As [IsDeleted]
		FROM CustomerPropertyGalleries CG
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	),
	Property_Services AS (
		SELECT 
			CS.[Id],
			[CustomerPropertyInformationId],
			JSON_VALUE(JsonData, '$.Name') AS [Name],
			JSON_VALUE(JsonData, '$.Icon') AS [Icon],
			JSON_VALUE(JsonData, '$.Description') AS [Description],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyServices CS
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)

		UNION ALL

		SELECT 
			CS.[Id],
			CS.[CustomerPropertyInformationId],
			CS.[Name],
			CS.[Icon],
			CS.[Description],
			 @ISDELETE As [IsDeleted]
		FROM CustomerPropertyServices CS
		WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	),
	Property_Service_Images AS (
		SELECT 
			CSI.[Id],
			[CustomerPropertyServiceId],
			JSON_VALUE(JsonData, '$.ServiceImages') AS [ServiceImages],
			JSON_VALUE(JsonData, '$.IsDeleted') AS [IsDeleted]
		FROM CustomerPropertyServiceImages CSI
		WHERE ISJSON(JsonData) = 1
            AND [DeletedAt] IS NULL
			AND (@UserType = 2)

		UNION ALL

		Select  CSI.[Id]
				,CSI.[CustomerPropertyServiceId]
				,CSI.[ServiceImages],
				@ISDELETE As [IsDeleted]
			From CustomerPropertyServiceImages CSI
			WHERE [DeletedAt] IS NULL
			AND ((@UserType = 2 AND JsonData IS NULL) OR (@UserType = 3 AND IsPublish = 1)) 
	)
	

	SELECT(
		 SELECT
			CI.Id
		   ,CI.[CustomerGuestAppBuilderId]
		  ,CI.[WifiUsername]
		  ,CI.[WifiPassword]
		  ,CI.[Overview]
		  ,CI.[CheckInPolicy]
		  ,CI.[TermsAndConditions]
		  ,CI.[Street]
		  ,CI.[StreetNumber]
		  ,CI.[City]
		  ,CI.[Postalcode]
		  ,CI.[Country]
		  ,@RoomName AS RoomName
		,JSON_QUERY(( 
		SELECT CS.[Id]
			  ,CS.[CustomerPropertyInformationId]
			  ,CS.[Name]
			  ,CS.[Icon]
			  ,CS.[Description]
			  ,CS.[IsDeleted]
			  ,JSON_QUERY((
			  Select  CSI.[Id]
					,CSI.[CustomerPropertyServiceId]
					,CSI.[ServiceImages]
					,CSI.[IsDeleted]
			  From Property_Service_Images CSI With (NOLOCK)
			  WHERE CSI.CustomerPropertyServiceId = CS.Id
			  AND CSI.IsDeleted != 1
			  FOR JSON PATH
			  )) as [CustomerPropertyInfoServiceImagesOuts]
		FROM Property_Services CS With (NOLOCK) where CustomerPropertyInformationId=CI.Id 
		AND CS.IsDeleted != 1
		FOR JSON PATH
		)) as [CustomerPropertyInfoServicesOuts]
		,JSON_QUERY(( 
		SELECT CG.[Id]
			  ,CG.[CustomerPropertyInformationId]
			  ,CG.[PropertyImage]
			  ,CG.[IsDeleted]
		FROM Property_Gallery CG With (NOLOCK) 
		where CG.CustomerPropertyInformationId=CI.Id
		AND CG.IsDeleted != 1
		Order By CG.[ID]
		FOR JSON PATH
		)) as [CustomerPropertyInfoGalleriesOuts]
		,JSON_QUERY(( 
		SELECT CEN.[Id]
		  ,CEN.[CustomerPropertyInformationId]
		  ,CEN.[Name]
		  ,CEN.[PhoneCountry]
		  ,CEN.[PhoneNumber]
		  ,CEN.[IsDeleted]	
		FROM Emergency_Numbers CEN With (NOLOCK) where CEN.CustomerPropertyInformationId=CI.Id 
		AND CEN.IsDeleted != 1
		FOR JSON PATH
		)) as [CustomerPropertyInfoEmergencyNumbersOuts]
		,JSON_QUERY(( 
		SELECT CE.[Id]
		  ,CE.[CustomerPropertyInformationId]
		  ,CE.[ExtraType]
		  ,CE.[Name]
		  ,CE.[IsDeleted]
		   ,JSON_QUERY((
			  Select  CED.[Id]
				  ,CED.[CustomerPropertyExtraId]
				  ,CED.[Description]
				  ,CED.[Link]
				  ,CED.[IsDeleted]
			  From Property_Extra_Detail CED With (NOLOCK)
			  WHERE CED.CustomerPropertyExtraId = CE.Id
			  AND CED.IsDeleted != 1
			  FOR JSON PATH
			  )) as [customerPropertyExtraDetailsOuts]
		FROM Property_Extra CE With (NOLOCK) where CE.CustomerPropertyInformationId=CI.Id
		AND CE.IsDeleted != 1
		FOR JSON PATH
		)) as [CustomerPropertyInfoExtrasOuts]
		FROM Property_Information CI With (NOLOCK)
		Where CI.CustomerId = @CustomerId
		AND CI.CustomerGuestAppBuilderId = @AppBuilderId
		FOR JSON PATH )
	END
END");
            #endregion

            #region GetUsers
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetUsers]    Script Date: 3/10/2023 5:44:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetUsers]
(
@DepartmentId INT = 0,
@GroupId INT = 0,
@UserLevelId INT = 0,
@UserId INT = 0
)
AS

BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON
	Declare @query NVARCHAR(MAX) = ''

	If(@UserLevelId = 3 OR @UserLevelId=2)
	Begin
		Set @query += 'SELECT
				(SELECT
						[Id],
						[FirstName],
						[LastName],
						[Email],
						[Title],
						[ProfilePicture],
						[PhoneCountry],
						[PhoneNumber],(
						SELECT [Id],[PermissionId],[UserId],[IsView],[IsEdit],[IsUpload],[IsReply],[IsSend],
								(
									SELECT [Name]
									FROM [dbo].[Permissions] (NOLOCK)
									WHERE [DeletedAt] IS NULL 
									AND [Id] = [up].[PermissionId]
								) AS [PermissionName]
						FROM [dbo].[UsersPermissions] up (NOLOCK)
						WHERE [up].[UserId] = [us].[Id]
						FOR JSON PATH
					) AS [UserModulePermissions]
				FROM [dbo].[Users] us
				WHERE [us].[DeletedAt] IS NULL
				AND [us].[UserLevelId] = 2
				AND [us].[IsActive] = 1
				AND [us].[Id] <>'+ Cast(@UserId as NVARCHAR(50)) +'
				ORDER BY [us].[Id] OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY
			FOR JSON PATH) as UserOut'
	End
	Else If(@UserLevelId = 4)
    Begin
        Set @query += 'SELECT
				(SELECT
					   [Id],
					   [FirstName],
					   [LastName],
					   [Email],
					   [Title],
					   [ProfilePicture],
					   [PhoneCountry],
					   [PhoneNumber],(
                       SELECT [Id],[PermissionId],[UserId],[IsView],[IsEdit],[IsUpload],[IsReply],[IsSend],
                              (
                                  SELECT [Name]
                                  FROM [dbo].[Permissions] (NOLOCK)
                                  WHERE [DeletedAt] IS NULL 
									AND [Id] = [up].[PermissionId]
                              ) AS [PermissionName]
                       FROM [dbo].[UsersPermissions] up (NOLOCK)
                       WHERE [up].[UserId] = [us].[Id]
                       FOR JSON PATH
                   ) AS [UserModulePermissions]
				FROM [dbo].[Users] us
				WHERE [us].[DeletedAt] IS NULL
				AND [us].[Id] <>'+ Cast(@UserId as NVARCHAR(50)) +'
				AND [us].[DepartmentId] = ' + Cast(ISNULL(@DepartmentId, 0) as NVARCHAR(50)) +
                   ' AND [us].[IsActive] = 1 AND [us].[UserLevelId] = 3
				ORDER BY [us].[Id] OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY
            FOR JSON PATH) as UserOut'
    End
	Else If(@UserLevelId = 5)
    Begin
        Set @query += 'SELECT
				(SELECT
					   [Id],
					   [FirstName],
					   [LastName],
					   [Email],
					   [Title],
					   [ProfilePicture],
					   [PhoneCountry],
					   [PhoneNumber],(
                       SELECT [Id],[PermissionId],[UserId],[IsView],[IsEdit],[IsUpload],[IsReply],[IsSend],
                              (
                                  SELECT [Name]
                                  FROM [dbo].[Permissions] (NOLOCK)
                                  WHERE [DeletedAt] IS NULL 
									AND [Id] = [up].[PermissionId]
                              ) AS [PermissionName]
                       FROM [dbo].[UsersPermissions] up (NOLOCK)
                       WHERE [up].[UserId] = [us].[Id]
                       FOR JSON PATH
                   ) AS [UserModulePermissions]
				FROM [dbo].[Users] us
				WHERE [us].[DeletedAt] IS NULL
				AND [us].[Id] <>'+ Cast(@UserId as NVARCHAR(50)) +'
				AND (([us].[DepartmentId] = ' + Cast(ISNULL(@DepartmentId, 0) as NVARCHAR(50)) +
                   ' AND [us].[UserLevelId] = 3)
				OR ([us].[GroupId] = ' + Cast(ISNULL(@GroupId, 0) as NVARCHAR(50)) +
                   ' AND [us].[UserLevelId] = 4))
				AND [us].[IsActive] = 1
				ORDER BY [us].[Id] FOR JSON PATH) as UserOut'
    End
	Else
	BEGIN


		Set @query += 'SELECT
			(SELECT [Id],
						   [FirstName],
						   [LastName],
						   [Email],
						   [Title],
						   [ProfilePicture],
						   [PhoneCountry],
						   [PhoneNumber],(
						   SELECT [Id],[PermissionId],[UserId],[IsView],[IsEdit],[IsUpload],[IsReply],[IsSend],
								  (
									  SELECT [Name]
									  FROM [dbo].[Permissions] (NOLOCK)
									  WHERE [DeletedAt] IS NULL 
										AND [Id] = [up].[PermissionId]
								  ) AS [PermissionName]
						   FROM [dbo].[UsersPermissions] up (NOLOCK)
						   WHERE [up].[UserId] = [us].[Id]
						   FOR JSON PATH
					   ) AS [UserModulePermissions]
					FROM [dbo].[Users] us
					WHERE [us].[DeletedAt] IS NULL
					AND ([us].DepartmentId = '+ Cast(ISNULL(@DepartmentId,0) as NVARCHAR(50)) +' OR ('+ Cast(ISNULL(@DepartmentId,0) as NVARCHAR(50)) +' = 0))
					AND ([us].GroupId = '+ Cast(ISNULL(@GroupId,0) as NVARCHAR(50)) +' OR ('+ Cast(ISNULL(@GroupId,0) as NVARCHAR(50))+' = 0))
					AND ([us].UserLevelId < '+ Cast(ISNULL(@UserLevelId,0) as NVARCHAR(50)) +' OR ('+ Cast(ISNULL(@UserLevelId,0) as NVARCHAR(50)) +' = 0))' 


			If(ISNULL(@UserLevelId,0) = 4 OR ISNULL(@UserLevelId,0) = 5)
				Set @query += 'AND [us].UserLevelId <> 2'

			If(IsNULL(@UserLevelId,0) <> 0)
				Set @query += 'AND [us].UserLevelId <> 1' 

		Set @query += 'ORDER BY [us].[Id]
				FOR JSON PATH
			) as UserOut'


	END

		EXEC sp_executesql @query
		Print @query
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
