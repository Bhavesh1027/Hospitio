using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitioApi.Data.Migrations
{
    public partial class Add_GuestRequests_SP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region GetGuestRequestById SP
            migrationBuilder.Sql(@"
                                    Create or ALTER PROCEDURE [dbo].[GetGuestRequests]
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
										select guestrequest.Id,customerguest.Firstname,customerguest.Lastname,guestrequest.Status as TaskStatus,customerroomname.Name as Room, guestrequest.RequestType, enhanceyourstay.ShortDescription as EnhanceYourStayItem,enhanceyourstay.Price as EnhanceYourStayItemPrice, housekeeping.Name as HouseKeepingItem,housekeeping.Price as HouseKeepingItemPrice, concierge.Name as ConciergeItem,concierge.Price as ConciergeItemPrice, reception.Name as ReceptionItem,reception.Price as ReceptionItemPrice, roomservice.Name as RoomServiceItem,roomservice.Price as RoomServiceItemPrice, customerguest.Rating, guestrequest.MonthValue, guestrequest.DayValue, guestrequest.YearValue, guestrequest.HourValue, guestrequest.MinuteValue, guestrequest.CreatedAt as TImeStamp, guestrequest.UpdateAt
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
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
