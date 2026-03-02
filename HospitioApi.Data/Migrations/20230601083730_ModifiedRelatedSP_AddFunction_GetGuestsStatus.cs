using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class ModifiedRelatedSP_AddFunction_GetGuestsStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add Function Get Guests Status
            migrationBuilder.Sql(@"
                                CREATE or ALTER FUNCTION GetGuestsStatus(@CustomerGuestId int)  
                                RETURNS int   --returns int type value of GuestsStatus ""1. Expected 2. InHouse 3. Checkout""
                                    AS 
                                    BEGIN
                                        
                                        DECLARE @GuestStatus int = 0;
                                    
                                        -- retrieves guests status based on reservation checkin and checkout date 
                                        select 
                                		@GuestStatus =  
                                		CASE
                                        WHEN dbo.CustomerReservations.CheckinDate > CAST(CURRENT_TIMESTAMP AS DATE) THEN 1
                                        WHEN CAST(CURRENT_TIMESTAMP AS DATE) between dbo.CustomerReservations.CheckinDate and dbo.CustomerReservations.CheckoutDate THEN 2
                                		WHEN dbo.CustomerReservations.CheckoutDate < CAST(CURRENT_TIMESTAMP AS DATE) THEN 3
                                		End
                                		from dbo.CustomerGuests
                                		inner join dbo.CustomerReservations on dbo.CustomerReservations.Id = dbo.CustomerGuests.CustomerReservationId 
                                		where dbo.CustomerGuests.Id = @CustomerGuestId
                                    
                                        RETURN @GuestStatus; --returns a value
                                    END
                                ");

            // Modified Get Customer Guests
            migrationBuilder.Sql(@"
                                 CREATE or ALTER PROCEDURE [dbo].[GetCustomerGuests] 
                                 (
                                     @CustomerId int = 0,
                                     @SearchValue NVARCHAR(50) = '',
                                     @PageNo INT = 1,
                                     @PageSize INT = 10,
                                     @SortColumn NVARCHAR(20) = 'Firstname',
                                     @SortOrder NVARCHAR(5) = 'ASC'
                                 )
                                 AS
                                 BEGIN
                                                                     
                                     SET NOCOUNT ON;
                                       
                                     SET @SearchValue = LTRIM(RTRIM(@SearchValue))
                                                                     
                                     ; WITH CustomerGuests_Results AS
                                     (
                                         SELECT dbo.CustomerGuests.[Id],dbo.CustomerGuests.[CustomerReservationId],dbo.CustomerGuests.[Firstname]
                                 		,dbo.CustomerGuests.[Lastname],dbo.CustomerGuests.[Email],dbo.CustomerGuests.[Picture],dbo.CustomerGuests.[PhoneCountry]
                                 		,dbo.CustomerGuests.[PhoneNumber],dbo.CustomerGuests.[Country],dbo.CustomerGuests.[Language],dbo.CustomerGuests.[IdProof]
                                 		,dbo.CustomerGuests.[IdProofType],dbo.CustomerGuests.[IdProofNumber],dbo.CustomerGuests.[BlePinCode]
                                 		,dbo.CustomerGuests.[Pin],dbo.CustomerGuests.[Street],dbo.CustomerGuests.[StreetNumber]
                                 		,dbo.CustomerGuests.[City],dbo.CustomerGuests.[Postalcode],dbo.CustomerGuests.[ArrivalFlightNumber]
                                 		,dbo.CustomerGuests.[DepartureAirline],dbo.CustomerGuests.[DepartureFlightNumber],dbo.CustomerGuests.[Signature],dbo.CustomerGuests.[RoomNumber]
                                 		,dbo.CustomerGuests.[TermsAccepted],dbo.CustomerGuests.[FirstJourneyStep],dbo.CustomerGuests.[Rating]
                                 		,dbo.CustomerGuests.[IsActive],dbo.GetGuestsStatus(dbo.CustomerGuests.Id) as GuestStatus
                                     ,COUNT(*) OVER() as FilteredCount
                                                                     
                                     FROM [dbo].[CustomerGuests] WITH (NOLOCK)
                                         Inner Join dbo.CustomerReservations  WITH (NOLOCK)
                                 		ON dbo.CustomerReservations.Id = dbo.CustomerGuests.CustomerReservationId
                                         WHERE dbo.CustomerGuests.DeletedAt is null
                                 		And dbo.CustomerReservations.CustomerId = @CustomerId AND (
                                 			dbo.CustomerGuests.Firstname LIKE '%' + @SearchValue + '%'
                                 			OR dbo.CustomerGuests.Lastname LIKE '%' + @SearchValue + '%'
                                 			OR dbo.CustomerGuests.Email LIKE '%' + @SearchValue + '%'
                                 			OR dbo.CustomerGuests.PhoneNumber LIKE '%' + @SearchValue + '%'
                                 			OR dbo.CustomerGuests.Country LIKE '%' + @SearchValue + '%'
                                 			OR dbo.CustomerGuests.IdProofNumber LIKE '%' + @SearchValue + '%'
                                 			OR dbo.CustomerGuests.[IdProofType] LIKE '%' + @SearchValue + '%'
                                 			OR dbo.CustomerGuests.Street LIKE '%' + @SearchValue + '%'
                                 			OR dbo.CustomerGuests.Postalcode LIKE '%' + @SearchValue + '%'
                                 			OR dbo.CustomerGuests.StreetNumber LIKE '%' + @SearchValue + '%'
                                 			OR dbo.CustomerGuests.ArrivalFlightNumber LIKE '%' + @SearchValue + '%'
                                 			OR dbo.CustomerGuests.DepartureFlightNumber LIKE '%' + @SearchValue + '%'
                                 			OR dbo.CustomerGuests.RoomNumber LIKE '%' + @SearchValue + '%'
                                 			OR dbo.CustomerGuests.Rating LIKE '%' + @SearchValue + '%'             
                                             )
                                                                     
                                     ORDER BY
                                 		CASE WHEN (@SortColumn = 'Firstname' AND @SortOrder='ASC')
                                         THEN Firstname
                                         END ASC,
                                                                            
                                 		CASE WHEN (@SortColumn = 'Firstname' AND @SortOrder='DESC')
                                         THEN Firstname
                                         END DESC,
                                 
                                 		CASE WHEN (@SortColumn = 'Lastname' AND @SortOrder='ASC')
                                         THEN Lastname
                                         END ASC,
                                                                            
                                 		CASE WHEN (@SortColumn = 'Lastname' AND @SortOrder='DESC')
                                         THEN Lastname
                                         END DESC,
                                 
                                 		CASE WHEN (@SortColumn = 'Email' AND @SortOrder='ASC')
                                         THEN Email
                                         END ASC,
                                                                            
                                 		CASE WHEN (@SortColumn = 'Email' AND @SortOrder='DESC')
                                         THEN Email
                                         END DESC,
                                 
                                 		CASE WHEN (@SortColumn = 'PhoneNumber' AND @SortOrder='ASC')
                                         THEN PhoneNumber
                                         END ASC,
                                                                            
                                 		CASE WHEN (@SortColumn = 'PhoneNumber' AND @SortOrder='DESC')
                                         THEN PhoneNumber
                                         END DESC,
                                 
                                 		CASE WHEN (@SortColumn = 'Country' AND @SortOrder='ASC')
                                         THEN Country
                                         END ASC,
                                                                            
                                 		CASE WHEN (@SortColumn = 'Country' AND @SortOrder='DESC')
                                         THEN Country
                                         END DESC,
                                 
                                 		CASE WHEN (@SortColumn = 'IdProofNumber' AND @SortOrder='ASC')
                                         THEN IdProofNumber
                                         END ASC,
                                                                            
                                 		CASE WHEN (@SortColumn = 'IdProofNumber' AND @SortOrder='DESC')
                                         THEN IdProofNumber
                                         END DESC,
                                 
                                 		CASE WHEN (@SortColumn = 'Street' AND @SortOrder='ASC')
                                         THEN Street
                                         END ASC,
                                                                            
                                 		CASE WHEN (@SortColumn = 'Street' AND @SortOrder='DESC')
                                         THEN Street
                                         END DESC,
                                 
                                 		CASE WHEN (@SortColumn = 'Postalcode' AND @SortOrder='ASC')
                                         THEN Postalcode
                                         END ASC,
                                                                            
                                 		CASE WHEN (@SortColumn = 'Postalcode' AND @SortOrder='DESC')
                                         THEN Postalcode
                                         END DESC,
                                 
                                 		CASE WHEN (@SortColumn = 'StreetNumber' AND @SortOrder='ASC')
                                         THEN StreetNumber
                                         END ASC,
                                                                            
                                 		CASE WHEN (@SortColumn = 'StreetNumber' AND @SortOrder='DESC')
                                         THEN StreetNumber
                                         END DESC,
                                 
                                 		CASE WHEN (@SortColumn = 'ArrivalFlightNumber' AND @SortOrder='ASC')
                                         THEN ArrivalFlightNumber
                                         END ASC,
                                                                            
                                 		CASE WHEN (@SortColumn = 'ArrivalFlightNumber' AND @SortOrder='DESC')
                                         THEN ArrivalFlightNumber
                                         END DESC,
                                 
                                 		CASE WHEN (@SortColumn = 'DepartureFlightNumber' AND @SortOrder='ASC')
                                         THEN DepartureFlightNumber
                                         END ASC,
                                                                            
                                 		CASE WHEN (@SortColumn = 'DepartureFlightNumber' AND @SortOrder='DESC')
                                         THEN DepartureFlightNumber
                                         END DESC,
                                 
                                 		CASE WHEN (@SortColumn = 'RoomNumber' AND @SortOrder='ASC')
                                         THEN RoomNumber
                                         END ASC,
                                                                            
                                 		CASE WHEN (@SortColumn = 'RoomNumber' AND @SortOrder='DESC')
                                         THEN RoomNumber
                                         END DESC,
                                 
                                 		CASE WHEN (@SortColumn = 'Rating' AND @SortOrder='ASC')
                                         THEN Rating
                                         END ASC,
                                                                            
                                 		CASE WHEN (@SortColumn = 'Rating' AND @SortOrder='DESC')
                                         THEN Rating
                                         END DESC
                                                                     
                                     OFFSET @PageSize * (@PageNo - 1) ROWS
                                         FETCH NEXT @PageSize ROWS ONLY
                                     )
                                                                     
                                     select [Id],[CustomerReservationId],[Firstname],[Lastname],[Email],[Picture],[PhoneCountry],[PhoneNumber]
                                 	,[Country],[Language],[IdProof],[IdProofType],[IdProofNumber],[BlePinCode],[Pin],[Street],[StreetNumber],[City],[Postalcode]
                                 	,[ArrivalFlightNumber],[DepartureAirline],[DepartureFlightNumber],[Signature],[RoomNumber],[TermsAccepted],[FirstJourneyStep]
                                 	,[Rating],[IsActive],FilteredCount,GuestStatus 
                                     from CustomerGuests_Results
                                     OPTION (RECOMPILE)
                                 END
                                ");

            // Modified Get Guest Requests
            migrationBuilder.Sql(@"
                                    Create or ALTER   PROCEDURE [dbo].[GetGuestRequests]
                                    (
                                        @Id int = 0,
                                    	@CustomerId int = 0,
                                    	@SortColumn NVARCHAR(20) = 'TaskStatus',
                                    	@SortOrder NVARCHAR(5) = 'ASC',
                                    	@PageNo INT = 1,
                                    	@PageSize INT = 10
                                    )
                                    AS
                                    BEGIN
									IF @Id != 0
									BEGIN
										select guestrequest.Id,customerguest.Firstname,customerguest.Lastname,guestrequest.Status as TaskStatus,customerroomname.Name as Room, guestrequest.RequestType, enhanceyourstay.ShortDescription as EnhanceYourStayItem,enhanceyourstay.Price as EnhanceYourStayItemPrice, housekeeping.Name as HouseKeepingItem,housekeeping.Price as HouseKeepingItemPrice, concierge.Name as ConciergeItem,concierge.Price as ConciergeItemPrice, reception.Name as ReceptionItem,reception.Price as ReceptionItemPrice, roomservice.Name as RoomServiceItem,roomservice.Price as RoomServiceItemPrice, customerguest.Rating, guestrequest.MonthValue, guestrequest.DayValue, guestrequest.YearValue, guestrequest.HourValue, guestrequest.MinuteValue, guestrequest.CreatedAt as TImeStamp, guestrequest.UpdateAt
										from GuestRequests guestrequest WITH (NOLOCK)
										inner join CustomerGuests customerguest WITH (NOLOCK) on customerguest.Id = guestrequest.GuestId
										inner join CustomerRoomNames customerroomname WITH (NOLOCK) on customerroomname.CustomerId = guestrequest.CustomerId
										left join CustomerGuestAppEnhanceYourStayItems enhanceyourstay WITH (NOLOCK) on enhanceyourstay.Id = guestrequest.CustomerGuestAppEnhanceYourStayItemId
										left join CustomerGuestAppHousekeepingItems housekeeping WITH (NOLOCK) on housekeeping.Id = guestrequest.CustomerGuestAppHousekeepingItemId
										left join CustomerGuestAppConciergeItems concierge WITH (NOLOCK) on concierge.Id = guestrequest.CustomerGuestAppConciergeItemId
										left join CustomerGuestAppReceptionItems reception WITH (NOLOCK) on reception.Id = guestrequest.CustomerGuestAppReceptionItemId
										left join CustomerGuestAppRoomServiceItems roomservice WITH (NOLOCK) on roomservice.Id = guestrequest.CustomerGuestAppRoomServiceItemId
										where guestrequest.Id = @Id and guestrequest.DeletedAt is null
									END
									IF @CustomerId != 0
									BEGIN
										select guestrequest.Id,customerguest.Firstname,customerguest.Lastname,guestrequest.Status as TaskStatus,customerroomname.Name as Room, guestrequest.RequestType, enhanceyourstay.ShortDescription as EnhanceYourStayItem,enhanceyourstay.Price as EnhanceYourStayItemPrice, housekeeping.Name as HouseKeepingItem,housekeeping.Price as HouseKeepingItemPrice, concierge.Name as ConciergeItem,concierge.Price as ConciergeItemPrice, reception.Name as ReceptionItem,reception.Price as ReceptionItemPrice, roomservice.Name as RoomServiceItem,roomservice.Price as RoomServiceItemPrice, customerguest.Rating, guestrequest.MonthValue, guestrequest.DayValue, guestrequest.YearValue, guestrequest.HourValue, guestrequest.MinuteValue, guestrequest.CreatedAt as TImeStamp, guestrequest.UpdateAt,dbo.GetGuestsStatus(customerguest.Id) as GuestStatus,COUNT(*) OVER() as FilteredCount
										from GuestRequests guestrequest WITH (NOLOCK)
										inner join CustomerGuests customerguest WITH (NOLOCK) on customerguest.Id = guestrequest.GuestId
										inner join CustomerRoomNames customerroomname WITH (NOLOCK) on customerroomname.CustomerId = guestrequest.CustomerId
										left join CustomerGuestAppEnhanceYourStayItems enhanceyourstay WITH (NOLOCK) on enhanceyourstay.Id = guestrequest.CustomerGuestAppEnhanceYourStayItemId
										left join CustomerGuestAppHousekeepingItems housekeeping WITH (NOLOCK) on housekeeping.Id = guestrequest.CustomerGuestAppHousekeepingItemId
										left join CustomerGuestAppConciergeItems concierge WITH (NOLOCK) on concierge.Id = guestrequest.CustomerGuestAppConciergeItemId
										left join CustomerGuestAppReceptionItems reception WITH (NOLOCK) on reception.Id = guestrequest.CustomerGuestAppReceptionItemId
										left join CustomerGuestAppRoomServiceItems roomservice WITH (NOLOCK) on roomservice.Id = guestrequest.CustomerGuestAppRoomServiceItemId
										where guestrequest.CustomerId = @CustomerId and guestrequest.DeletedAt is null

										ORDER BY
										CASE WHEN (@SortColumn = 'TaskStatus' AND @SortOrder='ASC')
										THEN guestrequest.Status
										END ASC,
                                           
										CASE WHEN (@SortColumn = 'TaskStatus' AND @SortOrder='DESC')
										THEN guestrequest.Status
										END DESC,

										CASE WHEN (@SortColumn = 'Room' AND @SortOrder='ASC')
										THEN customerroomname.Name
										END ASC,
                                           
										CASE WHEN (@SortColumn = 'Room' AND @SortOrder='DESC')
										THEN customerroomname.Name
										END DESC,

										CASE WHEN (@SortColumn = 'Department' AND @SortOrder='ASC')
										THEN guestrequest.RequestType
										END ASC,
                                           
										CASE WHEN (@SortColumn = 'Department' AND @SortOrder='DESC')
										THEN guestrequest.RequestType
										END DESC,

										CASE WHEN (@SortColumn = 'TimeStamp' AND @SortOrder='ASC')
										THEN guestrequest.CreatedAt
										END ASC,
                                           
										CASE WHEN (@SortColumn = 'TimeStamp' AND @SortOrder='DESC')
										THEN guestrequest.CreatedAt
										END DESC

										OFFSET @PageSize * (@PageNo - 1) ROWS
										FETCH NEXT @PageSize ROWS ONLY
									END
                                    	
                                    END
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
