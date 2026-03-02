using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // Customer SP 
            migrationBuilder.Sql(@"
GO
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
FOR JSON PATH )
");

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
		dbo.Products ON dbo.Customers.ServicePackageId = dbo.Products.Id LEFT OUTER JOIN
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
      ,m.[ServicePackageId]
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

            // Get BusinessType SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetBusinessTypes]    Script Date: 03-05-2023 18:57:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER Procedure [dbo].[GetBusinessTypes]

as
select * from BusinessTypes where DeletedAt is null");

            // Get CustomerGuestAlert SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerGuestAlertById]    Script Date: 03-05-2023 18:59:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetCustomerGuestAlertById]

@Id int=1
as
select * from CustomerGuestAlerts where 
Id = @Id and
DeletedAt is null ");

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerGuestAlerts]    Script Date: 03-05-2023 19:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetCustomerGuestAlerts]
@CustomerId int =1
as
select * from CustomerGuestAlerts 
where DeletedAt is null and CustomerId = @CustomerId");

            // Get CustomerPaymentProcessor SP 


            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPaymentProcessorByID]    Script Date: 03-05-2023 19:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER Procedure [dbo].[GetCustomerPaymentProcessorByID] @Id int=1
As 
Select * from CustomerPaymentProcessors where Id = @Id  and DeletedAt is null
");
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerPaymentProcessors]    Script Date: 03-05-2023 19:06:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPaymentProcessors] 
    @CustomerId int=1,
	@PageNo Int=1,
	@PageSize Int=10 --NoOf Record To Get
	
   AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	 
	SELECT [Id], [PaymentProcessorId],[CustomerId], [ClientId],[ClientSecret],[Currency], [IsActive],
	COUNT(*) OVER() AS TotalCount 
	FROM [dbo].[CustomerPaymentProcessors] WITH (NOLOCK)
	WHERE  DeletedAt is null and CustomerId =@CustomerId
	order by PaymentProcessorId
	OffSet @PageSize * (@PageNo-1) Rows
	Fetch Next @PageSize Rows Only");

            // Get CustomerStaffAlerts SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerStaffAlertsByCustomerId]    Script Date: 03-05-2023 19:06:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetCustomerStaffAlertsByCustomerId]
	@CustomerId int=1
	
	
   AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	SELECT * FROM CustomerStaffAlerts 
	WHERE CustomerId = @CustomerId
	AND DeletedAt IS NULL");

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetCustomerStaffAlertsById]    Script Date: 03-05-2023 19:07:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetCustomerStaffAlertsById] @Id int =1

as
select * from CustomerStaffAlerts where Id = @Id 
and DeletedAt is null ");

            // Get Group SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGroups]    Script Date: 03-05-2023 19:09:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER Procedure [dbo].[GetGroups]

as
select * from Groups where DeletedAt is null ");



            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetGroupById]    Script Date: 15-05-2023 12:22:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetGroupById]
@Id int=0
as
select * from Groups 
where DeletedAt is null 
and Id = @Id");
            // Get GuestJourneyMessagesTemplates SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGuestJourneyMessagesTemplates]    Script Date: 03-05-2023 19:10:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetGuestJourneyMessagesTemplates]

as
select * from GuestJourneyMessagesTemplates
where DeletedAt is null");

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGuestJourneyMessagesTemplatesById]    Script Date: 03-05-2023 19:10:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetGuestJourneyMessagesTemplatesById]
@Id int=1
as
select * from GuestJourneyMessagesTemplates
where DeletedAt is null and
Id = @Id");

            // Get PaymentProcessor SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetPaymentProcessorById]    Script Date: 03-05-2023 19:12:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetPaymentProcessorById]
@Id int=1
as
select * from PaymentProcessors where DeletedAt is null 
and Id =@Id");

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetPaymentProcessors]    Script Date: 03-05-2023 19:13:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetPaymentProcessors]

as
select * from PaymentProcessors where DeletedAt is null ");

            // Get QaCategories SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetQaCategories]    Script Date: 03-05-2023 19:14:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetQaCategories]

as
select * from QuestionAnswerCategories where DeletedAt is null");

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetQaCategoryById]    Script Date: 03-05-2023 19:15:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetQaCategoryById]
@Id int = 1
as
select * from QuestionAnswerCategories where DeletedAt is null
and Id = @Id");

            // Get HospitioPaymentProcessor SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetHospitioPaymentProcessorById]    Script Date: 03-05-2023 19:15:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetHospitioPaymentProcessorById]
@Id int = 1
as
select * from HospitioPaymentProcessors where DeletedAt is null
and Id = @Id");


            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetHospitioPaymentProcessors]    Script Date: 03-05-2023 19:16:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	CREATE OR ALTER PROCEDURE [dbo].[GetHospitioPaymentProcessors] 
    
	@PageNo Int=1,
	@PageSize Int=10 --NoOf Record To Get
	
   AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	 
	SELECT [Id], [PaymentProcessorId], [ClientId],[ClientSecret],[Currency], [IsActive],
	COUNT(*) OVER() AS TotalCount 
	FROM [dbo].[HospitioPaymentProcessors] WITH (NOLOCK)
	WHERE  DeletedAt is null
	order by PaymentProcessorId
	OffSet @PageSize * (@PageNo-1) Rows
	Fetch Next @PageSize Rows Only");

            // Get Modules SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetModules]    Script Date: 03-05-2023 19:17:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER Procedure [dbo].[GetModules]

as
select * from Modules where DeletedAt is null ");


            // Get ModuleServices SP

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetModuleServices]    Script Date: 03-05-2023 19:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetModuleServices]

as
select * from ModuleServices where DeletedAt is null ");

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetModuleServicesById]    Script Date: 03-05-2023 19:18:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetModuleServicesById]
@Id int=1
as
select * from ModuleServices where DeletedAt is null 
and Id = @Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
