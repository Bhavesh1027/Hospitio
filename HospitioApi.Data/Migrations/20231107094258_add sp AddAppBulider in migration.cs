using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class addspAddAppBuliderinmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" CREATE OR  ALTER     PROCEDURE [dbo].[AddAppBulider]
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
                                          ,[IsActive]
                                    	  ,[IsWork])
                                    	   Values
                                    	   (@CustomerId , @RoomId,null,null,0,0,0,0,0,0,0,0,0,1,3)
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
                                        ""name"": ""TransferService"",
                                        ""IsDisable"": true,
                                        ""DisplayOrder"": 2,
                                        ""Images"": """",
                                        ""Items"": 0,
                                        ""Categorie"": 0
                                      },
                                      {
                                        ""name"": ""e-Keys"",
                                        ""IsDisable"": true,
                                        ""DisplayOrder"": 3,
                                        ""Images"": """",
                                        ""Items"": 0,
                                        ""Categorie"": 0
                                      },
                                      {
                                        ""name"": ""Property Info"",
                                        ""IsDisable"": true,
                                        ""DisplayOrder"": 4,
                                        ""Images"": """",
                                        ""Items"": 0,
                                        ""Categorie"": 0
                                      },
                                      {
                                        ""name"": ""Enhance your Stay"",
                                        ""IsDisable"": true,
                                        ""DisplayOrder"": 5,
                                        ""Images"": """",
                                        ""Items"": 0,
                                        ""Categorie"": 0
                                      },
                                      {
                                        ""name"": ""Reception"",
                                        ""IsDisable"": true,
                                        ""DisplayOrder"": 6,
                                        ""Images"": """",
                                        ""Items"": 0,
                                        ""Categorie"": 0
                                      },
                                      {
                                        ""name"": ""Housekeeping"",
                                        ""IsDisable"": true,
                                        ""DisplayOrder"": 7,
                                        ""Images"": """",
                                        ""Items"": 0,
                                        ""Categorie"": 0
                                      },
                                      {
                                        ""name"": ""Room Service"",
                                        ""IsDisable"": true,
                                        ""DisplayOrder"": 8,
                                        ""Images"": """",
                                        ""Items"": 0,
                                        ""Categorie"": 0
                                      },
                                      {
                                        ""name"": ""Concierge"",
                                        ""IsDisable"": true,
                                        ""DisplayOrder"": 9,
                                        ""Images"": """",
                                        ""Items"": 0,
                                        ""Categorie"": 0
                                      },
									  {
                                        ""name"": ""Online Check-In"",
                                        ""IsDisable"": true,
                                        ""DisplayOrder"": 10,
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
                                    Where (M.Id = 6  OR M.Id = 4)
                                    ANd C.Id = @CustomerId
                                    AND PM.IsActive = 1
                                    	END
                                    
                                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
