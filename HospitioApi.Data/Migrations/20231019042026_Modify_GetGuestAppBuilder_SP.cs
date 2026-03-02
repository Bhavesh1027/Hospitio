using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Modify_GetGuestAppBuilder_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetGuestAppBuilder
            migrationBuilder.Sql(@"GO
/****** Object:  StoredProcedure [dbo].[GetGuestAppBuilder]    Script Date: 19/10/2023 9:42:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetGuestAppBuilder] 
(
	@BuilderId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
		SELECT [Id]
		      ,[CustomerId]
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
              ,[IsWork]
			  ,
		      JSON_QUERY(
							(
								  SELECT [Id]
                                        ,[ScreenName]
                                        ,[JsonData]
                                        ,[RefrenceId]
                                        ,[IsActive] 
								  FROM ScreenDisplayOrderAndStatuses  as DSP
                                  WHERE 
									 [DSP].[RefrenceId] = @BuilderId
                                     AND [DSP].ScreenName = 2
									 AND [DSP].[DeletedAt] IS NULL 
								  FOR JSON PATH
										)
			                   ) as [DisplayOrderForGuestBuilder],
		JSON_QUERY(
                   (
			       SELECT [Id]
							,[CustomerId]
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
							,[Country]
							,[Longitude]
							,[Latitude]
							,[IsActive]					
							,[IsPublish],
							JSON_QUERY(
							(
								  SELECT [Id]
                                        ,[ScreenName]
                                        ,[JsonData]
                                        ,[RefrenceId]
                                        ,[IsActive] 
								  FROM ScreenDisplayOrderAndStatuses  as DSP
                                  WHERE 
									 [DSP].[RefrenceId] = [CPI].[Id]
                                     AND [DSP].ScreenName = 1
									 AND [DSP].[DeletedAt] IS NULL 
								  FOR JSON PATH
										)
			                   ) as [DisplayOrderForPropertyInfo]
			              ,JSON_QUERY(
			              (
			                  SELECT [Id]
									,[CustomerPropertyInformationId]
									,[Name]
									,[Icon]
									,[Description]
									,[IsActive]							
									,[IsPublish]
									,JSON_QUERY(
										(
										    SELECT [Id]
												,[CustomerPropertyServiceId]
												,[ServiceImages]
												,[IsActive]										
												,[IsPublish]
										    FROM [dbo].[CustomerPropertyServiceImages] CPSI
										    WHERE [CPSI].[CustomerPropertyServiceId] = [CPS].[Id]
										          AND [CPSI].[DeletedAt] IS NULL
										    FOR JSON PATH
										)
			                        ) as [CustomerPropertyServiceImages]
			                  FROM [dbo].[CustomerPropertyServices] CPS
			                  WHERE [CPS].[CustomerPropertyInformationId] = [CPI].[Id]
			                        AND [CPS].[DeletedAt] IS NULL
			                  FOR JSON PATH
			              )
			                 ) as [CustomerPropertyServices],
						   JSON_QUERY(
							(
								  SELECT [Id]
                                        ,[CustomerPropertyInformationId]
                                        ,[Name]
                                        ,[PhoneNumber]
                                        ,[IsActive]
                                        ,[IsPublish]
										,[DisplayOrder]
								  FROM [dbo].[CustomerPropertyEmergencyNumbers] CEN
								  WHERE CEN.[CustomerPropertyInformationId] = [CPI].[Id]
								        AND [CEN].[DeletedAt] IS NULL
								  FOR JSON PATH
										)
			                ) as [CustomerPropertyEmergencyNo],
							JSON_QUERY(
							(
								  SELECT [Id]
                                        ,[CustomerPropertyInformationId]
                                        ,[PropertyImage]
                                        ,[IsActive]
                                        ,[IsPublish]
								  FROM [dbo].[CustomerPropertyGalleries] CEGL
								  WHERE CEGL.[CustomerPropertyInformationId] = [CPI].[Id]
								        AND [CEGL].[DeletedAt] IS NULL
								  FOR JSON PATH
										)
			                ) as [CustomerPropertyGallery],
							 JSON_QUERY(
			              (
			                  SELECT  [Id]
										,[CustomerPropertyInformationId]
										,[ExtraType]
										,[Name]
										,[IsActive]								
										,[IsPublish]
										,[DisplayOrder]
										,
									JSON_QUERY(
										(
										    SELECT [Id]
													,[CustomerPropertyExtraId]
													,[Description]
													,[Latitude]
													,[Longitude]
													,[IsActive]											
													,[IsPublish]
										    FROM [dbo].[CustomerPropertyExtraDetails] CPED
										    WHERE CPED.[CustomerPropertyExtraId] = CPE.[Id]
										          AND [CPED].[DeletedAt] IS NULL
										    FOR JSON PATH
										)
			                        ) as [CustomerPropertyExtraDetails]
			                  FROM [dbo].[CustomerPropertyExtras] CPE
			                  WHERE CPE.[CustomerPropertyInformationId] = [CPI].[Id]
			                        AND CPE.[DeletedAt] IS NULL
			                  FOR JSON PATH
			              )
			                 ) as [CustomerPropertyExtras]
			       FROM [dbo].[CustomerPropertyInformations] CPI (NOLOCK)
			       WHERE [CPI].[DeletedAt] IS NULL
					AND [CPI].CustomerGuestAppBuilderId = @BuilderId
			       FOR JSON PATH
				    )
			          ) as [CustomerPropertyinfo],
			JSON_QUERY(
                   (
			       SELECT [Id]
							,[CustomerGuestAppBuilderId]
							,[CustomerId]
							,[CategoryName]
							,[IsActive]							
							,[DisplayOrder]
							,[IsPublish]
							,
			              JSON_QUERY(
			              (
			                  SELECT [Id]
										,[CustomerGuestAppBuilderId]
										,[CustomerId]
										,[CustomerGuestAppBuilderCategoryId]
										,[Badge]
										,[ShortDescription]
										,[LongDescription]
										,[ButtonType]
										,[ButtonText]
										,[ChargeType]
										,[Discount]
										,[Price]
										,[Currency]
										,[IsActive]									
										,[DisplayOrder]
										,[IsPublish]
									, 
									JSON_QUERY(
										(
										    SELECT [Id]
													,[CustomerGuestAppEnhanceYourStayItemId]
													,[ItemsImages]
													,[DisaplayOrder]
													,[IsActive]
													,[IsPublish]
										    FROM [dbo].[CustomerGuestAppEnhanceYourStayItemsImages] CGEII
										    WHERE CGEII.[CustomerGuestAppEnhanceYourStayItemId] = CGEI.[Id]
										          AND CGEII.[DeletedAt] IS NULL
										    FOR JSON PATH
										)
			                        ) as [CustomerGuestAppEnhanceYourStayItemsImages]
									, 
									JSON_QUERY(
										(
										    SELECT [Id]
													,[CustomerGuestAppEnhanceYourStayItemId]
													,[QueType]
													,[Questions]
													,[OptionValues]
													,[IsActive]
													,[IsPublish]
										    FROM [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras] CGEIE
										    WHERE CGEIE.[CustomerGuestAppEnhanceYourStayItemId] = CGEI.[Id]
										          AND CGEIE.[DeletedAt] IS NULL
										    FOR JSON PATH
										)
			                        ) as [CustomerGuestAppEnhanceYourStayCategoryItemsExtras]
			                  FROM [dbo].[CustomerGuestAppEnhanceYourStayItems] CGEI
			                  WHERE CGEI.[CustomerGuestAppBuilderCategoryId] = CGEC.[Id]
			                        AND CGEI.[DeletedAt] IS NULL
			                  FOR JSON PATH
			              )
			                 ) as [CustomerGuestAppEnhanceYourStayItems]						  
			       FROM [dbo].[CustomerGuestAppEnhanceYourStayCategories] CGEC (NOLOCK)
			       WHERE CGEC.[DeletedAt] IS NULL
					AND CGEC.CustomerGuestAppBuilderId = @BuilderId
			       FOR JSON PATH
				    )
			          ) as [CustomerGuestAppEnhanceYourStayCategories]
					  ,
				JSON_QUERY(
					(
						SELECT
						    [CC].[Id],
						    [CC].[CustomerGuestAppBuilderId],
						    [CC].[CustomerId],
						    [CC].[CategoryName],
						    [CC].[IsActive],
						    [CC].[DisplayOrder],
						    [CC].[IsPublish],
						    [CC].[JsonData],
						    JSON_QUERY((
						        SELECT
						            [CI].[Id],
						            [CI].[CustomerId],
						            [CI].[CustomerGuestAppBuilderId],
						            [CI].[CustomerGuestAppConciergeCategoryId],
						            [CI].[Name],
						            [CI].[ItemsMonth],
						            [CI].[ItemsDay],
						            [CI].[ItemsMinute],
						            [CI].[ItemsHour],
						            [CI].[QuantityBar],
						            [CI].[ItemLocation],
						            [CI].[Comment],
						            [CI].[IsPriceEnable],
						            [CI].[Price],
						            [CI].[Currency],
						            [CI].[IsActive],
						            [CI].[DisplayOrder],
						            [CI].[IsPublish]
						        FROM [dbo].[CustomerGuestAppConciergeItems] AS [CI]
						        WHERE [CI].[CustomerGuestAppConciergeCategoryId] = [CC].[Id]
								AND [CI].[DeletedAt] IS NULL
 						        FOR JSON PATH
						    )) AS [Conciergeitem]
						FROM [dbo].[CustomerGuestAppConciergeCategories] AS [CC]
						WHERE [CC].[DeletedAt] IS NULL
						AND [CC].[CustomerGuestAppBuilderId] = @BuilderId
						FOR JSON PATH
					)
						) as [CustomerGuestAppConciergeCategories],
				JSON_QUERY(
					(
					SELECT
						    [RCC].[Id],
						    [RCC].[CustomerGuestAppBuilderId],
						    [RCC].[CustomerId],
						    [RCC].[CategoryName],
						    [RCC].[IsActive],
						    [RCC].[DisplayOrder],
						    [RCC].[IsPublish],
						    JSON_QUERY((
						        SELECT
						            [RCI].[Id],
						            [RCI].[CustomerId],
						            [RCI].[CustomerGuestAppBuilderId],
						            [RCI].[CustomerGuestAppReceptionCategoryId],
						            [RCI].[Name],
						            [RCI].[ItemsMonth],
						            [RCI].[ItemsDay],
						            [RCI].[ItemsMinute],
						            [RCI].[ItemsHour],
						            [RCI].[QuantityBar],
						            [RCI].[ItemLocation],
						            [RCI].[Comment],
						            [RCI].[IsPriceEnable],
						            [RCI].[Price],
						            [RCI].[Currency],
						            [RCI].[IsActive],
						            [RCI].[DisplayOrder],
						            [RCI].[IsPublish]
						        FROM [dbo].[CustomerGuestAppReceptionItems] AS [RCI]
						        WHERE [RCI].[CustomerGuestAppReceptionCategoryId] = [RCC].[Id]
								AND [RCI].[DeletedAt] IS NULL
						        FOR JSON PATH
						    )) AS [ReceptionItem]
						FROM [dbo].[CustomerGuestAppReceptionCategories] AS [RCC]
						WHERE [RCC].[DeletedAt] IS NULL
						AND [RCC].[CustomerGuestAppBuilderId] = @BuilderId
						FOR JSON PATH

					)) as [CustomerGuestAppReceptionCategories],
				JSON_QUERY(
					(
						SELECT
						    [HC].[Id],
						    [HC].[CustomerGuestAppBuilderId],
						    [HC].[CustomerId],
						    [HC].[CategoryName],
						    [HC].[IsActive],
						    [HC].[DisplayOrder],
						    [HC].[IsPublish],
						    JSON_QUERY((
						        SELECT
						            [HI].[Id],
						            [HI].[CustomerId],
						            [HI].[CustomerGuestAppBuilderId],
						            [HI].[CustomerGuestAppHousekeepingCategoryId],
						            [HI].[Name],
						            [HI].[ItemsMonth],
						            [HI].[ItemsDay],
						            [HI].[ItemsMinute],
						            [HI].[ItemsHour],
						            [HI].[QuantityBar],
						            [HI].[ItemLocation],
						            [HI].[Comment],
						            [HI].[IsPriceEnable],
						            [HI].[Price],
						            [HI].[Currency],
						            [HI].[IsActive],
						            [HI].[DisplayOrder],
						            [HI].[IsPublish]
						        FROM [dbo].[CustomerGuestAppHousekeepingItems] AS [HI]
						        WHERE [HI].[CustomerGuestAppHousekeepingCategoryId] = [HC].[Id]
								AND [HI].[DeletedAt] IS NULL
						        FOR JSON PATH
						    )) AS [HouseItem]
						FROM [dbo].[CustomerGuestAppHousekeepingCategories] AS [HC]
						WHERE [HC].[DeletedAt] IS NULL
						AND [HC].[CustomerGuestAppBuilderId] = @BuilderId
						FOR JSON PATH
					)) as [CustomerGuestAppHousekeepingCategories],
					JSON_QUERY(
					(
						SELECT
						    [RC].[Id],
						    [RC].[CustomerGuestAppBuilderId],
						    [RC].[CustomerId],
						    [RC].[CategoryName],
						    [RC].[IsActive],
						    [RC].[DisplayOrder],
						    [RC].[IsPublish],
						    JSON_QUERY((
						        SELECT
						            [RI].[Id],
						            [RI].[CustomerId],
						            [RI].[CustomerGuestAppBuilderId],
						            [RI].[CustomerGuestAppRoomServiceCategoryId],
						            [RI].[Name],
						            [RI].[ItemsMonth],
						            [RI].[ItemsDay],
						            [RI].[ItemsMinute],
						            [RI].[ItemsHour],
						            [RI].[QuantityBar],
						            [RI].[ItemLocation],
						            [RI].[Comment],
						            [RI].[IsPriceEnable],
						            [RI].[Price],
						            [RI].[Currency],
						            [RI].[IsActive],
						            [RI].[DisplayOrder],
						            [RI].[IsPublish]
						        FROM [dbo].[CustomerGuestAppRoomServiceItems] AS [RI]
						        WHERE [RI].[CustomerGuestAppRoomServiceCategoryId] = [RC].[Id]
								AND [RI].[DeletedAt] IS NULL
						        FOR JSON PATH
						    )) AS [RoomItem]
						FROM [dbo].[CustomerGuestAppRoomServiceCategories] AS [RC]
						WHERE [RC].[DeletedAt] IS NULL
						AND [RC].[CustomerGuestAppBuilderId] = @BuilderId
						FOR JSON PATH
					)) as [CustomerGuestAppRoomServiceCategories]
				From [dbo].[CustomerGuestAppBuilders] CGA (NOLOCK)
				WHERE [CGA].[DeletedAt] IS NULL
				AND [CGA].[Id] = @BuilderId
				 FOR JSON PATH
        ) 
END");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
