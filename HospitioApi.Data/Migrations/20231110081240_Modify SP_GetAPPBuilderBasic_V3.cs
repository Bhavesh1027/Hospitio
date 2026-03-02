using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModifySP_GetAPPBuilderBasic_V3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER   PROCEDURE [dbo].[GetAPPBuilderBasic] --1,1,''
(
    @RoomId INT = 0,
    @CustomerId INT = 0,
    @UserType NVARCHAR(50) = ''
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF (@UserType = 'Customer')
    BEGIN
        SELECT 
            [Id],
            JSON_VALUE(JsonData, '$.Message') AS [Message],
            JSON_VALUE(JsonData, '$.SecondaryMessage') AS [SecondaryMessage],
            JSON_VALUE(JsonData, '$.IsActive') AS [IsActive],
            JSON_VALUE(JsonData, '$.LocalExperience') AS [LocalExperience],
            JSON_VALUE(JsonData, '$.Ekey') AS [Ekey],
            JSON_VALUE(JsonData, '$.PropertyInfo') AS [PropertyInfo],
            JSON_VALUE(JsonData, '$.EnhanceYourStay') AS [EnhanceYourStay],
            JSON_VALUE(JsonData, '$.Reception') AS [Reception],
            JSON_VALUE(JsonData, '$.Housekeeping') AS [Housekeeping],
            JSON_VALUE(JsonData, '$.RoomService') AS [RoomService],
            JSON_VALUE(JsonData, '$.Concierge') AS [Concierge],
            JSON_VALUE(JsonData, '$.TransferServices') AS [TransferServices]
        FROM [dbo].[CustomerGuestAppBuilders] C1 (NOLOCK)
        WHERE [C1].[DeletedAt] IS NULL
              AND [C1].[CustomerRoomNameId] = @RoomId
              AND [C1].[CustomerId] = @CustomerId
              AND ISJSON(JsonData) = 1
        UNION ALL

        SELECT [Id]
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
        FROM [dbo].[CustomerGuestAppBuilders] C2 (NOLOCK)
        WHERE [C2].[DeletedAt] IS NULL
              AND [C2].[CustomerRoomNameId] = @RoomId
              AND [C2].[CustomerId] = @CustomerId
              AND [C2].[JsonData] IS NULL
    END
    ELSE
    BEGIN
        SELECT [Id]
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
        FROM [dbo].[CustomerGuestAppBuilders] C2 (NOLOCK)
        WHERE [C2].[DeletedAt] IS NULL
              AND [C2].[CustomerRoomNameId] = @RoomId
              AND [C2].[CustomerId] = @CustomerId
    END
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
