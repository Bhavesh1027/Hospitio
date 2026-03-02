using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class SP_Modification_Improvement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetAPPBuilderBasic 
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetAPPBuilderBasic]
(
    @RoomId INT = 0,
    @CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

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
    FROM [dbo].[CustomerGuestAppBuilders] (NOLOCK)
    WHERE [dbo].[CustomerGuestAppBuilders].[DeletedAt] IS NULL
          AND [dbo].[CustomerGuestAppBuilders].[CustomerRoomNameId] = @RoomId
          AND [dbo].[CustomerGuestAppBuilders].[CustomerId] = @CustomerId
END");
            #endregion

            #region GetBusinessTypes
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetBusinessTypes]
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

    SELECT [Id],
           [BizType],
           [IsActive]
    FROM [dbo].[BusinessTypes](NOLOCK)
    WHERE [DeletedAt] IS NULL
END");
            #endregion

            #region GetCustomerConciergeWithRelationSP
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomerConciergeWithRelationSP]
(
    @AppBuilderId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    WITH Customer_Concierge_Results
    AS (SELECT
            (
                SELECT [Id],
                       [CustomerId],
                       [CustomerGuestAppBuilderId],
                       [CategoryName],
                       [DisplayOrder],
                       [IsActive],
                       (
                           SELECT [Id],
                                  [CustomerId],
                                  [CustomerGuestAppBuilderId],
                                  [CustomerGuestAppConciergeCategoryId],
                                  [CategoryName],
                                  [Name],
                                  [ItemsMonth],
                                  [ItemsDay],
                                  [ItemsMinute],
                                  [ItemsHour],
                                  [QuantityBar],
                                  [ItemLocation],
                                  [Comment],
                                  [IsPriceEnable],
                                  [Price],
                                  [Currency],
                                  [IsActive],
                                  [DisplayOrder]
                           FROM [dbo].[CustomerGuestAppConciergeItems] items (NOLOCK)
                           WHERE [items].[CustomerGuestAppConciergeCategoryId] = [categories].[Id]
                                 AND [items].[DeletedAt] IS NULL
                           ORDER BY [DisplayOrder] ASC
                           FOR JSON PATH
                       ) AS [CustomerConciergeItems]
                FROM [dbo].[CustomerGuestAppConciergeCategories] categories (NOLOCK)
                WHERE [DeletedAt] IS NULL
                      AND [CustomerGuestAppBuilderId] = @AppBuilderId
                ORDER BY [DisplayOrder] ASC
                FOR JSON PATH
            ) AS [CustomerConciergeWithRelationOut]
       )
    SELECT * FROM Customer_Concierge_Results
    OPTION (RECOMPILE)
END");
            #endregion

            #region GetCustomerDigitalAssistants
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerDigitalAssistants] 
(
	@CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [dbo].[CustomerDigitalAssistants].[Id],
           [dbo].[CustomerDigitalAssistants].[CustomerId],
           [dbo].[CustomerDigitalAssistants].[NAME],
           [dbo].[CustomerDigitalAssistants].[Details],
           [dbo].[CustomerDigitalAssistants].[Icon],
           [dbo].[CustomerDigitalAssistants].[IsActive]
    FROM [dbo].[CustomerDigitalAssistants] WITH (NOLOCK)
    WHERE [dbo].[CustomerDigitalAssistants].[DeletedAt] IS NULL
          AND [dbo].[CustomerDigitalAssistants].[CustomerId] = @CustomerId
END");
            #endregion

            #region GetCustomerForHospitioAdmin
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerForHospitioAdmin] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [M].[Id],
           [M].[BusinessName],
           [M].[BusinessTypeId],
           [M].[NoOfRooms],
           [M].[TimeZone],
           [M].[WhatsappCountry],
           [M].[WhatsappNumber],
           [M].[Cname],
           [M].[ClientDoamin],
           [CU].[Email],
           [M].[Messenger],
           [M].[ViberCountry],
           [M].[ViberNumber],
           [M].[TelegramCounty],
           [M].[TelegramNumber],
           [M].[PhoneCountry],
           [M].[PhoneNumber],
           [M].[BusinessStartTime],
           [M].[BusinessCloseTime],
           [M].[DoNotDisturbGuestStartTime],
           [M].[DoNotDisturbGuestEndTime],
           [M].[StaffAlertsOffduty],
           [M].[NoMessageToGuestWhileQuiteTime],
           [M].[IncomingTranslationLangage],
           [M].[NoTranslateWords],
           [M].[ProductId],
           [M].[IsActive],
           [CU].[FirstName],
           [CU].[LastName],
           [CU].[Title],
           [CU].[ProfilePicture],
           [CU].[UserName]
    FROM [dbo].[Customers] M (NOLOCK)
        INNER JOIN [dbo].[CustomerUsers] CU (NOLOCK)
            ON [M].[Id] = [CU].[CustomerId]
               AND [CU].[DeletedAt] IS NULL
    WHERE [M].[DeletedAt] IS NULL
          AND [M].[Id] = @Id
END");
            #endregion

            #region GetCustomerGuestAlertById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuestAlertById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerId],
           [OfficeHoursMsg],
           [OfficeHoursMsgWaitTimeInMinutes],
           [OfflineHourMsg],
           [OfflineHoursMsgWaitTimeInMinutes],
           [ReplyAtDiffPeriod],
           [IsActive]
    FROM [dbo].[CustomerGuestAlerts] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetCustomerGuestAlerts
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuestAlerts] 
(
	@CustomerId INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
	SET XACT_ABORT ON

    SELECT [Id],
           [CustomerId],
           [OfficeHoursMsg],
           [OfficeHoursMsgWaitTimeInMinutes],
           [OfflineHourMsg],
           [OfflineHoursMsgWaitTimeInMinutes],
           [ReplyAtDiffPeriod],
           [IsActive]
    FROM [dbo].[CustomerGuestAlerts] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [CustomerId] = @CustomerId
END");
            #endregion

            #region GetCustomerGuestAppBuilderById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuestAppBuilderById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerId],
           [CustomerRoomNameId],
           [Message],
           [SecondaryMessage],
           [LocalExperience],
           [Ekey],
           [PropertyInfo],
           [EnhanceYourStay],
           [Reception],
           [Housekeeping],
           [RoomService],
           [Concierge],
           [TransferServices],
           [IsActive],
           [CreatedAt],
           [UpdateAt],
           [DeletedAt],
           [CreatedBy]
    FROM [dbo].[CustomerGuestAppBuilders] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetCustomerGuestAppBuilders
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuestAppBuilders]
(
    @CustomerId INT = 0,
    @SearchColumn NVARCHAR(50) = NULL,
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'CustomerRoomNameId',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
    SET @SearchValue = LTRIM(RTRIM(@SearchValue));

    WITH CustomerGuestAppBuilders_Results
    AS (SELECT [Id],
               [CustomerId],
               [CustomerRoomNameId],
               [Message],
               [SecondaryMessage],
               [LocalExperience],
               [Ekey],
               [PropertyInfo],
               [EnhanceYourStay],
               [Reception],
               [Housekeeping],
               [RoomService],
               [Concierge],
               [TransferServices],
               [IsActive]
        FROM [dbo].[CustomerGuestAppBuilders] WITH (NOLOCK)
        WHERE [DeletedAt] IS NULL
              AND ([CustomerId] = @CustomerId OR 0 = @CustomerId)
              AND @SearchColumn = ''
              OR (CASE @SearchColumn
                      WHEN 'CustomerRoomNameId' THEN
                          [CustomerRoomNameId]
                  END
                 ) LIKE '%' + @SearchValue + '%'
        ORDER BY CASE
                     WHEN
                     (
                         @SortColumn = 'CustomerRoomNameId'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         [CustomerRoomNameId]
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'CustomerRoomNameId'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         [CustomerRoomNameId]
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [CustomerId],
           [CustomerRoomNameId],
           [Message],
           [SecondaryMessage],
           [LocalExperience],
           [Ekey],
           [PropertyInfo],
           [EnhanceYourStay],
           [Reception],
           [Housekeeping],
           [RoomService],
           [Concierge],
           [TransferServices],
           [IsActive]
    FROM CustomerGuestAppBuilders_Results
    OPTION (RECOMPILE)
END");
            #endregion

            #region GetCustomerGuestAppEnhanceYourStayItemsExtraByStayId
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuestAppEnhanceYourStayItemsExtraByStayId]
(
	@CustomerGuestAppEnhanceYourStayItemId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras].[Id],
           [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras].[CustomerGuestAppEnhanceYourStayItemId],
           [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras].[QueType],
           [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras].[Questions],
           [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras].[OptionValues],
           [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras].[IsActive]
    FROM [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras] WITH (NOLOCK)
    WHERE [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras].[DeletedAt] IS NULL
          AND [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras].[CustomerGuestAppEnhanceYourStayItemId] = @CustomerGuestAppEnhanceYourStayItemId
END");
            #endregion

            #region GetCustomerGuestById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuestById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerReservationId],
           [Firstname],
           [Lastname],
           [Email],
           [Picture],
           [PhoneCountry],
           [PhoneNumber],
           [Country],
           [Language],
           [IdProof],
           [IdProofType],
           [IdProofNumber],
           [BlePinCode],
           [Pin],
           [Street],
           [StreetNumber],
           [City],
           [Postalcode],
           [ArrivalFlightNumber],
           [DepartureAirline],
           [DepartureFlightNumber],
           [Signature],
           [RoomNumber],
           [TermsAccepted],
           [FirstJourneyStep],
           [Rating],
           [IsActive],
           [CreatedAt],
           [UpdateAt],
           [DeletedAt],
           [CreatedBy],
           [DateOfBirth],
           [BookingChannel],
           [DepartingFlightDate]
    FROM [dbo].[CustomerGuests] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetCustomerGuests
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuests]
(
    @CustomerId INT = 0,
    @SearchValue NVARCHAR(50) = '',
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'Firstname',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchValue = LTRIM(RTRIM(@SearchValue));

    WITH CustomerGuests_Results
    AS (SELECT [dbo].[CustomerGuests].[Id],
               [dbo].[CustomerGuests].[CustomerReservationId],
               [dbo].[CustomerGuests].[Firstname],
               [dbo].[CustomerGuests].[Lastname],
               [dbo].[CustomerGuests].[Email],
               [dbo].[CustomerGuests].[Picture],
               [dbo].[CustomerGuests].[PhoneCountry],
               [dbo].[CustomerGuests].[PhoneNumber],
               [dbo].[CustomerGuests].[Country],
               [dbo].[CustomerGuests].[Language],
               [dbo].[CustomerGuests].[IdProof],
               [dbo].[CustomerGuests].[IdProofType],
               [dbo].[CustomerGuests].[IdProofNumber],
               [dbo].[CustomerGuests].[BlePinCode],
               [dbo].[CustomerGuests].[Pin],
               [dbo].[CustomerGuests].[Street],
               [dbo].[CustomerGuests].[StreetNumber],
               [dbo].[CustomerGuests].[City],
               [dbo].[CustomerGuests].[Postalcode],
               [dbo].[CustomerGuests].[ArrivalFlightNumber],
               [dbo].[CustomerGuests].[DepartureAirline],
               [dbo].[CustomerGuests].[DepartureFlightNumber],
               [dbo].[CustomerGuests].[Signature],
               [dbo].[CustomerGuests].[RoomNumber],
               [dbo].[CustomerGuests].[TermsAccepted],
               [dbo].[CustomerGuests].[FirstJourneyStep],
               [dbo].[CustomerGuests].[Rating],
               [dbo].[CustomerGuests].[IsActive],
               dbo.GetGuestsStatus(dbo.CustomerGuests.Id) AS [GuestStatus],
               COUNT(*) OVER () as [FilteredCount]
        FROM [dbo].[CustomerGuests] WITH (NOLOCK)
            INNER JOIN [dbo].[CustomerReservations] WITH (NOLOCK)
                ON [dbo].[CustomerReservations].[Id] = [dbo].[CustomerGuests].[CustomerReservationId]
        WHERE [dbo].[CustomerGuests].[DeletedAt] IS NULL
              AND (
                      [dbo].[CustomerReservations].[CustomerId] = @CustomerId
                      OR 0 = @CustomerId
                  )
              AND (
                      [dbo].[CustomerGuests].[Firstname] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[Lastname] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[Email] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[PhoneNumber] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[Country] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[IdProofNumber] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[IdProofType] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[Street] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[Postalcode] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[StreetNumber] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[ArrivalFlightNumber] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[DepartureFlightNumber] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[RoomNumber] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[CustomerGuests].[Rating] LIKE '%' + @SearchValue + '%'
                  )
        ORDER BY CASE
                     WHEN
                     (
                         @SortColumn = 'Firstname'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Firstname
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Firstname'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Firstname
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Lastname'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Lastname
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Lastname'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Lastname
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Email'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Email
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Email'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Email
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'PhoneNumber'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         PhoneNumber
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'PhoneNumber'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         PhoneNumber
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Country'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Country
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Country'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Country
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'IdProofNumber'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         IdProofNumber
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'IdProofNumber'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         IdProofNumber
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Street'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Street
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Street'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Street
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Postalcode'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Postalcode
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Postalcode'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Postalcode
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'StreetNumber'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         StreetNumber
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'StreetNumber'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         StreetNumber
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'ArrivalFlightNumber'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         ArrivalFlightNumber
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'ArrivalFlightNumber'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         ArrivalFlightNumber
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'DepartureFlightNumber'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         DepartureFlightNumber
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'DepartureFlightNumber'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         DepartureFlightNumber
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'RoomNumber'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         RoomNumber
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'RoomNumber'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         RoomNumber
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Rating'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Rating
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Rating'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Rating
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [CustomerReservationId],
           [Firstname],
           [Lastname],
           [Email],
           [Picture],
           [PhoneCountry],
           [PhoneNumber],
           [Country],
           [Language],
           [IdProof],
           [IdProofType],
           [IdProofNumber],
           [BlePinCode],
           [Pin],
           [Street],
           [StreetNumber],
           [City],
           [Postalcode],
           [ArrivalFlightNumber],
           [DepartureAirline],
           [DepartureFlightNumber],
           [Signature],
           [RoomNumber],
           [TermsAccepted],
           [FirstJourneyStep],
           [Rating],
           [IsActive],
           [FilteredCount],
           [GuestStatus]
    FROM CustomerGuests_Results
    OPTION (RECOMPILE)
END");
            #endregion

            #region GetCustomerGuestsByReservation
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuestsByReservation] 
(
	@ReservationId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerReservationId],
           [Firstname],
           [Lastname],
           [Email],
           [Picture],
           [PhoneCountry],
           [PhoneNumber],
           [AgeCategory],
           [IsCoGuest]
    FROM  [dbo].[CustomerGuests] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [CustomerReservationId] = @ReservationId
END");
            #endregion

            #region GetCustomerGuestsCheckInFormBuilder
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerGuestsCheckInFormBuilder] 
(
	@CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [Id],
                   [CustomerId],
                   [Color],
                   [Name],
                   [Stars],
                   [Logo],
                   [AppImage],
                   [SplashScreen],
                   [IsOnlineCheckInFormEnable],
                   [IsRedirectToGuestAppEnable],
                   [SubmissionMail],
                   [TermsLink],
                   [IsActive],
                   (
                       SELECT [Name],
                              [FieldType],
                              [RequiredFields],
                              [IsActive],
                              [DisplayOrder]
                       FROM [dbo].[CustomerGuestsCheckInFormFields] ff (NOLOCK)
                       WHERE [fb].[id] = [ff].[CustomerGuestsCheckInFormBuilderId]
                             AND [ff].[DeletedAt] IS NULL
                       ORDER BY [ff].[DisplayOrder] ASC
                       FOR JSON PATH
                   ) AS [GetCustomerGuestsCheckInFormFieldsOut]
            FROM [dbo].[CustomerGuestsCheckInFormBuilders] fb
            WHERE [fb].[DeletedAt] IS NULL
                  AND [fb].[CustomerId] = @CustomerId
            FOR JSON PATH
        ) as [GetCustomerGuestsCheckInFormBuilderResponseOut]
END");
            #endregion

            #region GetCustomerHouseKeepingWithRelationSP
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomerHouseKeepingWithRelationSP]
(
    @AppBuilderId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    WITH Customer_House_keeping_Results
    AS (SELECT
            (
                SELECT [Id],
                       [CustomerId],
                       [CustomerGuestAppBuilderId],
                       [CategoryName],
                       [DisplayOrder],
                       [IsActive],
                       (
                           SELECT [Id],
                                  [CustomerId],
                                  [CustomerGuestAppBuilderId],
                                  [CustomerGuestAppHousekeepingCategoryId],
                                  [CategoryName],
                                  [Name],
                                  [ItemsMonth],
                                  [ItemsDay],
                                  [ItemsMinute],
                                  [ItemsHour],
                                  [QuantityBar],
                                  [ItemLocation],
                                  [Comment],
                                  [IsPriceEnable],
                                  [Price],
                                  [Currency],
                                  [IsActive],
                                  [DisplayOrder]
                           FROM [dbo].[CustomerGuestAppHousekeepingItems] items (NOLOCK)
                           WHERE [items].[DeletedAt] IS NULL
                                 AND [items].[CustomerGuestAppHousekeepingCategoryId] = [categories].[Id]
                           ORDER BY [items].[DisplayOrder] ASC
                           FOR JSON PATH
                       ) AS [CustomerHouseKeepingItems]
                FROM [dbo].[CustomerGuestAppHousekeepingCategories] categories (NOLOCK)
                WHERE [DeletedAt] IS NULL
                      AND [categories].[CustomerGuestAppBuilderId] = @AppBuilderId
                FOR JSON PATH
            ) AS [CustomerHouseKeepingWithRelationOut]
       )
    SELECT *
    FROM Customer_House_keeping_Results
    OPTION (RECOMPILE)
END");
            #endregion

            #region GetCustomerPaymentProcessorByID
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPaymentProcessorByID] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerId],
           [PaymentProcessorId],
           [ClientId],
           [ClientSecret],
           [Currency],
           [IsActive]
    FROM [dbo].[CustomerPaymentProcessors] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetCustomerPaymentProcessors
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPaymentProcessors]
(
    @CustomerId INT = 0,
    @PageNo INT = 1,
    @PageSize INT = 10 --NoOf Record To Get
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [PaymentProcessorId],
           [CustomerId],
           [ClientId],
           [ClientSecret],
           [Currency],
           [IsActive]
    FROM [dbo].[CustomerPaymentProcessors] WITH (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [CustomerId] = @CustomerId
    ORDER BY [PaymentProcessorId] OffSet @PageSize * (@PageNo - 1) Rows Fetch Next @PageSize Rows Only
END");
            #endregion

            #region GetCustomerPropertyEmergencyNumberById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPropertyEmergencyNumberById] 
(
	@Id INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerPropertyInformationId],
           [Name],
           [PhoneCountry],
           [PhoneNumber],
           [IsActive]
    FROM [dbo].[CustomerPropertyEmergencyNumbers] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetCustomerPropertyEmergencyNumbers
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPropertyEmergencyNumbers] 
(
	@PropertyId INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerPropertyInformationId],
           [Name],
           [PhoneCountry],
           [PhoneNumber],
           [IsActive]
    FROM [dbo].[CustomerPropertyEmergencyNumbers] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [CustomerPropertyInformationId] = @PropertyId
END");
            #endregion

            #region GetCustomerPropertyExtraById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPropertyExtraById] 
(
	@Id INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerPropertyInformationId],
           [ExtraType],
           [Name],
           [IsActive]
    FROM [dbo].[CustomerPropertyExtras] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetCustomerPropertyExtras
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPropertyExtras] 
(
	@CustomerPropertyInformationId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [CE].[Id],
                   [CE].[CustomerPropertyInformationId],
                   [CE].[ExtraType],
                   [CE].[Name],
                   JSON_QUERY(
                   (
                       SELECT [CED].[Id],
                              [CED].[CustomerPropertyExtraId],
                              [CED].[Description],
                              [CED].[Link]
                       FROM [dbo].[CustomerPropertyExtraDetails] CED (NOLOCK)
                       WHERE [DeletedAt] IS NULL
                             AND [CED].[CustomerPropertyExtraId] = [CE].[Id]
                       FOR JSON PATH
                   )
                             ) as [CustomerPropertyExtraDetailsOuts]
            FROM [dbo].[CustomerPropertyExtras] CE (NOLOCK)
            WHERE [CE].[DeletedAt] IS NULL
                  AND [CE].[CustomerPropertyInformationId] = @CustomerPropertyInformationId
            FOR JSON PATH
        )
END");
            #endregion

            #region GetCustomerPropertyServiceById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPropertyServiceById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [CustomerPropertyServices].[Id],
           [CustomerPropertyServices].[Name],
           [CustomerPropertyServices].[Icon],
           [CustomerPropertyServices].[Description],
           [CustomerPropertyServices].[IsActive],
           [CustomerPropertyServiceImages].[ServiceImages]
    FROM [dbo].[CustomerPropertyServices] (NOLOCK)
        LEFT JOIN [dbo].[CustomerPropertyServiceImages] (NOLOCK)
            ON [CustomerPropertyServices].[Id] = [CustomerPropertyServiceImages].[CustomerPropertyServiceId]
    WHERE [CustomerPropertyServices].[DeletedAt] IS NULL
          AND [CustomerPropertyServices].[Id] = @Id
END");
            #endregion

            #region GetCustomerPropertyServices
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPropertyServices]
(
    @CustomerPropertyInformationId INT = 0,
    @SearchColumn NVARCHAR(50) = '',
    @SearchValue NVARCHAR(50) = '',
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
    SET @SearchValue = LTRIM(RTRIM(@SearchValue));
    WITH CustomerPropertyInformations_Results
    AS (SELECT [Id],
               [CustomerPropertyInformationId],
               [Name],
               [Icon],
               [Description],
               [IsActive],
               [CreatedAt],
               [UpdateAt],
               [DeletedAt],
               [CreatedBy],
               COUNT(*) OVER () as [FilteredCount]
        FROM [dbo].[CustomerPropertyServices] WITH (NOLOCK)
        WHERE [DeletedAt] IS NULL
              AND [CustomerPropertyInformationId] = @CustomerPropertyInformationId
              AND @SearchColumn = ''
              OR (CASE @SearchColumn
                      WHEN 'Name' THEN
                          Name
                  END
                 ) LIKE '%' + @SearchValue + '%'
        ORDER BY CASE
                     WHEN
                     (
                         @SortColumn = 'Name'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Name
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Name'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Name
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [CustomerPropertyInformationId],
           [Name],
           [Icon],
           [Description],
           [IsActive],
           [CreatedAt],
           [UpdateAt],
           [DeletedAt],
           [CreatedBy],
           FilteredCount
    FROM CustomerPropertyInformations_Results
    OPTION (RECOMPILE)
END");
            #endregion

            #region GetCustomerPropertyServicesForAPPBuilder
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerPropertyServicesForAPPBuilder] 
(
	@CustomerPropertyInformationId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [C].[Id],
                   [C].[CustomerPropertyInformationId],
                   [C].[Name],
                   [C].[Icon],
                   [C].[Description],
                   JSON_QUERY(
                   (
                       SELECT [CI].[Id],
                              [CI].[CustomerPropertyServiceId],
                              [CI].[ServiceImages]
                       FROM [dbo].[CustomerPropertyServiceImages] CI
                       WHERE [CI].[DeletedAt] IS NULL
                             AND [CI].[CustomerPropertyServiceId] = [C].[Id]
                       FOR JSON PATH
                   )
                             ) AS [customerPropertyInfoServiceImagesOuts]
            FROM [dbo].[CustomerPropertyServices] C
            WHERE [C].[DeletedAt] IS NULL
                  AND [C].[CustomerPropertyInformationId] = @CustomerPropertyInformationId
            FOR JSON PATH
        )
END");
            #endregion

            #region GetCustomerReceptionWithRelationSP
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerReceptionWithRelationSP]
(
    @AppBuilderId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    WITH Customer_Reception_Results
    AS (SELECT
            (
                SELECT [Id],
                       [CustomerId],
                       [CustomerGuestAppBuilderId],
                       [CategoryName],
                       [DisplayOrder],
                       [IsActive],
                       (
                           SELECT [Id],
                                  [CustomerId],
                                  [CustomerGuestAppBuilderId],
                                  [CustomerGuestAppReceptionCategoryId],
                                  [CategoryName],
                                  [Name],
                                  [ItemsMonth],
                                  [ItemsDay],
                                  [ItemsMinute],
                                  [ItemsHour],
                                  [QuantityBar],
                                  [ItemLocation],
                                  [Comment],
                                  [IsPriceEnable],
                                  [Price],
                                  [Currency],
                                  [IsActive],
                                  [DisplayOrder]
                           FROM [dbo].[CustomerGuestAppReceptionItems] items (NOLOCK)
                           WHERE [items].[DeletedAt] IS NULL
                                 AND [items].[CustomerGuestAppReceptionCategoryId] = [categories].[Id]
                           ORDER BY [DisplayOrder] ASC
                           FOR JSON PATH
                       ) AS CustomerReceptionItems
                FROM [dbo].[CustomerGuestAppReceptionCategories] categories (NOLOCK)
                WHERE [categories].[DeletedAt] IS NULL
                      AND [categories].[CustomerGuestAppBuilderId] = @AppBuilderId
                FOR JSON PATH
            ) AS [CustomerReceptionWithRelationOut]
       )
    SELECT *
    FROM Customer_Reception_Results
    OPTION (RECOMPILE)
END");
            #endregion

            #region GetCustomerReservationById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerReservationById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerId],
           [Uuid],
           [ReservationNumber],
           [Source],
           [NoOfGuestAdults],
           [NoOfGuestChildrens],
           [CheckinDate],
           [CheckoutDate],
           [IsActive]
    FROM [dbo].[CustomerReservations] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetCustomerReservations
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerReservations]
(
    @CustomerId INT = 0,
    @SearchColumn NVARCHAR(50) = '',
    @SearchValue NVARCHAR(50) = '',
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'ReservationNumber',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
    SET @SearchValue = LTRIM(RTRIM(@SearchValue));
    WITH CustomerReservations_Results
    AS (SELECT [Id],
               [CustomerId],
               [Uuid],
               [ReservationNumber],
               [Source],
               [NoOfGuestAdults],
               [NoOfGuestChildrens],
               [CheckinDate],
               [CheckoutDate],
               [IsActive]
        FROM [dbo].[CustomerReservations] WITH (NOLOCK)
        WHERE [DeletedAt] IS NULL
              AND ([CustomerId] = @CustomerId OR 0 = @CustomerId)
              AND @SearchColumn = ''
              OR (CASE @SearchColumn
                      WHEN 'ReservationNumber' THEN
                          [ReservationNumber]
                  END
                 ) LIKE '%' + @SearchValue + '%'
        ORDER BY CASE
                     WHEN
                     (
                         @SortColumn = 'ReservationNumber'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         [ReservationNumber]
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'ReservationNumber'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         [ReservationNumber]
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [CustomerId],
           [Uuid],
           [ReservationNumber],
           [Source],
           [NoOfGuestAdults],
           [NoOfGuestChildrens],
           [CheckinDate],
           [CheckoutDate],
           [IsActive]
    FROM CustomerReservations_Results 
    OPTION (RECOMPILE)
END");
            #endregion

            #region GetCustomerReservationWithCustomerPropInfo
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerReservationWithCustomerPropInfo]
(
    @GuestId INT = 0,
    @ReservationId INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [g].[IsCoGuest],
                   ISNULL([g].[Firstname], '') + SPACE(1) + ISNULL([g].[Lastname], '') AS [Name],
                   JSON_QUERY(
                   (
                       SELECT [Id],
                              [CustomerId],
                              [Uuid],
                              [ReservationNumber],
                              [Source],
                              [NoOfGuestAdults],
                              [NoOfGuestChildrens],
                              [CheckinDate],
                              [CheckoutDate],
                              [isCheckInCompleted],
                              [isSkipCheckIn],
                              [IsActive],
                              JSON_QUERY(
                              (
                                  SELECT [ff].[Id],
                                         [CustomerId],
                                         [Color],
                                         [Name],
                                         [Stars],
                                         [Logo],
                                         [AppImage],
                                         [SplashScreen],
                                         [IsOnlineCheckInFormEnable],
                                         [IsRedirectToGuestAppEnable],
                                         [SubmissionMail],
                                         [TermsLink],
                                         [ff].[IsActive],
                                         [bt].[BizType] AS [BusinessType]
                                  FROM [dbo].[CustomerGuestsCheckInFormBuilders] ff (NOLOCK)
                                      JOIN [dbo].[Customers] c (NOLOCK)
                                          ON [ff].[CustomerId] = [c].[Id]
                                      JOIN [dbo].[BusinessTypes] bt (NOLOCK)
                                          ON [c].[BusinessTypeId] = [bt].[Id]
                                  WHERE [ff].[DeletedAt] IS NULL
                                        AND [fb].[CustomerId] = [ff].[CustomerId]
                                  FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                              )
                                        ) as [GetCustomerGuestsCheckInFormBuilderResponseOut]
                       FROM [dbo].[CustomerReservations] fb (NOLOCK)
                       WHERE [fb].[DeletedAt] IS NULL
                             AND [fb].[Id] = @ReservationId
                       FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                   )
                             ) as [GetCustomerReservationResponseOut]
            FROM [dbo].[CustomerGuests] g (NOLOCK)
            WHERE [g].[DeletedAt] IS NULL
                  AND [g].[Id] = @GuestId
            FOR JSON PATH
        )
END");
            #endregion

            #region GetCustomerRoomNames
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerRoomNames] 
(
	@CustomerId INT = 0
)
AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
           CASE
               WHEN EXISTS
                    (
                        SELECT *
                        FROM [dbo].[CustomerGuestAppBuilders] (NOLOCK)
                        WHERE [DeletedAt] IS NULL
                              AND [CustomerRoomNameId] = [rooms].[Id]
                              AND [CustomerId] = @CustomerId
                    ) THEN
                   1
               ELSE
                   0
           END AS [IsCompleted]
    FROM [dbo].[CustomerRoomNames] rooms (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [CustomerId] = @CustomerId
END");
            #endregion

            #region GetCustomerRoomServiceWithRelationSP
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerRoomServiceWithRelationSP]
(
    @AppBuilderId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    WITH Customer_RoomService_Results
    AS (SELECT
            (
                SELECT [Id],
                       [CustomerId],
                       [CustomerGuestAppBuilderId],
                       [CategoryName],
                       [DisplayOrder],
                       [IsActive],
                       (
                           SELECT [Id],
                                  [CustomerId],
                                  [CustomerGuestAppBuilderId],
                                  [CustomerGuestAppRoomServiceCategoryId],
                                  [CategoryName],
                                  [Name],
                                  [ItemsMonth],
                                  [ItemsDay],
                                  [ItemsMinute],
                                  [ItemsHour],
                                  [QuantityBar],
                                  [ItemLocation],
                                  [Comment],
                                  [IsPriceEnable],
                                  [Price],
                                  [Currency],
                                  [IsActive],
                                  [DisplayOrder]
                           FROM [dbo].[CustomerGuestAppRoomServiceItems] items (NOLOCK)
                           WHERE [DeletedAt] IS NULL
                                 AND [items].[CustomerGuestAppRoomServiceCategoryId] = [categories].[Id]
                           ORDER BY [DisplayOrder] ASC
                           FOR JSON PATH
                       ) AS [CustomerRoomServiceItems]
                FROM [dbo].[CustomerGuestAppRoomServiceCategories] categories (NOLOCK)
                WHERE [DeletedAt] IS NULL
                      AND [CustomerGuestAppBuilderId] = @AppBuilderId
                ORDER BY [DisplayOrder] ASC
                FOR JSON PATH
            ) AS [CustomerRoomServiceWithRelationOut]
       )
    SELECT *
    FROM Customer_RoomService_Results
    OPTION (RECOMPILE)
END
");
            #endregion

            #region GetCustomers
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomers] 
(
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'BusinessName',
    @SortOrder NVARCHAR(5) = 'ASC',
    @AlphabetsStartsWith NVARCHAR(50) = NULL
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON;

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
    SET @AlphabetsStartsWith = LTRIM(RTRIM(@AlphabetsStartsWith));
    WITH Customer_Results
    AS (SELECT [dbo].[Customers].[Id],
               [dbo].[CustomerGuestsCheckInFormBuilders].[Logo] AS [ProfilePicture],
               [dbo].[Customers].[BusinessName],
               [dbo].[BusinessTypes].[BizType],
               [dbo].[Products].[Name] AS [ServicePackName],
               COUNT(*) OVER () AS [FilteredCount]
        FROM [dbo].[Customers] (NOLOCK)
            INNER JOIN [dbo].[BusinessTypes] (NOLOCK)
                ON [dbo].[Customers].[BusinessTypeId] = [dbo].[BusinessTypes].[Id]
            LEFT OUTER JOIN [dbo].[Products] (NOLOCK)
                ON [dbo].[Customers].[ProductId] = [dbo].[Products].[Id]
            LEFT JOIN [dbo].[CustomerGuestsCheckInFormBuilders] (NOLOCK)
                ON [dbo].[Customers].[Id] = [dbo].[CustomerGuestsCheckInFormBuilders].[CustomerId]
        WHERE [dbo].[Customers].[DeletedAt] IS NULL
              AND (
                      [dbo].[BusinessTypes].[BizType] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[Products].[Name] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[Customers].[BusinessName] LIKE '%' + @SearchValue + '%'
                  )
              AND (
                      @AlphabetsStartsWith IS NULL
                      OR EXISTS
        (
            SELECT 1 FROM STRING_SPLIT(@AlphabetsStartsWith, ',') AS s
        )
                  )
        ORDER BY CASE
                     WHEN @SortColumn = 'BusinessName'
                          AND @SortOrder = 'ASC' THEN
                         [dbo].[Customers].[BusinessName]
                 END ASC,
                 CASE
                     WHEN @SortColumn = 'BusinessName'
                          AND @SortOrder = 'DESC' THEN
                         [dbo].[Customers].[BusinessName]
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [ProfilePicture],
           [BusinessName],
           [BizType],
           [ServicePackName],
           [FilteredCount]
    FROM Customer_Results
    OPTION (RECOMPILE)
END");
            #endregion

            #region GetCustomersEnhanceYourStay
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomersEnhanceYourStay] 
(
	@BuilderId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [m].[Id],
                   [m].[CustomerGuestAppBuilderId],
                   [m].[CustomerId],
                   [m].[CategoryName],
                   [m].[IsActive],
                   [m].[DisplayOrder],
                   JSON_QUERY(
                   (
                       SELECT [Id],
                              [CustomerGuestAppBuilderId],
                              [CustomerId],
                              [CustomerGuestAppBuilderCategoryId],
                              [Badge],
                              [ShortDescription],
                              [LongDescription],
                              [ButtonType],
                              [ButtonText],
                              [ChargeType],
                              [Discount],
                              [Price],
                              [Currency],
                              [IsActive],
                              [DisplayOrder],
                              JSON_QUERY(
                              (
                                  SELECT [Id],
                                         [CustomerGuestAppEnhanceYourStayItemId],
                                         [ItemsImages],
                                         [DisaplayOrder],
                                         [IsActive]
                                  FROM [dbo].[CustomerGuestAppEnhanceYourStayItemsImages] (NOLOCK)
                                  WHERE [DeletedAt] IS NULL
                                        AND [CustomerGuestAppEnhanceYourStayItemId] = [n].[Id]
                                  ORDER BY
                                      [DisplayOrder] ASC
                                  FOR JSON PATH
                              )
                                        ) as [customerGuestAppEnhanceYourStayItemsImages]
                       FROM [dbo].[CustomerGuestAppEnhanceYourStayItems] (NOLOCK) n
                       WHERE [DeletedAt] IS NULL
                             AND [CustomerGuestAppBuilderCategoryId] = [m].[Id]
                       ORDER BY CASE
                                    WHEN [DisplayOrder] IS NULL THEN
                                        1
                                    ELSE
                                        0
                                END,
                                DisplayOrder ASC
                       FOR JSON PATH
                   )
                             ) AS [customerGuestAppEnhanceYourStayItems]
            FROM [dbo].[CustomerGuestAppEnhanceYourStayCategories] m
            WHERE [m].[DeletedAt] IS NULL
                  AND [m].[CustomerGuestAppBuilderId] = @BuilderId
            ORDER BY CASE
                         WHEN [m].[DisplayOrder] IS NULL THEN
                             1
                         ELSE
                             0
                     END,
                     [m].[DisplayOrder] ASC
            FOR JSON PATH
        )
END");
            #endregion

            #region GetCustomersEnhanceYourStayByCategory
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomersEnhanceYourStayByCategory] 
(
	@CategoryId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [m].[Id],
                   [m].[CustomerGuestAppBuilderId],
                   [m].[CustomerId],
                   [m].[CategoryName],
                   [m].[IsActive],
                   [m].[DisplayOrder],
                   JSON_QUERY(
                   (
                       SELECT [Id],
                              [CustomerGuestAppBuilderId],
                              [CustomerId],
                              [CustomerGuestAppBuilderCategoryId],
                              [Badge],
                              [ShortDescription],
                              [LongDescription],
                              [ButtonType],
                              [ButtonText],
                              [ChargeType],
                              [Discount],
                              [Price],
                              [Currency],
                              [IsActive],
                              [DisplayOrder],
                              JSON_QUERY(
                              (
                                  SELECT [Id],
                                         [CustomerGuestAppEnhanceYourStayItemId],
                                         [ItemsImages],
                                         [DisaplayOrder],
                                         [IsActive]
                                  FROM [dbo].[CustomerGuestAppEnhanceYourStayItemsImages] (NOLOCK)
                                  WHERE [DeletedAt] IS NULL
                                        AND [CustomerGuestAppEnhanceYourStayItemId] = [n].[Id]
                                  ORDER BY
                                      [DisplayOrder] ASC
                                  FOR JSON PATH
                              )
                                        ) as [customerGuestAppEnhanceYourStayItemsImages]
                       FROM [dbo].[CustomerGuestAppEnhanceYourStayItems] n (NOLOCK)
                       WHERE [DeletedAt] IS NULL
                             AND [CustomerGuestAppBuilderCategoryId] = [m].[Id]
                       ORDER BY CASE
                                    WHEN [DisplayOrder] IS NULL THEN
                                        1
                                    ELSE
                                        0
                                END,
                                [DisplayOrder] ASC
                       FOR JSON PATH
                   )
                             ) as [customerGuestAppEnhanceYourStayItems]
            FROM [dbo].[CustomerGuestAppEnhanceYourStayCategories] m
            WHERE [m].[DeletedAt] IS NULL
                  AND [m].[Id] = @CategoryId
            ORDER BY CASE
                         WHEN [m].[DisplayOrder] IS NULL THEN
                             1
                         ELSE
                             0
                     END,
                     [m].[DisplayOrder] ASC
            FOR JSON PATH
        )
END");
            #endregion

            #region GetCustomersEnhanceYourStayCategories
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomersEnhanceYourStayCategories]
(
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10, --NoOf Record To Get
    @SortColumn NVARCHAR(20) = 'CategoryName',
    @SortOrder NVARCHAR(5) = 'ASC',
    @CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchValue = LTRIM(RTRIM(@SearchValue));
    WITH Enhance_Stay_Categories_Results
    AS (SELECT [Id],
               [CustomerId],
               [CustomerGuestAppBuilderId],
               [CategoryName],
               COUNT(*) OVER () as [FilteredCount]
        FROM [dbo].[CustomerGuestAppEnhanceYourStayCategories] (NOLOCK)
        WHERE [DeletedAt] IS NULL
              AND (
                      [CustomerId] = @CustomerId
                      OR 0 = @CustomerId
                  )
              AND ([CategoryName] LIKE '%' + @SearchValue + '%')
        ORDER BY CASE
                     WHEN
                     (
                         @SortColumn = 'CategoryName'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         CategoryName
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'CategoryName'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         CategoryName
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [CustomerId],
           [CustomerGuestAppBuilderId],
           [CategoryName],
           [FilteredCount]
    FROM Enhance_Stay_Categories_Results
    OPTION (RECOMPILE)

END");
            #endregion

            #region GetCustomersEnhanceYourStayCategoriesWithRelation
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomersEnhanceYourStayCategoriesWithRelation]
(
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10, --NoOf Record To Get
    @SortColumn NVARCHAR(20) = 'CategoryName',
    @SortOrder NVARCHAR(5) = 'ASC',
    @CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchValue = LTRIM(RTRIM(@SearchValue));
    WITH Enhance_Stay_Results
    AS (SELECT
            (
                SELECT [Id],
                       [CustomerId],
                       [CustomerGuestAppBuilderId],
                       [CategoryName],
                       (
                           SELECT [Id],
                                  [CustomerGuestAppBuilderId],
                                  [CustomerId],
                                  [CustomerGuestAppBuilderCategoryId],
                                  [Badge],
                                  [ShortDescription],
                                  [LongDescription],
                                  [ButtonType],
                                  [ButtonText],
                                  [ChargeType],
                                  [Discount],
                                  [Price],
                                  [Currency],
                                  [IsActive]
                           FROM [dbo].[CustomerGuestAppEnhanceYourStayItems] items (NOLOCK)
                           WHERE [DeletedAt] IS NULL
                                 AND [items].[CustomerGuestAppBuilderCategoryId] = [categories].[Id]
                           FOR JSON PATH
                       ) AS [CustomerGuestAppEnhanceYourStayItems]
                FROM [dbo].[CustomerGuestAppEnhanceYourStayCategories] categories (NOLOCK)
                WHERE [DeletedAt] IS NULL
                      AND (
                              [CustomerId] = @CustomerId
                              OR 0 = @CustomerId
                          )
                      AND ([CategoryName] LIKE '%' + @SearchValue + '%')
                ORDER BY CASE
                             WHEN
                             (
                                 @SortColumn = 'CategoryName'
                                 AND @SortOrder = 'ASC'
                             ) THEN
                                 CategoryName
                         END ASC,
                         CASE
                             WHEN
                             (
                                 @SortColumn = 'CategoryName'
                                 AND @SortOrder = 'DESC'
                             ) THEN
                                 CategoryName
                         END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
                FOR JSON PATH
            ) AS [CustomerEnhanceYourStayCategoriesWithRelationOut]
       )
    SELECT *
    from Enhance_Stay_Results
    OPTION (RECOMPILE)

END");
            #endregion

            #region GetCustomersEnhanceYourStayCategoryById
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomersEnhanceYourStayCategoryById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerId],
           [CustomerGuestAppBuilderId],
           [CategoryName]
    FROM [dbo].[CustomerGuestAppEnhanceYourStayCategories] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetCustomersEnhanceYourStayCategoryItemById
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomersEnhanceYourStayCategoryItemById] 
(
	@Id INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [Id],
                   [CustomerGuestAppBuilderId],
                   [CustomerId],
                   [CustomerGuestAppBuilderCategoryId],
                   [Badge],
                   [ShortDescription],
                   [LongDescription],
                   [ButtonType],
                   [ButtonText],
                   [ChargeType],
                   [Discount],
                   [Price],
                   [Currency],
                   [DisplayOrder],
                   [IsActive],
                   (
                       SELECT [Id],
                              [CustomerGuestAppEnhanceYourStayItemId],
                              [ItemsImages],
                              [DisaplayOrder],
                              [IsActive],
                              [CreatedAt],
                              [UpdateAt],
                              [DeletedAt],
                              [CreatedBy]
                       FROM [dbo].[CustomerGuestAppEnhanceYourStayItemsImages] images (NOLOCK)
                       WHERE [DeletedAt] IS NULL
                             AND [images].[CustomerGuestAppEnhanceYourStayItemId] = [CustomerGuestAppEnhanceYourStayItems].[Id]
                       FOR JSON PATH
                   ) AS [ItemsImages],
                   (
                       SELECT [Id],
                              [CustomerGuestAppEnhanceYourStayItemId],
                              [QueType],
                              [Questions],
                              [OptionValues],
                              [IsActive],
                              [CreatedAt],
                              [UpdateAt],
                              [DeletedAt],
                              [CreatedBy]
                       FROM [dbo].[CustomerGuestAppEnhanceYourStayCategoryItemsExtras] extraDetails (NOLOCK)
                       WHERE [DeletedAt] IS NULL
                             AND [extraDetails].[CustomerGuestAppEnhanceYourStayItemId] = [CustomerGuestAppEnhanceYourStayItems].[Id]
                       FOR JSON PATH
                   ) AS [CustomerEnhanceYourStayCategoryItemsExtra]
            FROM [dbo].[CustomerGuestAppEnhanceYourStayItems] AS CustomerGuestAppEnhanceYourStayItems (NOLOCK)
            WHERE [DeletedAt] IS NULL
                  AND [Id] = @Id
            FOR JSON PATH
        ) AS [CustomerEnhanceYourStayItemByIdOut]
END");
            #endregion

            #region GetCustomersEnhanceYourStayItems
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomersEnhanceYourStayItems]
(
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10, --NoOf Record To Get
    @SortColumn NVARCHAR(20) = NULL,
    @SortOrder NVARCHAR(5) = NULL,
    @CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchValue = LTRIM(RTRIM(@SearchValue));
    WITH Enhance_Stay_Items_Results
    AS (SELECT [Id],
               [CustomerGuestAppBuilderId],
               [CustomerId],
               [CustomerGuestAppBuilderCategoryId],
               [Badge],
               [ShortDescription],
               [LongDescription],
               [ButtonType],
               [ButtonText],
               [ChargeType],
               [Discount],
               [Price],
               [Currency],
               [IsActive],
               COUNT(*) OVER () as [FilteredCount]
        FROM [dbo].[CustomerGuestAppEnhanceYourStayItems] (NOLOCK)
        WHERE [DeletedAt] IS NULL
              AND ([CustomerId] = @CustomerId OR 0 = @CustomerId)
              AND (
                      Badge LIKE '%' + @SearchValue + '%'
                      OR ShortDescription LIKE '%' + @SearchValue + '%'
                      OR LongDescription LIKE '%' + @SearchValue + '%'
                      OR ButtonType LIKE '%' + @SearchValue + '%'
                      OR ButtonText LIKE '%' + @SearchValue + '%'
                      OR ChargeType LIKE '%' + @SearchValue + '%'
                      OR Discount LIKE '%' + @SearchValue + '%'
                      OR Price LIKE '%' + @SearchValue + '%'
                      OR Currency LIKE '%' + @SearchValue + '%'
                      OR IsActive LIKE '%' + @SearchValue + '%'
                  )
        ORDER BY CASE
                     WHEN
                     (
                         @SortColumn = 'Badge'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Badge
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Badge'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Badge
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'ShortDescription'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         ShortDescription
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'ShortDescription'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         ShortDescription
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'LongDescription'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         LongDescription
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'LongDescription'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         LongDescription
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'ButtonType'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         ButtonType
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'ButtonType'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         ButtonType
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'ChargeType'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         ChargeType
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'ChargeType'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         ChargeType
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Discount'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Discount
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Discount'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Discount
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Price'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Price
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Price'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Price
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Currency'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Currency
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Currency'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Currency
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'IsActive'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         IsActive
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'IsActive'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         IsActive
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [CustomerGuestAppBuilderId],
           [CustomerId],
           [CustomerGuestAppBuilderCategoryId],
           [Badge],
           [ShortDescription],
           [LongDescription],
           [ButtonType],
           [ButtonText],
           [ChargeType],
           [Discount],
           [Price],
           [Currency],
           [IsActive],
           [FilteredCount]
    FROM Enhance_Stay_Items_Results
    OPTION (RECOMPILE)

END");
            #endregion

            #region GetCustomersGuestAppEnhanceYourStayItemImages
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomersGuestAppEnhanceYourStayItemImages]
(
    @SearchColumn NVARCHAR(50) = NULL,
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10, --NoOf Record To Get
    @SortColumn NVARCHAR(20) = 'ItemsImages',
    @SortOrder NVARCHAR(5) = 'ASC',
    @CustomerGuestAppEnhanceYourStayItemId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
    SET @SearchValue = LTRIM(RTRIM(@SearchValue));
    WITH CTE_Results
    AS (SELECT [Id],
               [CustomerGuestAppEnhanceYourStayItemId],
               [ItemsImages],
               [DisaplayOrder],
               [IsActive],
               COUNT(*) OVER () as [FilteredCount]
        FROM [dbo].[CustomerGuestAppEnhanceYourStayItemsImages] WITH (NOLOCK)
        WHERE [DeletedAt] IS NULL
              AND (
                      [CustomerGuestAppEnhanceYourStayItemId] = @CustomerGuestAppEnhanceYourStayItemId
                      OR 0 = @CustomerGuestAppEnhanceYourStayItemId
                  )
              AND @SearchColumn = ''
              OR (CASE @SearchColumn
                      WHEN 'ItemsImages' THEN
                          ItemsImages
                  END
                 ) LIKE '%' + @SearchValue + '%'
        ORDER BY CASE
                     WHEN (@SortColumn = '') THEN
                         DisaplayOrder
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'ItemsImages'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         ItemsImages
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'ItemsImages'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         ItemsImages
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [CustomerGuestAppEnhanceYourStayItemId],
           [ItemsImages],
           [DisaplayOrder],
           [IsActive],
           [FilteredCount]
    FROM CTE_Results
    OPTION (RECOMPILE)

END");
            #endregion

            #region GetCustomersGuestJourneys
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomersGuestJourneys] 
(
	@CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CutomerId],
           [JourneyStep],
           [Name],
           [SendType],
           [TimingOption1],
           [TimingOption2],
           [TimingOption3],
           [Timing],
           [NotificationDays],
           [NotificationTime],
           [GuestJourneyMessagesTemplateId],
           [TempletMessage],
           [IsActive]
    FROM [dbo].[CustomerGuestJournies] WITH (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [CutomerId] = @CustomerId
    ORDER BY [JourneyStep]
END");
            #endregion

            #region GetCustomersGuestJourneysById
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomersGuestJourneysById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON
    SELECT [Id],
           [CutomerId],
           [JourneyStep],
           [Name],
           [SendType],
           [TimingOption1],
           [TimingOption2],
           [TimingOption3],
           [Timing],
           [NotificationDays],
           [NotificationTime],
           [GuestJourneyMessagesTemplateId],
           [TempletMessage]
    FROM [dbo].[CustomerGuestJournies] WITH (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id 
END");
            #endregion

            #region GetCustomersPropertiesInfo
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetCustomersPropertiesInfo] 
(
    @SearchColumn NVARCHAR(50) = NULL,
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10, --NoOf Record To Get
    @SortColumn NVARCHAR(20) = 'WifiUsername',
    @SortOrder NVARCHAR(5) = 'ASC',
    @CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
    SET @SearchValue = LTRIM(RTRIM(@SearchValue));
    WITH CTE_Results
    AS (SELECT [Id],
               [CustomerId],
               [CustomerGuestAppBuilderId],
               [WifiUsername],
               [WifiPassword],
               [Overview],
               [CheckInPolicy],
               [TermsAndConditions],
               [Street],
               [StreetNumber],
               [City],
               [Postalcode],
               [Country],
               [IsActive],
               COUNT(*) OVER () as [FilteredCount]
        FROM [dbo].[CustomerPropertyInformations] WITH (NOLOCK)
        WHERE [DeletedAt] IS NULL
              AND ([CustomerId] = @CustomerId OR 0 = @CustomerId)
              AND @SearchColumn = ''
              OR (CASE @SearchColumn
                      WHEN 'WifiUsername' THEN
                          WifiUsername
                      WHEN 'WifiPassword' THEN
                          WifiPassword
                      WHEN 'Overview' THEN
                          Overview
                      WHEN 'CheckInPolicy' THEN
                          CheckInPolicy
                      WHEN 'TermsAndConditions' THEN
                          TermsAndConditions
                      WHEN 'Street' THEN
                          Street
                      WHEN 'StreetNumber' THEN
                          StreetNumber
                      WHEN 'City' THEN
                          City
                      WHEN 'Postalcode' THEN
                          Postalcode
                      WHEN 'Country' THEN
                          Country
                  END
                 ) LIKE '%' + @SearchValue + '%'
        ORDER BY CASE
                     WHEN
                     (
                         @SortColumn = 'WifiUsername'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         WifiUsername
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'WifiUsername'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         WifiUsername
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'WifiPassword'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         WifiPassword
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'WifiPassword'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         WifiPassword
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Overview'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Overview
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Overview'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Overview
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'CheckInPolicy'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         CheckInPolicy
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'CheckInPolicy'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         CheckInPolicy
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'TermsAndConditions'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         TermsAndConditions
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'TermsAndConditions'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         TermsAndConditions
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Street'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Street
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Street'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Street
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'StreetNumber'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         StreetNumber
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'StreetNumber'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         StreetNumber
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'City'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         City
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'City'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         City
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [CustomerId],
           [CustomerGuestAppBuilderId],
           [WifiUsername],
           [WifiPassword],
           [Overview],
           [CheckInPolicy],
           [TermsAndConditions],
           [Street],
           [StreetNumber],
           [City],
           [Postalcode],
           [Country],
           [IsActive],
           [FilteredCount]
    FROM CTE_Results
    OPTION (RECOMPILE)

END");
            #endregion

            #region GetCustomersPropertiesInfoById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomersPropertiesInfoById] 
(
	@Id INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerId],
           [CustomerGuestAppBuilderId],
           [WifiUsername],
           [WifiPassword],
           [Overview],
           [CheckInPolicy],
           [TermsAndConditions],
           [Street],
           [StreetNumber],
           [City],
           [Postalcode],
           [Country],
           [IsActive]
    FROM [dbo].[CustomerPropertyInformations] WITH (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id 
END  ");
            #endregion

            #region GetCustomerStaffAlertsByCustomerId
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerStaffAlertsByCustomerId] 
(
	@CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerId],
           [Name],
           [Platfrom],
           [PhoneCountry],
           [PhoneNumber],
           [WaitTimeInMintes],
           [IsActive]
    FROM [dbo].[CustomerStaffAlerts] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [CustomerId] = @CustomerId

END");
            #endregion

            #region GetCustomerStaffAlertsById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustomerStaffAlertsById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerId],
           [Name],
           [Platfrom],
           [PhoneCountry],
           [PhoneNumber],
           [WaitTimeInMintes],
           [IsActive]
    FROM [dbo].[CustomerStaffAlerts] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetCustPropGalleryById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetCustPropGalleryById] 
(
	@CustomerPropertyInformationId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [PropertyImage]
    FROM [dbo].[CustomerPropertyGalleries] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [CustomerPropertyInformationId] = @CustomerPropertyInformationId
          AND [IsActive] = 1
    ORDER BY [Id]
END");
            #endregion

            #region GetDepartmentById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetDepartmentById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [d].[Id],
           [d].[Name],
           [d].[DepartmentMangerId]
    FROM [dbo].[Departments] d (NOLOCK)
    WHERE [d].[DeletedAt] IS NULL
          AND [d].[IsActive] = 1
          AND [d].[Id] = @Id
END");
            #endregion

            #region GetDepartments
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetDepartments]
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
           [DepartmentMangerId]
    FROM [dbo].[Departments] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [IsActive] = 1
END");
            #endregion

            #region GetDepartmentsUsers
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetDepartmentsUsers] 
(
    @SearchValue NVARCHAR(50) = '',
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS
BEGIN

    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchValue = LTRIM(RTRIM(@SearchValue));
    WITH Users_Results
    AS (SELECT
            (
                SELECT [Id],
                       [Name],
                       [DepartmentMangerId] AS [ManagerId],
                       (
                           SELECT ([us].[FirstName] + SPACE(1) + [us].[LastName])
                           FROM [dbo].[Users] us (NOLOCK)
                           WHERE [dp].[DepartmentMangerId] = [us].[Id]
                       ) AS [ManagerName],
                       [IsActive],
                       (
                           SELECT [Id],
                                  [Name],
                                  [GroupLeaderId],
                                  (
                                      SELECT ([us].[FirstName] + SPACE(1) + [us].[LastName])
                                      FROM [dbo].[Users] us (NOLOCK)
                                      WHERE [gp].[GroupLeaderId] = [us].[Id]
                                  ) AS [GroupLeader],
                                  [IsActive],
                                  (
                                      SELECT [Id],
                                             [FirstName],
                                             [LastName],
                                             [IsActive]
                                      FROM [dbo].[Users] us (NOLOCK)
                                      WHERE [us].[GroupId] = [gp].[Id]
                                            AND [us].[DeletedAt] IS NULL
                                      FOR JSON PATH
                                  ) AS [UsersOut]
                           FROM [dbo].[groups] gp (NOLOCK)
                           WHERE [gp].[DepartmentId] = [dp].[Id]
                                 AND [gp].[DeletedAt] IS NULL
                           FOR JSON PATH
                       ) AS [Groups],
                       COUNT(*) OVER () as [FilteredCount]
                FROM [dbo].[Departments] dp (NOLOCK)
                WHERE [DeletedAt] IS NULL
                      AND (
                              [dp].[Name] LIKE '%' + @SearchValue + '%'
                              OR
                              (
                                  SELECT ([us].[FirstName] + SPACE(1) + [us].[LastName])
                                  FROM [dbo].[Users] us (NOLOCK)
                                  WHERE [dp].[DepartmentMangerId] = [us].[Id]
                              ) LIKE '%' + @SearchValue + '%'
                              OR
                              (
                                  SELECT ([gp].[Name])
                                  FROM [dbo].[Groups] gp (NOLOCK)
                                  WHERE [gp].[DepartmentId] = [dp].[Id]
                                  FOR JSON PATH
                              ) LIKE '%' + @SearchValue + '%'
                              OR EXISTS
                (
                    SELECT 1
                    FROM [dbo].[Groups] gp (NOLOCK)
                        JOIN [dbo].[Users] us (NOLOCK)
                            ON [gp].[GroupLeaderId] = [us].[Id]
                    WHERE [gp].[DepartmentId] = [dp].[Id]
                          AND CONCAT(   [gp].[Name],
                              (
                                  SELECT CONCAT([us].[FirstName], ' ', [us].[LastName])
                                  FROM [dbo].[Users] us (NOLOCK)
                                  WHERE [gp].[GroupLeaderId] = [us].[Id]
                              )
                                    ) LIKE '%' + @SearchValue + '%'
                )
                              OR EXISTS
                (
                    SELECT 1
                    FROM [dbo].[Groups] gp (NOLOCK)
                        JOIN [dbo].[Users] us (NOLOCK)
                            ON [us].[GroupId] = [gp].[Id]
                    WHERE [gp].[DepartmentId] = [dp].[Id]
                          AND (
                                  [us].[FirstName] LIKE '%' + @SearchValue + '%'
                                  OR [us].[LastName] LIKE '%' + @SearchValue + '%'
                              )
                )
                          )
                ORDER BY CASE
                             WHEN
                             (
                                 @SortColumn = 'Name'
                                 AND @SortOrder = 'ASC'
                             ) THEN
                                 Name
                         END ASC,
                         CASE
                             WHEN
                             (
                                 @SortColumn = 'Name'
                                 AND @SortOrder = 'DESC'
                             ) THEN
                                 Name
                         END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
                FOR JSON PATH
            ) as [UserByIdOut]
       )
    SELECT *
    FROM Users_Results
    OPTION (RECOMPILE)
END");
            #endregion

            #region GetDigitalAssistantsById
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetDigitalAssistantsById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [CustomerId],
           [Name],
           [Details],
           [Icon]
    FROM [dbo].[CustomerDigitalAssistants] WITH (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END ");
            #endregion

            #region GetDisplayOrder
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetDisplayOrder] 
(
	@ReferenceId INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [ScreenName],
           [JsonData],
           [RefrenceId],
           [IsActive],
           [CreatedAt],
           [UpdateAt],
           [DeletedAt],
           [CreatedBy]
    FROM [dbo].[ScreenDisplayOrderAndStatuses] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [RefrenceId] = @ReferenceId
END");
            #endregion

            #region GetExtradetails
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetExtradetails]
(
    @CustomerAppBuliderId INT = 0,
    @CustomerId iNT = 0,
    @GuestService NVARCHAR(100) = ''
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT CASE
               WHEN @GuestService = 'Reception' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppReceptionItems] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
               )
               WHEN @GuestService = 'Enhance your Stay' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppEnhanceYourStayItems]
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
               )
               WHEN @GuestService = 'Room Service' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppRoomServiceItems] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
               )
               WHEN @GuestService = 'Concierge' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppConciergeItems]
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
               )
               WHEN @GuestService = 'Housekeeping' Then
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppHousekeepingItems]
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
               )
           END AS [Items],
           CASE
               WHEN @GuestService = 'Reception' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppReceptionCategories] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
               )
               WHEN @GuestService = 'Enhance your Stay' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppEnhanceYourStayCategories] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
               )
               WHEN @GuestService = 'Room Service' THEN
               (
                   Select Count(*)
                   FROM [dbo].[CustomerGuestAppRoomServiceCategories] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
               )
               WHEN @GuestService = 'Concierge' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppConciergeCategories] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
               )
               WHEN @GuestService = 'Housekeeping' THEN
               (
                   SELECT Count(*)
                   FROM [dbo].[CustomerGuestAppHousekeepingCategories] (NOLOCK)
                   WHERE [CustomerGuestAppBuilderId] = @CustomerAppBuliderId
               )
           END AS [Categories]
    FROM [dbo].[CustomerGuestAppBuilders] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [dbo].[CustomerGuestAppBuilders].[Id] = @CustomerAppBuliderId
          AND [dbo].[CustomerGuestAppBuilders].[CustomerId] = @CustomerId
END");
            #endregion

            #region GetGroupById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetGroupById] 
(
	@Id INT = 0
)
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
           [DepartmentId],
           [IsActive],
           [GroupLeaderId]
    FROM [dbo].[Groups] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetGroups
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetGroups]
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
           [DepartmentId],
           [IsActive],
           [GroupLeaderId]
    FROM [dbo].[Groups]
    WHERE [DeletedAt] IS NULL
END");
            #endregion

            #region GetGroupsByDepartmentId
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetGroupsByDepartmentId] 
(
	@DepartmentId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [dbo].[Groups].[Id],
           [dbo].[Groups].[Name]
    FROM [dbo].[Groups] (NOLOCK)
    WHERE [dbo].[Groups].[DeletedAt] IS NULL
          AND [dbo].[Groups].[DepartmentId] = @DepartmentId 
          AND [dbo].[Groups].[IsActive] = 1
END ");
            #endregion

            #region GetGuestJourneyMessagesTemplates
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetGuestJourneyMessagesTemplates]
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [TempleteType],
           [Name],
           [TempletMessage],
           [IsActive]
    FROM [dbo].[GuestJourneyMessagesTemplates] (NOLOCK)
    WHERE [DeletedAt] IS NULL
END");
            #endregion

            #region GetGuestJourneyMessagesTemplatesById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetGuestJourneyMessagesTemplatesById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [TempleteType],
           [Name],
           [TempletMessage],
           [IsActive]
    FROM [dbo].[GuestJourneyMessagesTemplates] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetGuestJourneyMessagesTemplatesForCustomer
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetGuestJourneyMessagesTemplatesForCustomer]
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [TempleteType],
           [Name],
           [TempletMessage],
           [IsActive]
    FROM [dbo].[GuestJourneyMessagesTemplates] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [IsActive] = 1
END");
            #endregion

            #region GetGuestRequests
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetGuestRequests]
(
    @Id INT = 0,
    @CustomerId INT = 0,
    @SortColumn NVARCHAR(20) = 'TaskStatus',
    @SortOrder NVARCHAR(5) = 'ASC',
    @PageNo INT = 1,
    @PageSize INT = 10
)
AS
BEGIN
    IF @Id != 0
    BEGIN
        SELECT [guestrequest].[Id],
               [customerguest].[Firstname],
               [customerguest].[Lastname],
               [guestrequest].[Status] as [TaskStatus],
               [customerroomname].[Name] as [Room],
               [guestrequest].[RequestType],
               [enhanceyourstay].[ShortDescription] as [EnhanceYourStayItem],
               [enhanceyourstay].[Price] as [EnhanceYourStayItemPrice],
               [housekeeping].[Name] as [HouseKeepingItem],
               [housekeeping].[Price] as [HouseKeepingItemPrice],
               [concierge].[Name] as [ConciergeItem],
               [concierge].[Price] as [ConciergeItemPrice],
               [reception].[Name] as [ReceptionItem],
               [reception].[Price] as [ReceptionItemPrice],
               [roomservice].[Name] as [RoomServiceItem],
               [roomservice].[Price] as [RoomServiceItemPrice],
               [customerguest].[Rating],
               [guestrequest].[MonthValue],
               [guestrequest].[DayValue],
               [guestrequest].[YearValue],
               [guestrequest].[HourValue],
               [guestrequest].[MinuteValue],
               [guestrequest].[CreatedAt] as [TImeStamp],
               [guestrequest].[UpdateAt]
        FROM [dbo].[GuestRequests] guestrequest WITH (NOLOCK)
            INNER JOIN [dbo].[CustomerGuests] customerguest WITH (NOLOCK)
                ON [customerguest].[Id] = [guestrequest].[GuestId]
            INNER JOIN [dbo].[CustomerRoomNames] customerroomname WITH (NOLOCK)
                ON [customerroomname].[CustomerId] = [guestrequest].[CustomerId]
            LEFT JOIN [dbo].[CustomerGuestAppEnhanceYourStayItems] enhanceyourstay WITH (NOLOCK)
                ON [enhanceyourstay].[Id] = [guestrequest].[CustomerGuestAppEnhanceYourStayItemId]
            LEFT JOIN [dbo].[CustomerGuestAppHousekeepingItems] housekeeping WITH (NOLOCK)
                ON [housekeeping].[Id] = [guestrequest].[CustomerGuestAppHousekeepingItemId]
            LEFT JOIN [dbo].[CustomerGuestAppConciergeItems] concierge WITH (NOLOCK)
                ON [concierge].[Id] = [guestrequest].[CustomerGuestAppConciergeItemId]
            LEFT JOIN [dbo].[CustomerGuestAppReceptionItems] reception WITH (NOLOCK)
                ON [reception].[Id] = [guestrequest].[CustomerGuestAppReceptionItemId]
            LEFT JOIN [dbo].[CustomerGuestAppRoomServiceItems] roomservice WITH (NOLOCK)
                ON [roomservice].[Id] = [guestrequest].[CustomerGuestAppRoomServiceItemId]
        WHERE [guestrequest].[Id] = @Id
              AND [guestrequest].[DeletedAt] IS NULL
    END
    IF @CustomerId != 0
    BEGIN
        SELECT [guestrequest].[Id],
               [customerguest].[Firstname],
               [customerguest].[Lastname],
               [guestrequest].[Status] as [TaskStatus],
               [customerroomname].[Name] as [Room],
               [guestrequest].[RequestType],
               [enhanceyourstay].[ShortDescription] as [EnhanceYourStayItem],
               [enhanceyourstay].[Price] as [EnhanceYourStayItemPrice],
               [housekeeping].[Name] as [HouseKeepingItem],
               [housekeeping].[Price] as [HouseKeepingItemPrice],
               [concierge].[Name] as [ConciergeItem],
               [concierge].[Price] as [ConciergeItemPrice],
               [reception].[Name] as [ReceptionItem],
               [reception].[Price] as [ReceptionItemPrice],
               [roomservice].[Name] as [RoomServiceItem],
               [roomservice].[Price] as [RoomServiceItemPrice],
               [customerguest].[Rating],
               [guestrequest].[MonthValue],
               [guestrequest].[DayValue],
               [guestrequest].[YearValue],
               [guestrequest].[HourValue],
               [guestrequest].[MinuteValue],
               [guestrequest].[CreatedAt] as [TImeStamp],
               [guestrequest].[UpdateAt],
               [dbo].GetGuestsStatus(customerguest.Id) as [GuestStatus],
               COUNT(*) OVER () as [FilteredCount]
        FROM [dbo].[GuestRequests] guestrequest WITH (NOLOCK)
            INNER JOIN [dbo].[CustomerGuests] customerguest WITH (NOLOCK)
                ON [customerguest].[Id] = [guestrequest].[GuestId]
            INNER JOIN [dbo].[CustomerRoomNames] customerroomname WITH (NOLOCK)
                ON [customerroomname].[CustomerId] = [guestrequest].[CustomerId]
            LEFT JOIN [dbo].[CustomerGuestAppEnhanceYourStayItems] enhanceyourstay WITH (NOLOCK)
                ON [enhanceyourstay].[Id] = [guestrequest].[CustomerGuestAppEnhanceYourStayItemId]
            LEFT JOIN [dbo].[CustomerGuestAppHousekeepingItems] housekeeping WITH (NOLOCK)
                ON [housekeeping].[Id] = [guestrequest].[CustomerGuestAppHousekeepingItemId]
            LEFT JOIN [dbo].[CustomerGuestAppConciergeItems] concierge WITH (NOLOCK)
                ON [concierge].[Id] = [guestrequest].[CustomerGuestAppConciergeItemId]
            LEFT JOIN [dbo].[CustomerGuestAppReceptionItems] reception WITH (NOLOCK)
                ON [reception].[Id] = [guestrequest].[CustomerGuestAppReceptionItemId]
            LEFT JOIN [dbo].[CustomerGuestAppRoomServiceItems] roomservice WITH (NOLOCK)
                ON [roomservice].[Id] = [guestrequest].[CustomerGuestAppRoomServiceItemId]
        WHERE [guestrequest].[CustomerId] = @CustomerId
              AND [guestrequest].[DeletedAt] IS NULL
        ORDER BY CASE
                     WHEN
                     (
                         @SortColumn = 'TaskStatus'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         guestrequest.Status
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'TaskStatus'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         guestrequest.Status
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Room'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         customerroomname.Name
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Room'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         customerroomname.Name
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Department'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         guestrequest.RequestType
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Department'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         guestrequest.RequestType
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'TimeStamp'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         guestrequest.CreatedAt
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'TimeStamp'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         guestrequest.CreatedAt
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'GuestStatus'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         dbo.GetGuestsStatus(customerguest.Id)
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'GuestStatus'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         dbo.GetGuestsStatus(customerguest.Id)
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
    END

END");
            #endregion

            #region GetLeadById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetLeadById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [FirstName],
           [LastName],
           [Email],
           [Comment],
           [PhoneCountry],
           [PhoneNumber],
           [ContactFor],
           [IsActive],
           [Company]
    FROM [dbo].[Leads] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id

END");
            #endregion

            #region GetLeads
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetLeads] 
(
    @SearchValue NVARCHAR(50) = '',
    @PageNo INT = 1,
    @PageSize INT = 10, --NoOf Record To Get
    @SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC',
    @AlphabetsStartsWith NVARCHAR(50) = ''
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchValue = LTRIM(RTRIM(@SearchValue))
    SET @AlphabetsStartsWith = LTRIM(RTRIM(@AlphabetsStartsWith));
    WITH CTE_Results
    AS (SELECT [Id],
               ISNULL([FirstName], '') + SPACE(1) + ISNULL([LastName], '') AS [Name],
               [Email],
               [Comment],
               [PhoneNumber],
               [ContactFor],
               [IsActive],
               [CreatedAt],
               [UpdateAt],
               [Company],
               COUNT(*) OVER () as [FilteredCount]
        FROM [dbo].[Leads] WITH (NOLOCK)
        WHERE [DeletedAt] IS NULL
              AND (
                      [FirstName] LIKE '%' + @SearchValue + '%'
                      OR [LastName] LIKE '%' + @SearchValue + '%'
                      OR [Company] LIKE '%' + @SearchValue + '%'
                      OR [PhoneNumber] LIKE '%' + @SearchValue + '%'
                      OR [Email] LIKE '%' + @SearchValue + '%'
                  )
              AND (
                      @AlphabetsStartsWith IS NULL
                      OR EXISTS
        (
            SELECT 1
            FROM STRING_SPLIT(@AlphabetsStartsWith, ',') AS s
            WHERE [FirstName] LIKE LTRIM(RTRIM(s.value)) + '%'
        )
                  )
        ORDER BY CASE
                     WHEN @SortColumn = 'Name'
                          AND @SortOrder = 'ASC' THEN
                         ISNULL(FirstName, '') + ' ' + ISNULL(LastName, '')
                 END ASC,
                 CASE
                     WHEN @SortColumn = 'Name'
                          AND @SortOrder = 'DESC' THEN
                         ISNULL(FirstName, '') + ' ' + ISNULL(LastName, '')
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Company'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Company
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Company'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Company
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'PhoneNumber'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         PhoneNumber
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'PhoneNumber'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         PhoneNumber
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Email'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Email
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Email'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Email
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [Name],
           [Email],
           [Comment],
           [PhoneNumber],
           [ContactFor],
           [IsActive],
           [CreatedAt],
           [UpdateAt],
           [Company],
           [FilteredCount]
    FROM CTE_Results
    OPTION (RECOMPILE)

END");
            #endregion

            #region GetModuleJson_sample
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetModuleJson_sample]
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [m].[Id],
                   [m].[Name],
                   [m].[ModuleType],
                   [m].[IsActive],
                   [m].[CreatedAt],
                   [m].[UpdateAt],
                   [m].[DeletedAt],
                   [m].[CreatedBy],
                   JSON_QUERY(
                   (
                       SELECT [Id],
                              [ModuleId],
                              [Name],
                              [IsActive],
                              [CreatedAt],
                              [UpdateAt],
                              [DeletedAt],
                              [CreatedBy]
                       FROM [dbo].[ModuleServices]
                       WHERE [ModuleId] = [m].[Id]
                             AND [DeletedAt] IS NULL
                       FOR JSON PATH
                   )
                             ) as [ModuleServices]
            FROM [dbo].[Modules] m (NOLOCK)
            WHERE [DeletedAt] IS NULL
            FOR JSON PATH
        )
END");
            #endregion

            #region GetModules
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetModules]
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
           [ModuleType],
           [IsActive]
    FROM [dbo].[Modules] (NOLOCK)
    WHERE [DeletedAt] IS NULL
END");
            #endregion

            #region GetModules_sample
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetModules_sample]
(
    @PageNo INT = 1,
    @PageSize INT = 10 --NoOf Record To Get
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
           [ModuleType],
           [IsActive],
           [CreatedAt],
           [UpdateAt],
           [DeletedAt],
           [CreatedBy],
           COUNT(*) OVER () AS [TotalCount]
    FROM [dbo].[Modules] WITH (NOLOCK)
    WHERE [DeletedAt] IS NULL
    ORDER BY [Name] OffSet @PageSize * (@PageNo - 1) Rows Fetch Next @PageSize Rows Only
END");
            #endregion

            #region GetModuleServices
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetModuleServices]
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [ModuleId],
           [Name],
           [IsActive]
    FROM [dbo].[ModuleServices] (NOLOCK)
    WHERE [DeletedAt] IS NULL
END
");
            #endregion

            #region GetModuleServicesById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetModuleServicesById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [ModuleId],
           [Name],
           [IsActive]
    FROM [dbo].[ModuleServices] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetModulesListV2_sample
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetModulesListV2_sample] 
(
    @SearchColumn NVARCHAR(50) = NULL,
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchColumn = LTRIM(RTRIM(@SearchColumn))
    SET @SearchValue = LTRIM(RTRIM(@SearchValue));
    WITH CTE_Results
    AS (SELECT [Id],
               [Name],
               [ModuleType],
               [IsActive],
               COUNT(*) OVER () as FilteredCount
        FROM [dbo].[Modules] WITH (NOLOCK)
        WHERE @SearchColumn = ''
              OR (CASE @SearchColumn
                      WHEN 'Name' THEN
                          Name
                  END
                 ) LIKE '%' + @SearchValue + '%'
        ORDER BY CASE
                     WHEN
                     (
                         @SortColumn = 'Name'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Name
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Name'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Name
                 END DESC
       OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [Name],
           [ModuleType],
           [IsActive],
           [FilteredCount]
    FROM CTE_Results
    OPTION (RECOMPILE)

END");
            #endregion

            #region GetNotifications
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetNotifications]
(
    @PageNo INT = 1,
    @PageSize INT = 10,
    @CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON

    IF @CustomerId != 0
    BEGIN
        ;WITH FilteredNotifications
         AS (SELECT [Notifications].[Id],
                    [Title],
                    [Message],
                    [Notifications].[CreatedAt],
                    COUNT(*) OVER () as [FilteredCount]
             FROM [dbo].[Notifications] WITH (NOLOCK)
                 INNER JOIN [dbo].[NotificationHistorys] (NOLOCK)
                     ON [dbo].[Notifications].[Id] = [NotificationHistorys].[NotificationId]
             WHERE [Notifications].[DeletedAt] IS NULL
                   AND [dbo].[NotificationHistorys].[CustomerId] = @CustomerId
            )
        SELECT [Id],
               [Title],
               [Message],
               [CreatedAt],
               [FilteredCount]
        FROM FilteredNotifications
        ORDER BY [CreatedAt] DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;
    END
    ELSE
    BEGIN
        ;WITH AllNotifications
         AS (SELECT [Id],
                    [Title],
                    [Message],
                    [CreatedAt],
                    COUNT(*) OVER () as [FilteredCount]
             FROM [dbo].[Notifications] WITH (NOLOCK)
             WHERE [Notifications].[DeletedAt] IS NULL
            )
        SELECT [Id],
               [Title],
               [Message],
               [CreatedAt],
               [FilteredCount]
        FROM AllNotifications
        ORDER BY [CreatedAt] DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;
    END
END");
            #endregion

            #region GetPaymentProcessorById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetPaymentProcessorById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
           [IsActive]
    FROM [dbo].[PaymentProcessors] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetPaymentProcessors
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetPaymentProcessors]
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
           [IsActive]
    FROM [dbo].[PaymentProcessors] (NOLOCK)
    WHERE [DeletedAt] IS NULL
END");
            #endregion

            #region GetProductNames
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetProductNames]
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [dbo].[Products].[Id],
           [dbo].[Products].[Name]
    FROM [dbo].[Products] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [IsActive] = 1
END");
            #endregion

            #region GetProducts
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetProducts]
(
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10,
    @SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC'
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchValue = LTRIM(RTRIM(@SearchValue));
    WITH Product_Results
    AS (SELECT [dbo].[Products].[Id],
               [dbo].[Products].[Name],
               [dbo].[Products].[CreatedAt],
               ([dbo].[Users].[FirstName] + SPACE(1) + [dbo].[Users].[LastName]) as [CreatedBy],
               [dbo].[Products].[IsActive],
               COUNT(*) OVER () as [TotalCount]
        FROM [dbo].[Products] (NOLOCK)
            INNER JOIN [dbo].[Users] (NOLOCK)
                ON [dbo].[Products].[CreatedBy] = [dbo].[Users].[Id]
        WHERE [dbo].[Products].[DeletedAt] IS NULL
              AND (
                      [dbo].[Products].[Name] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[Products].[CreatedAt] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[Users].[FirstName] LIKE '%' + @SearchValue + '%'
                      OR [dbo].[Users].[LastName] LIKE '%' + @SearchValue + '%'
                  )
        ORDER BY CASE
                     WHEN
                     (
                         @SortColumn = 'Name'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Name
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Name'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Name
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'CreatedAt'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Name
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'CreatedAt'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Name
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'FirstName'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Name
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'FirstName'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Name
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'IsActive'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         Name
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'IsActive'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         Name
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [Name],
           [CreatedAt],
           [CreatedBy],
           [IsActive],
           [TotalCount]
    FROM Product_Results
    OPTION (RECOMPILE)

END");
            #endregion

            #region GetQaCategories
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetQaCategories]
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
           [IsActive]
    FROM [dbo].[QuestionAnswerCategories] (NOLOCK)
    WHERE [DeletedAt] IS NULL
END");
            #endregion

            #region GetQaCategoryById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetQaCategoryById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
           [IsActive]
    FROM [dbo].[QuestionAnswerCategories] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetQuestionAnswerById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetQuestionAnswerById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [q].[Id],
                   [q].[QuestionAnswerCategoryId],
                   [q].[Name],
                   [q].[Description],
                   [q].[Icon],
                   [q].[IsActive],
                   [q].[IsPublish],
                   JSON_QUERY(
                   (
                       SELECT [Id],
                              [Attachment],
                              [AttachmentType]
                       FROM [dbo].[QuestionAnswerAttachements] (NOLOCK)
                       WHERE [DeletedAt] IS NULL
                             AND [QuestionAnswerId] = [q].[Id]
                       FOR JSON PATH
                   )
                             ) as [QuestionAnswerAttachements]
            FROM [dbo].[QuestionAnswers] (NOLOCK) q
            WHERE [DeletedAt] IS NULL
                  AND [q].[Id] = @Id
            FOR JSON PATH
        )
END");
            #endregion

            #region GetQuestionAnswers
            migrationBuilder.Sql(@"CREATE OR ALTER PROC [dbo].[GetQuestionAnswers]
(
    @SearchValue NVARCHAR(50) = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10, --NoOf Record To Get
    @SortColumn NVARCHAR(20) = 'Name',
    @SortOrder NVARCHAR(5) = 'ASC',
    @CategoryId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON

    SET @SearchValue = LTRIM(RTRIM(@SearchValue));
    WITH CTE_Results
    AS (SELECT [QA].[Id],
               [QA].[QuestionAnswerCategoryId],
               [QA].[Name],
               [QA].[Description],
               [QA].[Icon],
               [QA].[IsActive],
               [QA].[IsPublish],
               [QAA].[Id] AS [QuestionAnswerAttachementId],
               [QAA].[AttachmentType],
               [QAA].[Attachment],
               COUNT(*) OVER () as [FilteredCount]
        FROM [dbo].[QuestionAnswers] QA WITH (NOLOCK)
            INNER JOIN [dbo].[QuestionAnswerAttachements] QAA WITH (NOLOCK)
                ON [QA].[Id] = [QAA].[QuestionAnswerId]
        WHERE [QA].[DeletedAt] IS NULL
              AND (
                      [QA].[QuestionAnswerCategoryId] = @CategoryId
                      OR 0 = @CategoryId
                  )
              AND (
                      [Name] LIKE '%' + @SearchValue + '%'
                      OR [Description] LIKE '%' + @SearchValue + '%'
                  )
        ORDER BY CASE
                     WHEN
                     (
                         @SortColumn = 'Name'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         [Name]
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Name'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         [Name]
                 END DESC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Description'
                         AND @SortOrder = 'ASC'
                     ) THEN
                         [Description]
                 END ASC,
                 CASE
                     WHEN
                     (
                         @SortColumn = 'Description'
                         AND @SortOrder = 'DESC'
                     ) THEN
                         [Description]
                 END DESC OFFSET @PageSize * (@PageNo - 1) ROWS FETCH NEXT @PageSize ROWS ONLY
       )
    SELECT [Id],
           [QuestionAnswerCategoryId],
           [Name],
           [Description],
           [Icon],
           [IsActive],
           [IsPublish],
           [FilteredCount],
           [QuestionAnswerAttachementId],
           [AttachmentType],
           [Attachment]
    FROM CTE_Results
    OPTION (RECOMPILE)

END");
            #endregion

            #region GetHospitioPaymentProcessorById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetHospitioPaymentProcessorById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [PaymentProcessorId],
           [ClientId],
           [ClientSecret],
           [Currency],
           [IsActive]
    FROM [dbo].[HospitioPaymentProcessors] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Id] = @Id
END");
            #endregion

            #region GetHospitioPaymentProcessors
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetHospitioPaymentProcessors]
(
    @PageNo INT = 1,
    @PageSize INT = 10 --NoOf Record To Get
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [PaymentProcessorId],
           [ClientId],
           [ClientSecret],
           [Currency],
           [IsActive]
    FROM [dbo].[HospitioPaymentProcessors] WITH (NOLOCK)
    WHERE [DeletedAt] IS NULL
    ORDER BY [PaymentProcessorId] OffSet @PageSize * (@PageNo - 1) Rows Fetch Next @PageSize Rows Only
END");
            #endregion

            #region GetRecentTicketByCustomerId
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetRecentTicketByCustomerId] 
(
	@CustomerId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT TOP 5
        [Id],
        [CustomerId],
        [Title],
        [Details],
        [Priority],
        [Duedate],
        [TicketCategoryId],
        [CSAgentId],
        [Status],
        [CloseDate],
        [CreatedFrom],
        [IsActive],
        [CreatedAt]
    FROM [dbo].[Tickets] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [Status] != 3
          AND (
                  [CustomerId] = @CustomerId
                  OR 0 = @CustomerId
              )
    ORDER BY [CreatedAt] DESC
END");
            #endregion

            #region GetTicketById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetTicketById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [t].[Id],
                   [t].[Title],
                   [t].[Details],
                   [t].[CloseDate],
                   [t].[CreatedFrom],
                   [t].[CSAgentId],
                   CASE
                       WHEN [t].[CSAgentId] IS NULL THEN
                           ''
                       ELSE
                           ISNULL([U].[FirstName], '') + SPACE(1) + ISNULL([U].[LastName], '')
                   END AS [CSAgentName],
                   [t].[CustomerId],
                   ISNULL([C].[BusinessName], '') as [CustomerName],
                   [t].[Duedate],
                   [t].[Priority],
                   [t].[Status],
                   [t].[CreatedAt],
                   JSON_QUERY(
                   (
                       SELECT [TR].[Id],
                              [TR].[Reply],
                              [TR].[TicketId],
                              [TR].[CreatedAt],
                              [TR].[CreatedBy],
                              [TR].[CreatedFrom],
                              CONCAT_WS(
                                           ' ',
                                           COALESCE([U].[FirstName], [CU].[FirstName]),
                                           COALESCE([U].[LastName], [CU].[LastName])
                                       ) AS [UserName]
                       FROM [dbo].[TicketReplies] TR (NOLOCK)
                           LEFT JOIN [dbo].[Users] U (NOLOCK)
                               ON [TR].[CreatedBy] = 1
                                  AND [U].[Id] = [TR].[CreatedFrom]
                           LEFT JOIN [dbo].[CustomerUsers] CU (NOLOCK)
                               ON [TR].[CreatedBy] = 2
                                  AND [CU].[Id] = [TR].[CreatedFrom]
                       WHERE [TR].[TicketId] = [t].[Id]
                             AND [TR].[DeletedAt] IS NULL
                       FOR JSON PATH
                   )
                             ) as [Replies]
            FROM [dbo].[Tickets] t (NOLOCK)
                LEFT JOIN [dbo].[Users] U (NOLOCK)
                    ON [U].[Id] = [t].[CSAgentId]
                INNER JOIN [dbo].[Customers] C (NOLOCK)
                    ON [C].[Id] = [t].[CustomerId]
            WHERE [t].[DeletedAt] IS NULL
                  AND [t].[Id] = @Id 
            FOR JSON PATH
        );

END");
            #endregion

            #region GetTicketsV2
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetTicketsV2]
(
    @CategoryId INT = 0,
    @Status INT = 0,
    @Priority INT = 0,
    @CustomerId INT = 0,
    @CSAgentId INT = 0,
    @FromCreate DATETIME = NULL,
    @ToCreate DATETIME = NULL,
    @FromClose DATETIME = NULL,
    @ToClose DATETIME = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10,
    @ShortBy TINYINT = 1, -- on CreateAt 1=Short by Date,2=Short By Month,3=Short By Year (modify in feture if need)
    @CreatedFrom TINYINT  --1 Fro Radfy ,2 for Customer
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [dbo].[Tickets].[Id],
           [dbo].[Tickets].[CustomerId],
           [dbo].[Customers].[BusinessName],
           [dbo].[Customers].[Cname] as [CustomerName],
           [dbo].[Tickets].[Title],
           [dbo].[Tickets].[Details],
           [dbo].[Tickets].[Priority],
           [dbo].[Tickets].[Duedate],
           [dbo].[Tickets].[Status],
           [dbo].[Tickets].[CloseDate],
           [dbo].[Tickets].[CreatedFrom],
           CONCAT(ISNULL([dbo].[Users].[FirstName], ''), ' ', ISNULL([dbo].[Users].[LastName], '')) as [CSAgentName],
           [dbo].[TicketCategorys].[CategoryName] as [TicketCategoryName]
    FROM [dbo].[Tickets] WITH (NOLOCK)
        INNER JOIN [dbo].[TicketCategorys] WITH (NOLOCK)
            ON [dbo].[Tickets].[TicketCategoryId] = [dbo].[TicketCategorys].[Id]
               AND [dbo].[TicketCategorys].[DeletedAt] IS NULL
        INNER JOIN [dbo].[Users] WITH (NOLOCK)
            ON [dbo].[Tickets].[CSAgentId] = [dbo].[Users].[Id]
               AND [dbo].[Users].[DeletedAt] IS NULL
        INNER JOIN [dbo].[Customers] WITH (NOLOCK)
            ON [dbo].[Tickets].[CustomerId] = [dbo].[Customers].[Id]
               AND [dbo].[Customers].[DeletedAt] IS NULL
    WHERE ([dbo].[Tickets].[DeletedAt] IS NULL)
          AND (
                  [dbo].[Tickets].[TicketCategoryId] = @CategoryId
                  OR @CategoryId = 0
              )
          AND (
                  [dbo].[Tickets].[Status] = @Status
                  OR @Status = 0
              )
          AND (
                  [dbo].[Tickets].[Priority] = @Priority
                  OR @Priority = 0
              )
          AND (
                  [dbo].[Tickets].[CustomerId] = @CustomerId
                  OR @CustomerId = 0
              )
          AND (
                  [dbo].[Tickets].[CSAgentId] = @CSAgentId
                  OR @CSAgentId = 0
              )
          AND (
                  [dbo].[Tickets].[CreatedFrom] = @CreatedFrom
                  OR @CreatedFrom = 0
              )
          AND (
                  ([dbo].[Tickets].[CreatedAt]
          BETWEEN @FromCreate AND @ToCreate
                  )
                  OR (
                         @FromCreate IS NULL
                         AND @ToCreate IS NULL
                     )
              )
          AND (
                  ([dbo].[Tickets].[CloseDate]
          BETWEEN @FromClose AND @ToClose
                  )
                  OR (
                         @FromClose IS NULL
                         AND @ToClose IS NULL
                     )
              )
    ORDER BY CASE
                 WHEN @ShortBy = 1 THEN
                     [dbo].[Tickets].[CreatedAt]
                 WHEN @ShortBy = 2 THEN
                     MONTH([dbo].[Tickets].[CreatedAt])
                 WHEN @ShortBy = 3 THEN
                     YEAR([dbo].[Tickets].[CreatedAt])
             END Desc OffSet @PageSize * (@PageNo - 1) Rows Fetch Next @PageSize Rows Only

END");
            #endregion

            #region GetUserById
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetUserById] 
(
	@Id INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [u].[Id],
                   [u].[FirstName],
                   [u].[LastName],
                   [u].[Email],
                   [u].[Title],
                   [u].[ProfilePicture],
                   [u].[PhoneCountry],
                   [u].[PhoneNumber],
                   [u].[DepartmentId],
                   [u].[UserLevelId],
                   [u].[SupervisorId],
                   [u].[UserName],
                   [u].[Password],
                   [u].[IsActive],
                   [u].[GroupId],
                   JSON_QUERY(
                   (
                       SELECT [Id],
                              [PermissionId],
                              [UserId],
                              [IsView],
                              [IsEdit],
                              [IsUpload],
                              [IsReply],
                              [IsSend],
                              [IsActive]
                       FROM [dbo].[UsersPermissions] (NOLOCK)
                       WHERE [DeletedAt] IS NULL
                             AND [UserId] = [u].[Id]
                       FOR JSON PATH
                   )
                             ) as [UserModulePermissions]
            FROM [dbo].[Users] u (NOLOCK)
            WHERE [u].[DeletedAt] IS NULL
                  AND [u].[Id] = @Id
            FOR JSON PATH
        )
END");
            #endregion

            #region GetUserLevels
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetUserLevels] 
(
	@IsHospitioUserType BIT = 'true'
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [LevelName],
           [NormalizedLevelName],
           [IsHospitioUserType],
           [IsActive]
    FROM [dbo].[UserLevels] (NOLOCK)
    WHERE [DeletedAt] IS NULL
          AND [IsHospitioUserType] = @IsHospitioUserType 
          AND [LevelName] != 'Super Admin'
END");
            #endregion

            #region GetUsers
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetUsers]
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT
        (
            SELECT [Id],
                   [FirstName],
                   [LastName],
                   [Email],
                   [Title],
                   [ProfilePicture],
                   [PhoneCountry],
                   [PhoneNumber],
                   (
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
            ORDER BY [us].[Id]
            FOR JSON PATH
        ) as UserOut
END");
            #endregion

            #region GetUsersByGroupId
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetUsersByGroupId] 
(
	@GroupId INT = 0
)
AS
BEGIN
    SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [dbo].[Users].[Id],
           CONCAT(ISNULL([FirstName], ''), ' ', ISNULL([LastName], '')) AS [Name]
    FROM [dbo].[Users] (NOLOCK)
    WHERE [dbo].[Users].[DeletedAt] IS NULL
          AND [dbo].[Users].[GroupId] = @GroupId
          AND [dbo].[Users].[IsActive] = 1
END");
            #endregion

            #region GetPermissions
            migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE [dbo].[GetPermissions]
AS
BEGIN
	SET NOCOUNT ON
    SET XACT_ABORT ON

    SELECT [Id],
           [Name],
		   [NormalizedName],
		   [IsACtive]
    FROM [dbo].[Permissions] (NOLOCK)
	WHERE [DeletedAt] IS NULL
END"); 
            #endregion


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
