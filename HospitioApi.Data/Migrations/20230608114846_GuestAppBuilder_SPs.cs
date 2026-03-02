using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GuestAppBuilder_SPs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region Appbuilder SPs - GetExtradetails
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetExtradetails]    Script Date: 08-06-2023 17:14:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetExtradetails] 
(
@CustomerAppBuliderId int = 0,
@CustomerId int = 0,
@GuestService NVARCHAR(100) = ''
)
as
select
Case
When @GuestService='Reception' Then (Select Count(*) from CustomerGuestAppReceptionItems where CustomerGuestAppBuilderId = @CustomerAppBuliderId)
WHEN @GuestService = 'Enhance your Stay' Then (Select Count(*) from CustomerGuestAppEnhanceYourStayItems where CustomerGuestAppBuilderId = @CustomerAppBuliderId)
WHEN @GuestService = 'Room Service' Then (Select Count(*) from CustomerGuestAppRoomServiceItems where CustomerGuestAppBuilderId = @CustomerAppBuliderId)
WHEN @GuestService = 'Concierge' Then (Select Count(*) from CustomerGuestAppConciergeItems where CustomerGuestAppBuilderId = @CustomerAppBuliderId)
WHEN @GuestService = 'Housekeeping' Then (Select Count(*) from CustomerGuestAppHousekeepingItems where CustomerGuestAppBuilderId = @CustomerAppBuliderId)
End as Items
,
Case
When @GuestService='Reception' Then (Select Count(*) from CustomerGuestAppReceptionCategories where CustomerGuestAppBuilderId = @CustomerAppBuliderId)
WHEN @GuestService = 'Enhance your Stay' Then (Select Count(*) from CustomerGuestAppEnhanceYourStayCategories where CustomerGuestAppBuilderId = @CustomerAppBuliderId)
WHEN @GuestService = 'Room Service' Then (Select Count(*) from CustomerGuestAppRoomServiceCategories where CustomerGuestAppBuilderId = @CustomerAppBuliderId)
WHEN @GuestService = 'Concierge' Then (Select Count(*) from CustomerGuestAppConciergeCategories where CustomerGuestAppBuilderId = @CustomerAppBuliderId)
WHEN @GuestService = 'Housekeeping' Then (Select Count(*) from CustomerGuestAppHousekeepingCategories where CustomerGuestAppBuilderId = @CustomerAppBuliderId)
End as Categories


from dbo.CustomerGuestAppBuilders
where DeletedAt is null 
AND dbo.CustomerGuestAppBuilders.Id = @CustomerAppBuliderId
AND dbo.CustomerGuestAppBuilders.CustomerId = @CustomerId");
            #endregion

            #region Appbuilder SPs - Get APP Builder

            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[AddAppBulider]    Script Date: 09-06-2023 17:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[AddAppBulider]
(
	@RoomId int = 0,
	@CustomerId int  = 0 
)
AS BEGIN
	SET NOCOUNT ON;
	Declare @RoomNameId int
	Declare @CustomerAppBuliderId int
	Declare @DisplayOrder int 

	Select @RoomNameId = COUNT(CustomerRoomNameId)
	From CustomerGuestAppBuilders With (NOLOCK)
	Where CustomerId = @CustomerId And
		CustomerRoomNameId = @RoomId

	

	IF(@RoomNameId = 0)
	BEGIN 
		INSERT INTO CustomerGuestAppBuilders (
	   [CustomerId]
      ,[CustomerRoomNameId]
      ,[Message]
      ,[SecondaryMessage]
      ,[LocalExperience]
      ,[Ekey]
      ,[PropertyInfo]
      ,[EnhanceYourStay]
      ,[Reception]
      ,[Housekeeping]
      ,[RoomService]
      ,[Concierge]
      ,[TransferServices]
      ,[IsActive])
	   Values
	   (@CustomerId , @RoomId,null,null,0,0,0,0,0,0,0,0,0,0)
	END
	Select @CustomerAppBuliderId=Id 
	From CustomerGuestAppBuilders With (NOLOCK)
	Where CustomerId = @CustomerId And
		CustomerRoomNameId = @RoomId

	Select @DisplayOrder = COUNT(RefrenceId)
	From ScreenDisplayOrderAndStatuses With (NOLOCK)
	where RefrenceId = @CustomerAppBuliderId
	AND ScreenName = 2
	IF(@DisplayOrder = 0)
	BEGIN
		INSERT INTO ScreenDisplayOrderAndStatuses (ScreenName,JsonData,RefrenceId,IsActive)
		values(2,'[
  {
    ""name"": ""Local Experiences"",
    ""IsDisable"": true,
    ""DisplayOrder"": 1,
    ""Images"": """",
    ""Items"": 0,
    ""Categorie"": 0
  },
  {
    ""name"": ""e-Keys"",
    ""IsDisable"": true,
    ""DisplayOrder"": 2,
    ""Images"": """",
    ""Items"": 0,
    ""Categorie"": 0
  },
  {
    ""name"": ""Property Info"",
    ""IsDisable"": true,
    ""DisplayOrder"": 3,
    ""Images"": """",
    ""Items"": 0,
    ""Categorie"": 0
  },
  {
    ""name"": ""Enhance your Stay"",
    ""IsDisable"": true,
    ""DisplayOrder"": 4,
    ""Images"": """",
    ""Items"": 0,
    ""Categorie"": 0
  },
  {
    ""name"": ""Reception"",
    ""IsDisable"": true,
    ""DisplayOrder"": 5,
    ""Images"": """",
    ""Items"": 0,
    ""Categorie"": 0
  },
  {
    ""name"": ""Housekeeping"",
    ""IsDisable"": true,
    ""DisplayOrder"": 6,
    ""Images"": """",
    ""Items"": 0,
    ""Categorie"": 0
  },
  {
    ""name"": ""Room Service"",
    ""IsDisable"": true,
    ""DisplayOrder"": 7,
    ""Images"": """",
    ""Items"": 0,
    ""Categorie"": 0
  },
  {
    ""name"": ""Concierge"",
    ""IsDisable"": true,
    ""DisplayOrder"": 8,
    ""Images"": """",
    ""Items"": 0,
    ""Categorie"": 0
  },
  {
    ""name"": ""TransferService"",
    ""IsDisable"": true,
    ""DisplayOrder"": 9,
    ""Images"": """",
    ""Items"": 0,
    ""Categorie"": 0
  }
]',@CustomerAppBuliderId,1)
	END

	BEGIN
	
select MS.Name, 0 as IsModule,@CustomerAppBuliderId as CustomerAppBuliderId from dbo.Customers C 
INNER JOIN dbo.Products P
ON C.ProductId = P.Id
INNER JOIN dbo.ProductModules PM
ON p.Id = PM.ProductId
INNER JOIN dbo.ProductModuleServices PMS
ON PM.Id = PMS.ProductModuleId
Inner Join dbo.ModuleServices MS
ON Ms.Id = PMS.ModuleServiceId
Where PM.ModuleId = 5 -- Guest Portal Defult
ANd C.Id = @CustomerId
AND PMS.IsActive = 1
union all
select m.Name,1 as IsModule,@CustomerAppBuliderId as CustomerAppBuliderId from dbo.Customers C 
INNER JOIN dbo.Products P
ON C.ProductId = P.Id
INNER JOIN dbo.ProductModules PM
ON p.Id = PM.ProductId
INNER JOIN dbo.Modules M
ON M.Id = PM.ModuleId
Where M.Id = 6 ANd 
C.Id = @CustomerId
AND PM.IsActive = 1
	END

END");
            #endregion

            #region  Get Basic Details
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetAPPBuilderBasic]    Script Date: 08-06-2023 17:21:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER Procedure [dbo].[GetAPPBuilderBasic]
(
@RoomId int = 0,
@CustomerId int =0
)
as
SELECT
       [Id]
      ,[Message]
      ,[SecondaryMessage]   
	  ,[IsActive]
	  ,[LocalExperience]
      ,[Ekey]
      ,[PropertyInfo]
      ,[EnhanceYourStay]
      ,[Reception]
      ,[Housekeeping]
      ,[RoomService]
      ,[Concierge]
      ,[TransferServices]
  FROM [dbo].[CustomerGuestAppBuilders]

  Where dbo.CustomerGuestAppBuilders.CustomerRoomNameId = @RoomId
  AND dbo.CustomerGuestAppBuilders.CustomerId = @CustomerId");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
