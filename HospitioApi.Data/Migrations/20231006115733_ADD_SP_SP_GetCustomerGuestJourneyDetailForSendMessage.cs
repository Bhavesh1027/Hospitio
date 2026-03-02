using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ADD_SP_SP_GetCustomerGuestJourneyDetailForSendMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
/****** Object:  StoredProcedure [dbo].[SP_GetCustomerGuestJourneyDetailForSendMessage_v5]    Script Date: 06-10-2023 17:22:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER 

       PROCEDURE [dbo].[SP_GetCustomerGuestJourneyDetailForSendMessage] --'2023-06-28 11:56:52.333',8
(
	@SendDateTime DATETIME = NULL,
	@CustomerId INT = 0
)
AS
BEGIN

    SET NOCOUNT ON
    SET XACT_ABORT ON

    DECLARE @JourneyStep INT,
			@TemplateName NVARCHAR(MAX),
            @TimingOption1 INT,
            @TimingOption2 INT,
            @TimingOption3 INT,
            @Timing INT,
            @NotificationDays NVARCHAR(50),
            @NotificationTime TIME,
            @TempletMessage NVARCHAR(MAX),
			@Buttons NVARCHAR(MAX),
			@VonageTemplateId NVARCHAR(MAX),--
			@VonageTemplateStatus NVARCHAR(MAX),--
			@CustomerUserId int

    DECLARE @Id INT,
            @Uuid NVARCHAR(32),
            @ReservationNumber NVARCHAR(100),
            @Source NVARCHAR(20),
            @NoOfGuestAdults INT,
            @NoOfGuestChildrens INT,
            @CheckinDate DATETIME,
            @CheckoutDate DATETIME,
            @IsActive bit,
            @CreatedAt DATETIME,
            @UpdateAt DATETIME,
            @DeletedAt DATETIME,
            @CreatedBy INT,
            @isCheckInCompleted BIT,
            @isSkipCheckIn BIT

    DECLARE @MinDiff INT = -5
	DECLARE @DayLimit INT = 72
	DECLARE @FormatStartDate DATETIME =  DATEADD(DAY,-(@DayLimit),@SendDateTime)
	DECLARE @FormatEndDate DATETIME = DATEADD(DAY,@DayLimit,@SendDateTime)

    IF OBJECT_ID('tempdb..#TempTables') IS NOT NULL
        DROP TABLE #TempTables;

    CREATE TABLE #TempTables
    (
        [Id] INT,
        [CustomerId] INT,
		[BussinessName] VARCHAR(MAX),
		[BussinessAddress] NVARCHAR(MAX),
		[BussinessPhoneNumber] NVARCHAR(MAX),
        [JourneyStep] INT,
		[TemplateName] VARCHAR(MAX),
        [Uuid] VARCHAR(32),
        [ReservationNumber] NVARCHAR(100),
        [Source] NVARCHAR(20),
        [NoOfGuestAdults] INT,
        [NoOfGuestChildrens] INT,
        [CheckinDate] DATETIME,
        [CheckoutDate] DATETIME,
		[BookingDays] INT,
        [IsActive] BIT,
        [CreatedAt] DATETIME,
        [isCheckInCompleted] BIT,
        [isSkipCheckIn] BIT,
        [Email] VARCHAR(100),
        [Phone] VARCHAR(20),
        [TempletMessage] VARCHAR(MAX),
		[GuestId] INT,
		[Buttons] VARCHAR(MAX),
		[VonageTemplateId] VARCHAR(MAX),--
		[VonageTemplateStatus] VARCHAR(MAX),--
		[CustomerUserId] int,
		[CustomerWhatsAppNumber] VARCHAR(MAX),
		[APPId] VARCHAR(MAX),
		[PrivateKey] NVARCHAR(MAX),
		[GuestName] NVARCHAR(MAX)
    )

    IF OBJECT_ID('tempdb..#TempTablesReservation') IS NOT NULL
        DROP TABLE #TempTablesReservation;

    CREATE TABLE #TempTablesReservation
    (
        [Id] INT,
        [CustomerId] INT,
        [Uuid] NVARCHAR(32),
        [ReservationNumber] NVARCHAR(100),
        [Source] NVARCHAR(20),
        [NoOfGuestAdults] INT,
        [NoOfGuestChildrens] INT,
        [CheckinDate] DATETIME,
        [CheckoutDate] DATETIME,
		[BookingDays] INT,
        [IsActive] BIT,
        [CreatedAt] DATETIME,
        [UpdateAt] DATETIME,
        [DeletedAt] DATETIME,
        [CreatedBy] INT,
        [isCheckInCompleted] BIT,
        [isSkipCheckIn] BIT,
		[CustomerUserId] int
    )

    DECLARE CustomerJourneyCursor CURSOR FOR
    SELECT DISTINCT
        [CGJ].[Id],
        [CGJ].[CutomerId],
        [CGJ].[JourneyStep],
		[CGJ].[Name],
        [CGJ].[TimingOption1],
        [CGJ].[TimingOption2],
        [CGJ].[TimingOption3],
        [CGJ].[Timing],
        [CGJ].[NotificationDays],
        [CGJ].[NotificationTime],
        [CGJ].[TempletMessage],
		[CGJ].[Buttons],
		[CGJ].[VonageTemplateId],
		[CGJ].[VonageTemplateStatus]
    FROM [dbo].[CustomerGuestJournies] CGJ (NOLOCK)
    WHERE [CGJ].[DeletedAt] IS NULL
          AND [CGJ].[CutomerId] = @CustomerId
    ORDER BY [CGJ].[CutomerId]

    OPEN CustomerJourneyCursor

    FETCH NEXT FROM CustomerJourneyCursor
    INTO @Id,
         @CustomerId,
         @JourneyStep,
		 @TemplateName,
         @TimingOption1,
         @TimingOption2,
         @TimingOption3,
         @Timing,
         @NotificationDays,
         @NotificationTime,
         @TempletMessage,
		 @Buttons,
		 @VonageTemplateId,
		 @VonageTemplateStatus

    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO #TempTablesReservation
        (
            [Id],
            [CustomerId],
            [Uuid],
            [ReservationNumber],
            [Source],
            [NoOfGuestAdults],
            [NoOfGuestChildrens],
            [CheckinDate],
            [CheckoutDate],
			[BookingDays],
            [IsActive],
            [CreatedAt],
            [isCheckInCompleted],
            [isSkipCheckIn],
			CustomerUserId
        )
        SELECT [CR].[Id],
               [CR].[CustomerId],
               [CR].[Uuid],
               [CR].[ReservationNumber],
               [CR].[Source],
               [CR].[NoOfGuestAdults],
               [CR].[NoOfGuestChildrens],
               [CR].[CheckinDate],
               [CR].[CheckoutDate],
			   DATEDIFF(day, [CR].[CheckinDate],[CR].[CheckoutDate]),
               [CR].[IsActive],
               [CR].[CreatedAt],
               [CR].[isCheckInCompleted],
               [CR].[isSkipCheckIn],
			   [CU].Id as CustomerUserId
        FROM [dbo].[CustomerReservations] CR (NOLOCK)
		Inner join [dbo].CustomerUsers CU (NOLOCK)
		ON CU.CustomerId = [CR].CustomerId
        WHERE [CR].[DeletedAt] IS NULL
			AND [CR].[CustomerId] = @CustomerId
			AND CAST([CR].[CreatedAt] AS DATE) BETWEEN CAST(@FormatStartDate AS DATE) AND CAST(@FormatEndDate AS DATE)
			AND CAST([CR].[CheckinDate] AS DATE) BETWEEN CAST(@FormatStartDate AS DATE) AND CAST(@FormatEndDate AS DATE)
			AND CAST([CR].[CheckoutDate] AS DATE) BETWEEN CAST(@FormatStartDate AS DATE) AND CAST(@FormatEndDate AS DATE)
		
        DECLARE @TimingDate DATETIME,
                @ModifyDate DATETIME,
                @DAY INT,
                @HOUR INT

		-- Convert days into minutes
        SET @TimingDate = @SendDateTime
        SET @DAY = (@Timing * 24 * 60)
        SET @HOUR = (@Timing * 60)
		
        -- Booking ----------------------------------------------------------------------------------------------------------------------
        IF (@TimingOption3 = 1)
        BEGIN
            IF (@TimingOption2 = 1)
            BEGIN
                IF (@TimingOption1 = 1) -- 1.DAYS
                BEGIN
                    -- Booking After Days
                    SET @ModifyDate = DATEADD(MINUTE, - (@DAY), @TimingDate)
                END
                ELSE IF (@TimingOption1 = 2) -- 2. HOUR
                BEGIN
                    -- Booking After Hours
                    SET @ModifyDate = DATEADD(MINUTE, - (@HOUR), @TimingDate)
                END
            END
        END
        -- ARRIVAL  ----------------------------------------------------------------------------------------------------------------------
        ELSE IF (@TimingOption3 = 2)
        BEGIN
            SET @TimingDate = @SendDateTime
            IF (@TimingOption2 = 1)
            BEGIN
                IF (@TimingOption1 = 1) -- 1.DAYS
                BEGIN
                    -- Arrival After Days
                    SET @ModifyDate = DATEADD(MINUTE, - (@DAY), @TimingDate)
                END
                ELSE IF (@TimingOption1 = 2) -- 2. HOUR
                BEGIN
                    -- Arrival After Hours
                    SET @ModifyDate = DATEADD(MINUTE, - (@HOUR), @TimingDate)
                END
            END
            ELSE IF (@TimingOption2 = 2)
            BEGIN
                IF (@TimingOption1 = 1) -- 1.DAYS
                BEGIN
                    -- Arrival Before Days
                    SET @ModifyDate = DATEADD(MINUTE, (@DAY), @TimingDate)
                END
                ELSE IF (@TimingOption1 = 2) -- 2. HOUR
                BEGIN
                    -- Arrival Before Hours
                    SET @ModifyDate = DATEADD(MINUTE, (@HOUR), @TimingDate)
                END
            END
        END
        -- DEPARTURE  ----------------------------------------------------------------------------------------------------------------------
        ELSE IF (@TimingOption3 = 3)
        BEGIN
            SET @TimingDate = @SendDateTime
            IF (@TimingOption2 = 1)
            BEGIN
                IF (@TimingOption1 = 1) -- 1.DAYS
                BEGIN
                    -- Departure After Days
                    SET @ModifyDate = DATEADD(MINUTE, - (@DAY), @TimingDate)
                END
                ELSE IF (@TimingOption1 = 2) -- 2. HOUR
                BEGIN
                    -- Departure After Hours
                    SET @ModifyDate = DATEADD(MINUTE, - (@HOUR), @TimingDate)
                END
            END
            ELSE IF (@TimingOption2 = 2)
            BEGIN
                IF (@TimingOption1 = 1) -- 1.DAYS
                BEGIN
                    -- Departure Before Days
                    SET @ModifyDate = DATEADD(MINUTE, (@DAY), @TimingDate)
                END
                ELSE IF (@TimingOption1 = 2) -- 2. HOUR
                BEGIN
                    -- Departure Before Hours
                    SET @ModifyDate = DATEADD(MINUTE, (@HOUR), @TimingDate)
                END
            END
        END

        DECLARE @diffMinuteDate DATETIME
        SET @diffMinuteDate = DATEADD(MINUTE, @MinDiff, @ModifyDate)

        DECLARE @orderBy VARCHAR(30) = ''
        DECLARE @query NVARCHAR(MAX) = ''

        IF (ISNULL(@NotificationDays, '') = '' AND ISNULL(@NotificationTime, '') = '')
        BEGIN
            PRINT 'WITHOUT NOTIFICATION DAYS AND TIME'
            SET @query
                = ' INSERT INTO #TempTables ([Id],[CustomerId],[JourneyStep],[Uuid],[ReservationNumber],[Source],[NoOfGuestAdults],[NoOfGuestChildrens],[CheckinDate],[CheckoutDate],[IsActive],[CreatedAt],[isCheckInCompleted],[isSkipCheckIn],[Email],[Phone],[TempletMessage],[GuestId],[Buttons],[VonageTemplateId],[VonageTemplateStatus],[CustomerUserId],[CustomerWhatsAppNumber],[APPId],[PrivateKey],[TemplateName],[BussinessName],[BussinessAddress],[BookingDays],BussinessPhoneNumber,GuestName)
					SELECT DISTINCT ''' + CAST(@CustomerId AS NVARCHAR(10)) + ''',[TTR].[Id],'''+ CAST(@JourneyStep AS NVARCHAR(10))+ ''',[TTR].[Uuid],[TTR].[ReservationNumber],[TTR].[Source],[TTR].[NoOfGuestAdults],[TTR].[NoOfGuestChildrens],[TTR].[CheckinDate],[TTR].[CheckoutDate],[TTR].[IsActive],[TTR].[CreatedAt],[TTR].[isCheckInCompleted],[TTR].[isSkipCheckIn],[CG].[Email],[CG].[PhoneNumber],'''+ CAST(@TempletMessage AS NVARCHAR(MAX))+ ''',[CG].[Id]
					,'''+ CAST(ISNULL(@Buttons,'') AS NVARCHAR(MAX))+ ''','''+ CAST(ISNULL(@VonageTemplateId,'') AS NVARCHAR(MAX))+ ''','''+ CAST(ISNULL(@VonageTemplateStatus,'') AS NVARCHAR(MAX))+ ''',[TTR].[CustomerUserId],[C].[WhatsappNumber],[VC].[APPId],[VC].[AppPrivatKey],'''+ CAST(ISNULL(@TemplateName,'') AS NVARCHAR(MAX))+ ''',[C].[BusinessName],CONCAT(C.City,'', '',C.Country,'', '',C.Postalcode),TTR.BookingDays,C.PhoneNumber,CG.Firstname
					FROM #TempTablesReservation TTR (NOLOCK)
							INNER JOIN [dbo].[CustomerGuests] CG (NOLOCK)
								ON [CG].[CustomerReservationId] = [TTR].[Id]
								INNER JOIN [dbo].[Customers] C (NOLOCK)
								ON [C].[Id] = [TTR].[CustomerId]
								INNER JOIN [dbo].[VonageCredentials] VC (NOLOCK)
								ON [VC].[CustomerId] = [TTR].[CustomerId]
									AND [CG].[DeletedAt] IS NULL
						WHERE [TTR].[DeletedAt] IS NULL
							AND [TTR].[CustomerId] = ''' + CAST(@CustomerId AS NVARCHAR(10)) + ''' 
					'
					print  @query
            IF (@TimingOption3 = 1)
            BEGIN
                SET @query += ' AND CAST([TTR].[CreatedAt] AS DATE) = CAST(''' + CAST(@ModifyDate AS NVARCHAR(50)) + ''' AS DATE)'
                SET @query += ' AND CONVERT(VARCHAR(5), [TTR].[CreatedAt], 108) BETWEEN CONVERT(VARCHAR(5), '''+ CAST(CAST(@diffMinuteDate AS TIME) AS NVARCHAR(50))+ ''', 108) 
								AND CONVERT(VARCHAR(5), ''' + CAST(CAST(@ModifyDate AS TIME) AS NVARCHAR(50))  + ''', 108)
							  '
            END
            ELSE IF (@TimingOption3 = 2)
            BEGIN
                SET @query += ' AND CAST([TTR].[CheckinDate] AS DATE) = CAST(''' + CAST(@ModifyDate AS NVARCHAR(50)) + ''' AS DATE)'
                SET @query += ' AND CONVERT(VARCHAR(5), [TTR].[CheckinDate], 108) BETWEEN CONVERT(VARCHAR(5), ''' + CAST(CAST(@diffMinuteDate AS TIME) AS NVARCHAR(50)) + ''', 108) 
								AND CONVERT(VARCHAR(5), ''' + CAST(CAST(@ModifyDate AS TIME) AS NVARCHAR(50)) + ''', 108)
								'

            END
            ELSE IF (@TimingOption3 = 3)
            BEGIN
                SET @query += ' AND CAST([TTR].[CheckoutDate] AS DATE) = CAST(''' + CAST(@ModifyDate AS NVARCHAR(50)) + ''' AS DATE)'
                SET @query += ' AND CONVERT(VARCHAR(5), [TTR].[CheckoutDate], 108) BETWEEN CONVERT(VARCHAR(5), ''' + CAST(CAST(@diffMinuteDate AS TIME) AS NVARCHAR(50)) + ''', 108) 
								AND CONVERT(VARCHAR(5), ''' + CAST(CAST(@ModifyDate AS TIME) AS NVARCHAR(50)) + ''', 108)
								'
            END
        END
        ELSE
        BEGIN
            PRINT 'WITH NOTIFICATION DAYS AND TIME'
            DECLARE @WeekNumber INT = 0
            SET @WeekNumber = DATEPART(WEEKDAY, @ModifyDate)

            IF (CHARINDEX(CAST(@WeekNumber AS VARCHAR(10)), @NotificationDays) > 0)
            BEGIN
                SET @query
                    = ' INSERT INTO #TempTables ([Id],[CustomerId],[JourneyStep],[Uuid],[ReservationNumber],[Source],[NoOfGuestAdults],[NoOfGuestChildrens],[CheckinDate],[CheckoutDate],[IsActive],[CreatedAt],[isCheckInCompleted],[isSkipCheckIn],[Email],[Phone],[TempletMessage],[GuestId],[Buttons],[VonageTemplateId],[VonageTemplateStatus],[CustomerUserId],[CustomerWhatsAppNumber],[APPId],[PrivateKey],[TemplateName],[BussinessName],[BussinessAddress],[BookingDays],BussinessPhoneNumber,GuestName)
						SELECT DISTINCT ''' + CAST(@CustomerId AS NVARCHAR(10)) + ''',[TTR].[Id],''' + CAST(@JourneyStep AS NVARCHAR(30))
                      + ''',[TTR].[Uuid],[TTR].[ReservationNumber],[TTR].[Source],[TTR].[NoOfGuestAdults],[TTR].[NoOfGuestChildrens],[TTR].[CheckinDate],[TTR].[CheckoutDate],[TTR].[IsActive],[TTR].[CreatedAt],[TTR].[isCheckInCompleted],[TTR].[isSkipCheckIn],[CG].[Email],[CG].[PhoneNumber],'''
                      + CAST(@TempletMessage AS NVARCHAR(MAX))
                      + ''',[CG].[Id],'''+ CAST(ISNULL(@Buttons,'') AS NVARCHAR(MAX))+ ''','''+ CAST(ISNULL(@VonageTemplateId,'') AS NVARCHAR(MAX))+ ''','''+ CAST(ISNULL(@VonageTemplateStatus,'') AS NVARCHAR(MAX))+ ''',[TTR].[CustomerUserId],[C].[WhatsappNumber],[VC].[APPId],[VC].[AppPrivatKey],'''+ CAST(ISNULL(@TemplateName,'') AS NVARCHAR(MAX))+ ''',[c].[BusinessName],CONCAT(C.City,'', '',C.Country,'', '',C.Postalcode),TTR.BookingDays,C.PhoneNumber,CG.Firstname
							  FROM #TempTablesReservation TTR (NOLOCK)
								INNER JOIN [dbo].[CustomerGuests] CG (NOLOCK)
									ON [CG].[CustomerReservationId] = [TTR].[Id]
									INNER JOIN [dbo].[Customers] C (NOLOCK)
									ON [C].[Id] = [TTR].[CustomerId]
									INNER JOIN [dbo].[VonageCredentials] VC (NOLOCK)
								    ON [VC].[CustomerId] = [TTR].[CustomerId]
										AND [CG].[DeletedAt] IS NULL
								WHERE [TTR].[DeletedAt] IS NULL
								AND [TTR].[CustomerId] = ''' + CAST(@CustomerId AS NVARCHAR(10)) + ''' 
							'
                IF (@TimingOption3 = 1)
                BEGIN
                    SET @query += ' AND CAST([TTR].[CreatedAt] AS DATE) = CAST(''' + CAST(@ModifyDate AS NVARCHAR(50)) + ''' AS DATE)'
                    SET @query += ' AND CONVERT(VARCHAR(5), [TTR].[CreatedAt], 108) = CONVERT(VARCHAR(5), ''' + CAST(CAST(@ModifyDate AS TIME) AS NVARCHAR(50)) + ''', 108)'
					SET @query += ' AND CONVERT(VARCHAR(5), [TTR].[CreatedAt], 108) BETWEEN CONVERT(VARCHAR(5), '''+ CAST(CAST(@diffMinuteDate AS TIME) AS NVARCHAR(50))+ ''', 108) AND CONVERT(VARCHAR(5), '''+ CAST(CAST(@ModifyDate AS TIME) AS NVARCHAR(50)) + ''', 108)'
					SET @query += ' AND CONVERT(VARCHAR(5), ''' + CAST(@NotificationTime AS VARCHAR(100)) + ''', 108) = CONVERT(VARCHAR(5), '''+ CAST(CAST(@ModifyDate AS TIME) AS NVARCHAR(50)) + ''', 108 )'
                END
                ELSE IF (@TimingOption3 = 2)
                BEGIN
                    SET @query += ' AND CAST([TTR].[CheckinDate] AS DATE) = CAST(''' + CAST(@ModifyDate AS NVARCHAR(50))+ ''' AS DATE)'
                    SET @query += ' AND CONVERT(VARCHAR(5), [TTR].[CheckinDate], 108) = CONVERT(VARCHAR(5), ''' + CAST(CAST(@ModifyDate AS TIME) AS NVARCHAR(50)) + ''', 108)'
					SET @query += ' AND CONVERT(VARCHAR(5), [TTR].[CheckinDate], 108) BETWEEN CONVERT(VARCHAR(5), '''+ CAST(CAST(@diffMinuteDate AS TIME) AS NVARCHAR(50))+ ''', 108) AND CONVERT(VARCHAR(5), '''+ CAST(CAST(@ModifyDate AS TIME) AS NVARCHAR(50)) + ''', 108)'
					SET @query += ' AND CONVERT(VARCHAR(5), ''' + CAST(@NotificationTime AS VARCHAR(100)) + ''', 108) = CONVERT(VARCHAR(5), '''+ CAST(CAST(@ModifyDate AS TIME) AS NVARCHAR(50)) + ''', 108 )'
                END
                ELSE IF (@TimingOption3 = 3)
                BEGIN
                    SET @query += ' AND CAST([TTR].[CheckoutDate] AS DATE) = CAST('''+ CAST(@ModifyDate AS NVARCHAR(50)) + ''' AS DATE)'
                    SET @query += ' AND CONVERT(VARCHAR(5), [TTR].[CheckoutDate], 108) = CONVERT(VARCHAR(5), '''+ CAST(CAST(@ModifyDate AS TIME) AS NVARCHAR(50)) + ''', 108)'
					SET @query += ' AND CONVERT(VARCHAR(5), [TTR].[CheckoutDate], 108) BETWEEN CONVERT(VARCHAR(5), '''+ CAST(CAST(@diffMinuteDate AS TIME) AS NVARCHAR(50)) + ''', 108) AND CONVERT(VARCHAR(5), '''+ CAST(CAST(@ModifyDate AS TIME) AS NVARCHAR(50)) + ''', 108)'
					SET @query += ' AND CONVERT(VARCHAR(5), ''' + CAST(@NotificationTime AS VARCHAR(100))+ ''', 108) = CONVERT(VARCHAR(5), '''+ CAST(CAST(@ModifyDate AS TIME) AS NVARCHAR(50)) + ''', 108 )'
                END
            END
        END

        SET @query += @orderBy

        EXEC sp_executesql @query

        FETCH NEXT FROM CustomerJourneyCursor
        INTO @Id,
             @CustomerId,
             @JourneyStep,
			 @TemplateName,
             @TimingOption1,
             @TimingOption2,
             @TimingOption3,
             @Timing,
             @NotificationDays,
             @NotificationTime,
             @TempletMessage,
			 @Buttons,
			 @VonageTemplateId,
			 @VonageTemplateStatus
    END

    CLOSE CustomerJourneyCursor
    DEALLOCATE CustomerJourneyCursor

    SELECT DISTINCT
        [Id],
        [CustomerId],
		[BussinessName],
		[BussinessAddress],
		[BussinessPhoneNumber],
        [JourneyStep],
        [Uuid],
        [ReservationNumber],
        [Source],
        [NoOfGuestAdults],
        [NoOfGuestChildrens],
        [CheckinDate],
        [CheckoutDate],
		[BookingDays],
        [IsActive],
        [CreatedAt],
        [isCheckInCompleted],
        [isSkipCheckIn],
        [Email],
        [Phone],
        [TempletMessage],
		[GuestId],
		[Buttons],
		[VonageTemplateId],
		[VonageTemplateStatus],
		[CustomerUserId],
		[CustomerWhatsAppNumber],
		[APPId],
		[PrivateKey],
		[TemplateName],
		[GuestName]
    FROM #TempTables
    ORDER BY ID

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
