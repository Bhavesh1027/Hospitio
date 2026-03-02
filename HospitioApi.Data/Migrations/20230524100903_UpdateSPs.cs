using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class UpdateSPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            #region Customer Get
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomer]    Script Date: 03-05-2023 18:46:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER Procedure [dbo].[GetCustomer] 
@Id int=1
AS

 
SELECT 
( 
SELECT [Id]
      ,[BusinessName]
      ,[BusinessTypeId]
      ,[NoOfRooms]
      ,[TimeZone]
      ,[WhatsappCountry]
      ,[WhatsappNumber]
      ,[Cname]
      ,[ClientDoamin]
      ,[Email]
      ,[Messenger]
      ,[ViberCountry]
      ,[ViberNumber]
      ,[TelegramCounty]
      ,[TelegramNumber]
      ,[PhoneCountry]
      ,[PhoneNumber]
      ,[BusinessStartTime]
      ,[BusinessCloseTime]
      ,[DoNotDisturbGuestStartTime]
      ,[DoNotDisturbGuestEndTime]
      ,[StaffAlertsOffduty]
      ,[NoMessageToGuestWhileQuiteTime]
      ,[IncomingTranslationLangage]
      ,[NoTranslateWords]
      ,[ProductId]
      ,[SmsTitle]
      ,[IsActive]
,JSON_QUERY(( 
SELECT [Id]
	  ,[CustomerId]
      ,[Name]
      ,[CreatedFrom]
      ,[IsActive]
FROM [dbo].[CustomerRoomNames] where CustomerId=m.Id
FOR JSON PATH
)) as [UpdateCustomerRoomNamesOuts]
FROM  [dbo].[Customers] m 
where Id = @Id
FOR JSON PATH )");
            #endregion

            #region Customers Get
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomers]    Script Date: 15-05-2023 17:37:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[GetCustomers] --,'',1,5
(
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'BusinessName',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS BEGIN
    SET NOCOUNT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))

    ; WITH Customer_Results AS
    (
		SELECT dbo.Customers.Id, dbo.CustomerUsers.Title,dbo.CustomerUsers.FirstName, dbo.CustomerUsers.LastName, dbo.CustomerUsers.ProfilePicture, dbo.Customers.BusinessName, dbo.BusinessTypes.BizType, dbo.Products.Name AS ""ServicePackName""
		,COUNT(*) OVER() as FilteredCount
		FROM     dbo.Customers INNER JOIN
		dbo.BusinessTypes ON dbo.Customers.BusinessTypeId = dbo.BusinessTypes.Id LEFT OUTER JOIN
		dbo.Products ON dbo.Customers.ProductId = dbo.Products.Id LEFT OUTER JOIN
		dbo.CustomerUsers ON dbo.Customers.Id = dbo.CustomerUsers.CustomerId

		WHERE dbo.Customers.DeletedAt is null and (
		    BusinessName LIKE '%' + @SearchValue + '%' OR
		    dbo.CustomerUsers.FirstName LIKE '%' + @SearchValue + '%' OR
            dbo.CustomerUsers.LastName LIKE '%' + @SearchValue + '%' OR
            dbo.BusinessTypes.BizType LIKE '%' + @SearchValue + '%' OR
            dbo.Products.Name LIKE '%' + @SearchValue + '%'
		)

		ORDER BY
		CASE WHEN (@SortColumn = 'BusinessName' AND @SortOrder='ASC')
        THEN Name
        END ASC,
        
		CASE WHEN (@SortColumn = 'BusinessName' AND @SortOrder='DESC')
        THEN Name
        END DESC
            
		OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
    )

	select Id,FirstName,LastName,ProfilePicture,BusinessName,Title,BizType,'Name' as ""ServicePackName"",FilteredCount 
	from Customer_Results
	OPTION (RECOMPILE)
    
END
");
            #endregion

            #region GetCustomerForHospitioAdmin
			migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerForHospitioAdmin]    Script Date: 04-05-2023 12:56:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetCustomerForHospitioAdmin]
@Id int=1
AS
SELECT m.[Id]
      ,m.[BusinessName]
      ,m.[BusinessTypeId]
      ,m.[NoOfRooms]
      ,m.[TimeZone]
      ,m.[WhatsappCountry]
      ,m.[WhatsappNumber]
      ,m.[Cname]
      ,m.[ClientDoamin]
      ,CU.[Email]
      ,m.[Messenger]
      ,m.[ViberCountry]
      ,m.[ViberNumber]
      ,m.[TelegramCounty]
      ,m.[TelegramNumber]
      ,m.[PhoneCountry]
      ,m.[PhoneNumber]
      ,m.[BusinessStartTime]
      ,m.[BusinessCloseTime]
      ,m.[DoNotDisturbGuestStartTime]
      ,m.[DoNotDisturbGuestEndTime]
      ,m.[StaffAlertsOffduty]
      ,m.[NoMessageToGuestWhileQuiteTime]
      ,m.[IncomingTranslationLangage]
      ,m.[NoTranslateWords]
      ,m.[ProductId]
      ,m.[IsActive]
	  ,CU.[FirstName]
      ,CU.[LastName]
      ,CU.[Title]
      ,CU.[ProfilePicture]
      ,CU.[UserName]
FROM [dbo].[Customers] m 
	INNER JOIN  [dbo].[CustomerUsers] CU
	On m.Id = CU.CustomerId
	Where m.Id = @Id
");
            #endregion

            #region GetCustomerReceptionWithRelationSP
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerReceptionWithRelationSP]    Script Date: 23-05-2023 16:58:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC [dbo].[GetCustomerReceptionWithRelationSP] --'',1,2,'CategoryName','ASC',1
(
    @SearchValue NVARCHAR(50) = null,
    @PageNo Int=1,
	@PageSize Int=10, --NoOf Record To Get
	@SortColumn NVARCHAR(20) = 'DisplayOrder',
    @SortOrder NVARCHAR(5) = 'ASC',
	@CustomerId Int=1
)
AS BEGIN
	SET NOCOUNT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
	
	; WITH Customer_Reception_Results AS
    (
		SELECT 
		( 
			SELECT [Id],[CustomerId],[CustomerGuestAppBuilderId],[CategoryName],[DisplayOrder],
				(SELECT [Id]
					,[CustomerId]
					,[CustomerGuestAppBuilderId]
					,[CustomerGuestAppReceptionCategoryId]
					,[CategoryName]
					,[Name]
					,[ItemsMonth]
					,[ItemsDay]
					,[ItemsMinute]
					,[ItemsHour]
					,[QuantityBar]
					,[PickupLocation]
					,[DestinationLocation]
					,[Comment]
					,[IsPriceEnable]
					,[Price]
					,[Currency]
					,[IsActive]
					,[DisplayOrder]
				FROM [dbo].[CustomerGuestAppReceptionItems] items
				WHERE items.CustomerGuestAppReceptionCategoryId = categories.Id
				ORDER BY DisplayOrder ASC
				FOR JSON PATH) AS CustomerReceptionItems
			FROM [dbo].[CustomerGuestAppReceptionCategories] categories
			WHERE DeletedAt is null AND CustomerId = @CustomerId AND (
			CategoryName LIKE '%' + @SearchValue + '%' )

			ORDER BY
			CASE WHEN (@SortColumn = 'DisplayOrder' AND @SortOrder='ASC')
			THEN DisplayOrder
			END ASC,

			CASE WHEN (@SortColumn = 'DisplayOrder' AND @SortOrder='DESC')
			THEN DisplayOrder
			END DESC,
       
	        CASE WHEN (@SortColumn = 'CategoryName' AND @SortOrder='ASC')
			THEN CategoryName
			END ASC,

			CASE WHEN (@SortColumn = 'CategoryName' AND @SortOrder='DESC')
			THEN CategoryName
			END DESC
			OFFSET @PageSize * (@PageNo - 1) ROWS
			FETCH NEXT @PageSize ROWS ONLY

		FOR JSON PATH) AS CustomerReceptionWithRelationOut 
    )

	SELECT *
	FROM Customer_Reception_Results
	OPTION (RECOMPILE)
END");
			#endregion

			#region GetCustomerHouseKeepingWithRelationSP
			migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerHouseKeepingWithRelationSP]    Script Date: 24-05-2023 11:49:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROC [dbo].[GetCustomerHouseKeepingWithRelationSP] --'',1,2,'CategoryName','ASC',1
(
    @SearchValue NVARCHAR(50) = null,
    @PageNo Int=1,
	@PageSize Int=10, --NoOf Record To Get
	@SortColumn NVARCHAR(20) = 'DisplayOrder',
    @SortOrder NVARCHAR(5) = 'ASC',
	@CustomerId Int=1
)
AS BEGIN
	SET NOCOUNT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
	
	; WITH Customer_House_keeping_Results AS
    (
		SELECT ( 
            SELECT [Id],[CustomerId],[CustomerGuestAppBuilderId],[CategoryName],[DisplayOrder],
				(SELECT [Id]
				  ,[CustomerId]
				  ,[CustomerGuestAppBuilderId]
				  ,[CustomerGuestAppHousekeepingCategoryId]
				  ,[CategoryName]
				  ,[Name]
				  ,[ItemsMonth]
				  ,[ItemsDay]
				  ,[ItemsMinute]
				  ,[ItemsHour]
				  ,[QuantityBar]
				  ,[PickupLocation]
				  ,[DestinationLocation]
				  ,[Comment]
				  ,[IsPriceEnable]
				  ,[Price]
				  ,[Currency]
				  ,[IsActive]
				  ,[DisplayOrder]
                FROM [dbo].[CustomerGuestAppHousekeepingItems] items
                WHERE items.CustomerGuestAppHousekeepingCategoryId = categories.Id
				ORDER BY DisplayOrder ASC
                FOR JSON PATH) AS CustomerHouseKeepingItems
		FROM [dbo].[CustomerGuestAppHousekeepingCategories] categories
		WHERE DeletedAt is null AND CustomerId = @CustomerId AND (CategoryName LIKE '%' + @SearchValue + '%') 

		ORDER BY
		CASE WHEN (@SortColumn = 'DisplayOrder' AND @SortOrder='ASC')
		THEN DisplayOrder
		END ASC,

		CASE WHEN (@SortColumn = 'DisplayOrder' AND @SortOrder='DESC')
		THEN DisplayOrder
		END DESC,

		CASE WHEN (@SortColumn = 'CategoryName' AND @SortOrder='ASC')
        THEN CategoryName
        END ASC,

		CASE WHEN (@SortColumn = 'CategoryName' AND @SortOrder='DESC')
        THEN CategoryName
        END DESC
       
		OFFSET @PageSize * (@PageNo - 1) ROWS
        FETCH NEXT @PageSize ROWS ONLY
		FOR JSON PATH) AS CustomerHouseKeepingWithRelationOut
	)

	SELECT *
	FROM Customer_House_keeping_Results
	OPTION (RECOMPILE)
	
END");
            #endregion

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
