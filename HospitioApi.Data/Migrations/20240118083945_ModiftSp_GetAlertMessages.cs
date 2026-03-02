using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModiftSp_GetAlertMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
/****** Object:  StoredProcedure [dbo].[GetAlertMessages]    Script Date: 18-01-2024 12:44:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetAlertMessages] -- '2024-01-18 08:38:51.703'
(
    @SendDateTime DATETIME = NULL
)
AS
BEGIN 
	 SET NOCOUNT ON
     SET XACT_ABORT ON
   DECLARE 
    @AlertMessage NVARCHAR(MAX), 
    @Minute INT,
    @ChannelId INT,
    @MessageSenderType INT,
    @MessageSenderId INT,
	@SenderType int,
	@SenderId int,
    @CreatedAt DATETIME,
    @Message NVARCHAR(MAX),
    @diffinminute INT,
    @UserId INT;

IF OBJECT_ID('tempdb..#TempTables') IS NOT NULL
    DROP TABLE #TempTables;

CREATE TABLE #TempTables
(
    [AlertMessage] NVARCHAR(MAX),
    [ChatId] INT,
    [Minute] INT,
	[LastMessageSendingTime] DAtetime,
	[LastMessage] NVARCHAR(MAX),
    [AlertType] NVARCHAR(MAX),
	[SenderId] int,
	[SenderType] int,
	[ReceiverId] Int,
	[ReceiverType] Int,
	[Platform] int,
	[NameOfStaffPerson] Nvarchar(MAX),
	[FromPhoneNumber]  nvarchar(max),
	[ToPhoneCountry] nvarchar(max),
	[ToPhoneNumber] nvarchar(max),
	[VonageAppId] nvarchar(max),
	[VonagePrivateKey] nvarchar(max),
	[VonageTemplateId] nvarchar(max),
	[VonageTemplateStatus] nvarchar(max),
	[WhatsappTemplateName] nvarchar(max)
);

DECLARE AdminCustomerAlert CURSOR FOR
select Msg,MsgWaitTimeInMinutes,CreatedBy from AdminCustomerAlerts (NOLOCK)
where DeletedAt is null

OPEN AdminCustomerAlert;
FETCH NEXT FROM AdminCustomerAlert
INTO @AlertMessage, @Minute, @UserId;

SET @SenderId = @UserId;
set @SenderType = 1

WHILE @@FETCH_STATUS = 0
BEGIN

DECLARE AdminCustomerAlertCursor CURSOR FOR
WITH customer_admin_cte AS (
	SELECT cm.*,
       ROW_NUMBER() OVER (PARTITION BY cm.ChannelId ORDER BY cm.CreatedAt DESC) AS RowNum
	FROM ChannelMessages cm
	JOIN ChannelUsers cu1 ON cm.ChannelId = cu1.ChannelId
	JOIN ChannelUsers cu2 ON cu1.ChannelId = cu2.ChannelId
	WHERE cu2.UserType = 'HospitioUser' AND cu2.UserId = @UserId
	AND cu1.UserType = 'CustomerUser'
)
SELECT 
    ChannelId,
    MessageSender,
    MessageSenderId,
    CreatedAt,
    Message
FROM customer_admin_cte
WHERE RowNum = 1
AND MessageType = 'Text'
ORDER BY ChannelId, CreatedAt DESC;

OPEN AdminCustomerAlertCursor;

FETCH NEXT FROM AdminCustomerAlertCursor
INTO @ChannelId, @MessageSenderType, @MessageSenderId, @CreatedAt, @Message;

WHILE @@FETCH_STATUS = 0
BEGIN

    IF (@MessageSenderType = 2) 
    BEGIN
        SET @diffinminute = DATEDIFF(MINUTE, @CreatedAt, @SendDateTime);

        IF (@Minute = @diffinminute)
        BEGIN
            INSERT INTO #TempTables ([AlertMessage], [ChatId], [Minute], [LastMessageSendingTime],[LastMessage],[AlertType],[ReceiverId],[ReceiverType],[Platform],[NameOfStaffPerson],[SenderId],[SenderType])
            VALUES (@AlertMessage, @ChannelId, @Minute,@CreatedAt ,@Message,'AdminCustomerAlert',@MessageSenderId,@MessageSenderType,null,null,@SenderId,@SenderType);
        END
    END

    FETCH NEXT FROM AdminCustomerAlertCursor
    INTO @ChannelId, @MessageSenderType, @MessageSenderId, @CreatedAt, @Message;
END

CLOSE AdminCustomerAlertCursor;
DEALLOCATE AdminCustomerAlertCursor;
FETCH NEXT FROM AdminCustomerAlert
INTO @AlertMessage, @Minute, @UserId;

END

CLOSE AdminCustomerAlert;
DEALLOCATE AdminCustomerAlert;

-----------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------

DECLARE 
    @staffName NVARCHAR(MAX),
    @Platfrom INT,
    @phoneCountry NVARCHAR(MAX),
    @PhoneNumber NVARCHAR(MAX),
    @AdminStaffId INT,
    @AdminsId INT,
	@MessageSenderType2 INT,
	@MessageSenderId2 INT ,
	@diffinminute2 int,
	@CreatedAt2 DateTime,
	@Minute2 int,
	@AlertMessage2 nvarchar(max),
	@ChannelId2 int,
	@Message2 nvarchar(max),
	@AdminPhoneNumber nvarchar(max),
	@counterForAdminstaffAlert INT,
	@vonageTemplateId nvarchar(max),
	@vonageTemplateStatus nvarchar(max),
	@whatsappTemplateName nvarchar(max)

	SET @counterForAdminstaffAlert = 0;
	
DECLARE AdminStaffAlertCursor CURSOR FOR
SELECT 
    Name,
    Platfrom,
    PhoneCountry,
    PhoneNumber,
    WaitTimeInMintes,
    Msg,
    UserId,
    CreatedBy,
	VonageTemplateId,
	VonageTemplateStatus,
	WhatsappTemplateName
FROM AdminStaffAlerts
WHERE 
DeletedAt IS NULL;
OPEN AdminStaffAlertCursor;

FETCH NEXT FROM AdminStaffAlertCursor
INTO @staffName, @Platfrom, @phoneCountry, @PhoneNumber, @Minute2, @AlertMessage2, @AdminStaffId, @AdminsId,@vonageTemplateId,@vonageTemplateStatus,@whatsappTemplateName;

WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE AdminStaffAlert CURSOR FOR
    WITH customer_admin_cte AS (
        SELECT cm.*,
       ROW_NUMBER() OVER (PARTITION BY cm.ChannelId ORDER BY cm.CreatedAt DESC) AS RowNum
FROM ChannelMessages cm
JOIN ChannelUsers cu1 ON cm.ChannelId = cu1.ChannelId
JOIN ChannelUsers cu2 ON cu1.ChannelId = cu2.ChannelId
WHERE cu2.UserId = @AdminStaffId AND cu2.UserType = 'HospitioUser'
  AND cu1.UserType = 'CustomerUser'

    )
    SELECT 
        ChannelId,
        MessageSender,
        MessageSenderId,
        CreatedAt,
        Message,
		(select TOP(1) WhatsappNumber from HospitioOnboardings) AS FromPhoneNumber
    FROM customer_admin_cte
    WHERE RowNum = 1
    AND MessageType = 'Text'
    ORDER BY ChannelId, CreatedAt DESC;

	OPEN AdminStaffAlert;

    FETCH NEXT FROM AdminStaffAlert
    INTO @ChannelId2, @MessageSenderType2, @MessageSenderId2, @CreatedAt2, @Message2,@AdminPhoneNumber;

    WHILE @@FETCH_STATUS = 0
    BEGIN
		IF (@MessageSenderType2 = 2) 
        BEGIN
				SET @diffinminute2 = DATEDIFF(MINUTE, @CreatedAt2, @SendDateTime);

					IF (@Minute2 = @diffinminute2)
						BEGIN
							SET @counterForAdminstaffAlert = @counterForAdminstaffAlert + 1;
							--INSERT INTO #TempTables ([AlertMessage], [ChatId], [Minute],[LastMessageSendingTime],[LastMessage],[AlertType],[ReceiverId],[ReceiverType],[Platform],[NameOfStaffPerson],[SenderId],[SenderType],[FromPhoneNumber],[ToPhoneNumber],[ToPhoneCountry])
							--VALUES (@AlertMessage2, @ChannelId2, @Minute2,@CreatedAt2,@Message2,'AdminStaffAlert',@MessageSenderId2,@MessageSenderType2,@Platfrom,@staffName,@AdminsId,1,@AdminPhoneNumber,@PhoneNumber,@phoneCountry);
						END
         END
         FETCH NEXT FROM AdminStaffAlert
        INTO @ChannelId2, @MessageSenderType2, @MessageSenderId2, @CreatedAt2, @Message2,@AdminPhoneNumber;
     END
 
    CLOSE AdminStaffAlert;
    DEALLOCATE AdminStaffAlert;

	IF (@counterForAdminstaffAlert <> 0) 
	BEGIN
		INSERT INTO #TempTables ([AlertMessage], [ChatId], [Minute],[LastMessageSendingTime],[LastMessage],[AlertType],[ReceiverId],[ReceiverType],[Platform],[NameOfStaffPerson],[SenderId],[SenderType],[FromPhoneNumber],[ToPhoneNumber],[ToPhoneCountry],[VonageTemplateId],[VonageTemplateStatus],[WhatsappTemplateName])
		VALUES (@AlertMessage2, 0, @Minute2,@CreatedAt2,@Message2,'AdminStaffAlert',@AdminStaffId,1,@Platfrom,@staffName,@AdminsId,1,@AdminPhoneNumber,@PhoneNumber,@phoneCountry,@vonageTemplateId,@vonageTemplateStatus,@whatsappTemplateName);
	END

    FETCH NEXT FROM AdminStaffAlertCursor
    INTO @staffName, @Platfrom, @phoneCountry, @PhoneNumber, @Minute2, @AlertMessage2, @AdminStaffId, @AdminsId,@vonageTemplateId,@vonageTemplateStatus,@whatsappTemplateName;
END
CLOSE AdminStaffAlertCursor;
DEALLOCATE AdminStaffAlertCursor;

-----------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------
DECLARE @customerId int,
@OfficeHoursMsg varchar(max),
@OfficeHoursMsgWaitTimeInMinutes int,
@OfflineHourMsg varchar(max),
@OfflineHoursMsgWaitTimeInMinutes int,
@ReplyAtDiffPeriod bit,
@CustomerUserId int,
@ChannelId3 int,
@ReceiverId int,
@ReceiverType int,
@SenderId2 int,
@SenderType2 int,
@LastMessage varchar(max),
@LastMessageSendingTime Datetime,
@Source int,
@diffinminute3 int,
@bussinessStartTime Time,
@bussinessEndTime Time,
@CurrentTimeOnly Time; 



DECLARE CustomerGuestAlert CURSOR FOR
select CustomerId,
OfficeHoursMsg,
OfficeHoursMsgWaitTimeInMinutes,
OfflineHourMsg,
OfflineHoursMsgWaitTimeInMinutes,
ReplyAtDiffPeriod,
CreatedBy
from CustomerGuestAlerts where DeletedAt is null

OPEN CustomerGuestAlert

FETCH NEXT FROM CustomerGuestAlert
INTO @customerId,@OfficeHoursMsg,@OfficeHoursMsgWaitTimeInMinutes,@OfflineHourMsg,@OfflineHoursMsgWaitTimeInMinutes,@ReplyAtDiffPeriod,@CustomerUserId

WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE CustomerGuestAlertCursor CURSOR FOR
    WITH customer_guest_channelMessages AS (
        SELECT cm.*,
       ROW_NUMBER() OVER (PARTITION BY cm.ChannelId ORDER BY cm.CreatedAt DESC) AS RowNum
		FROM ChannelMessages cm
		JOIN ChannelUsers cu1 ON cm.ChannelId = cu1.ChannelId
		JOIN ChannelUsers cu2 ON cu1.ChannelId = cu2.ChannelId
		WHERE cu2.UserId = @CustomerUserId AND cu2.UserType = 'CustomerUser'
		  AND cu1.UserType = 'CustomerGuest'
    )
    SELECT  
        ChannelId,
        MessageSender,
        MessageSenderId,
        Source,
        CreatedAt,
        Message
    FROM customer_guest_channelMessages
    WHERE RowNum = 1
    AND MessageType = 'Text'
    ORDER BY ChannelId, CreatedAt DESC;

    OPEN CustomerGuestAlertCursor
    FETCH NEXT FROM CustomerGuestAlertCursor
    INTO @ChannelId, @ReceiverType, @ReceiverId, @Source, @LastMessageSendingTime, @LastMessage;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @SenderId = @CustomerUserId;
        SET @SenderType = 2;
        SELECT @bussinessStartTime = BusinessStartTime,
               @bussinessEndTime = BusinessCloseTime
        FROM Customers
        WHERE Id = @customerId;
        SET @CurrentTimeOnly = CAST(@SendDateTime AS TIME);
		
        IF (@ReceiverType = 3)
        BEGIN
            SET @diffinminute = DATEDIFF(MINUTE, @LastMessageSendingTime, @SendDateTime);
            
            IF (@CurrentTimeOnly BETWEEN @bussinessStartTime AND @bussinessEndTime)
            BEGIN
                IF (@OfficeHoursMsgWaitTimeInMinutes = @diffinminute)
                BEGIN
                    INSERT INTO #TempTables ([AlertMessage], [ChatId], [Minute], [LastMessageSendingTime], [LastMessage], [AlertType], [ReceiverId], [ReceiverType], [Platform], [NameOfStaffPerson], [SenderId], [SenderType])
                    VALUES (@OfficeHoursMsg, @ChannelId, @OfficeHoursMsgWaitTimeInMinutes, @LastMessageSendingTime, @LastMessage, 'CustomerGuestAlert', @ReceiverId, @ReceiverType, NULL, NULL, @SenderId, @SenderType);
                END
            END
            ELSE
            BEGIN
                IF (@ReplyAtDiffPeriod = 1)
                BEGIN
                    IF (@OfflineHoursMsgWaitTimeInMinutes <= @diffinminute)
                    BEGIN
                        INSERT INTO #TempTables ([AlertMessage], [ChatId], [Minute], [LastMessageSendingTime], [LastMessage], [AlertType], [ReceiverId], [ReceiverType], [Platform], [NameOfStaffPerson], [SenderId], [SenderType])
                        VALUES (@OfflineHourMsg, @ChannelId, @OfflineHoursMsgWaitTimeInMinutes, @LastMessageSendingTime, @LastMessage, 'CustomerGuestAlert', @ReceiverId, @ReceiverType, NULL, NULL, @SenderId, @SenderType);
                    END
                END
            END
        END

        FETCH NEXT FROM CustomerGuestAlertCursor
        INTO @ChannelId, @ReceiverType, @ReceiverId, @Source, @LastMessageSendingTime, @LastMessage;
    END
    
    CLOSE CustomerGuestAlertCursor;
    DEALLOCATE CustomerGuestAlertCursor;
    
    FETCH NEXT FROM CustomerGuestAlert
    INTO @customerId, @OfficeHoursMsg, @OfficeHoursMsgWaitTimeInMinutes, @OfflineHourMsg, @OfflineHoursMsgWaitTimeInMinutes, @ReplyAtDiffPeriod, @CustomerUserId;
END


CLOSE CustomerGuestAlert;
DEALLOCATE CustomerGuestAlert;
-----------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------

DECLARE @CustomerUsersId int,
        @CustomerUserStaffId int,
		@MessageSenderType4 int,
		@MessageSenderId4 int,
		@Platform int,
		@PhoneNumber2 varchar(max),
		@PhoneCountry2 varchar(max),
		@Minutes int,
		@Msg varchar(max),
		@ChannelId4 int,
		@Message4 varchar(max),
		@CreatedAt4 DateTime,
		@diffinminute4 int,
		@NameOfCustomerStaff varchar(max),
		@CustomerPhoneNumber varchar(max),
		@customerAppId varchar(max),
		@customerAppPrivateKey varchar(max),
		@CustomerId2 varchar(max);
	

  DECLARE CustomerStaffAlert CURSOR FOR
  select CustomerId,Name,Platfrom,PhoneCountry,PhoneNumber,WaitTimeInMintes,CreatedBy,Msg,CustomerUserId from CustomerStaffAlerts where DeletedAt is null

  OPEN CustomerStaffAlert
  FETCH NEXT FROM CustomerStaffAlert
  INTO @CustomerId2,@NameOfCustomerStaff,@Platform,@PhoneCountry,@PhoneNumber,@Minutes,@CustomerUsersId,@Msg,@CustomerUserStaffId

  WHILE @@FETCH_STATUS = 0
  BEGIN
  
  DECLARE CustomerStaffAlertCursor CURSOR FOR
  With customer_staff_cte as (
SELECT cm.*,
       ROW_NUMBER() OVER (PARTITION BY cm.ChannelId ORDER BY cm.CreatedAt DESC) AS RowNum
FROM ChannelMessages cm
JOIN ChannelUsers cu1 ON cm.ChannelId = cu1.ChannelId
JOIN ChannelUsers cu2 ON cu1.ChannelId = cu2.ChannelId
WHERE cu2.UserId = @CustomerUsersId AND cu2.UserType = 'CustomerUser'
  AND cu1.UserType = 'CustomerUser' AND cu1.UserId = @CustomerUserStaffId
 
	 )
	 SELECT 
        ChannelId,
        MessageSender,
        MessageSenderId,
        CreatedAt,
        Message,
		(select TOP (1) WhatsappNumber from  Customers  where Id =  @CustomerId2) AS FromNumber,
		(select AppId from VonageCredentials where	CustomerId = @CustomerId2 and DeletedAt is null) AS AppId,
		(select AppPrivatKey from VonageCredentials where	CustomerId = @CustomerId2 and DeletedAt is null) AS AppPrivatKey
    FROM customer_staff_cte
    WHERE RowNum = 1
    AND MessageType = 'Text'
    ORDER BY ChannelId, CreatedAt DESC;

	OPEN CustomerStaffAlertCursor;
	 FETCH NEXT FROM CustomerStaffAlertCursor
    INTO @ChannelId4, @MessageSenderType4, @MessageSenderId4, @CreatedAt4, @Message4,@CustomerPhoneNumber,@customerAppId,@customerAppPrivateKey;
	SET @SenderId = @CustomerUsersId;
	WHILE @@FETCH_STATUS = 0
    BEGIN
	IF (@MessageSenderType4 = 2)
	BEGIN
		IF (@MessageSenderId4 = @CustomerUserStaffId) 
		BEGIN
		SET @diffinminute2 = DATEDIFF(MINUTE, @CreatedAt4, @SendDateTime);
		IF (@Minutes = @diffinminute2)
						BEGIN
							INSERT INTO #TempTables ([AlertMessage], [ChatId], [Minute],[LastMessageSendingTime],[LastMessage],[AlertType],[ReceiverId],[ReceiverType],[Platform],[NameOfStaffPerson],[SenderId],[SenderType],[FromPhoneNumber],[ToPhoneNumber],[ToPhoneCountry],[VonageAppId],[VonagePrivateKey])
							VALUES (@Msg, @ChannelId4, @Minutes,@CreatedAt4,@Message4,'CustomerStaffAlert',@MessageSenderId4,@MessageSenderType4,@Platform,@NameOfCustomerStaff,@SenderId,2,@CustomerPhoneNumber,@PhoneNumber,@phoneCountry,@customerAppId,@customerAppPrivateKey);
						END
		END
	END
	FETCH NEXT FROM CustomerStaffAlertCursor
    INTO @ChannelId4, @MessageSenderType4, @MessageSenderId4, @CreatedAt4, @Message4,@CustomerPhoneNumber,@customerAppId,@customerAppPrivateKey;
	
	END
	 CLOSE CustomerStaffAlertCursor;
     DEALLOCATE CustomerStaffAlertCursor;

     FETCH NEXT FROM CustomerStaffAlert
     INTO @CustomerId2,@NameOfCustomerStaff,@Platform,@PhoneCountry,@PhoneNumber,@Minutes,@CustomerUsersId,@Msg,@CustomerUserStaffId
  END
  CLOSE CustomerStaffAlert;
  DEALLOCATE CustomerStaffAlert;

  SELECT * FROM #TempTables;

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
