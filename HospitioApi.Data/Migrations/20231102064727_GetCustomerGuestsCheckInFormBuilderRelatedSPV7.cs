using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class GetCustomerGuestsCheckInFormBuilderRelatedSPV7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
GO
/****** Object:  StoredProcedure [dbo].[GetCustomerReservationWithCustomerPropInfo]    Script Date: 02-11-2023 12:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER           PROCEDURE [dbo].[GetCustomerReservationWithCustomerPropInfo] -- 1,1
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
				   [g].[PhoneNumber],
				   (
                       SELECT [cu].[CurrencyCode]
                       FROM [dbo].[CustomerReservations] r
                       INNER JOIN [dbo].[Customers] cu ON r.[CustomerId] = cu.[Id]
                       WHERE r.[Id] = @ReservationId
                   ) AS [CurrencyCode],
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
                              [g].[isCheckInCompleted],
                              [g].[isSkipCheckIn],
							  [g].[AppAccessCode],
							  [g].[PhoneNumber],
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
										 [GuestWelcomeMessage],
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
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
