using HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.CreateCustomerEnhanceYourStay;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.CreateCustomerEnhanceYourStayItem;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.UpdateCustomerEnhanceYourStayItem;
using HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.CreateCustomerGuestAppEnhanceYourStayCategoryItemExtra;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.CreateCustomerHouseKeeping;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.UpdateCustomerHouseKeeping;
using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.UpdateCustomerPropertyExtras;
using HospitioApi.Core.HandleCustomerPropertyService.Commands.CreateCustomerPropertyService;
using HospitioApi.Core.HandleCustomerReception.Commands.CreateCustomerReception;
using HospitioApi.Core.HandleCustomerReception.Commands.UpdateCustomerReception;
using HospitioApi.Core.HandleCustomerRoomService.Commands.CreateCustomerRoomService;
using HospitioApi.Core.HandleCustomerRoomService.Commands.UpdateCustomerRoomService;
using HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;
using HospitioApi.Core.HandleCustomers.Commands.CreateERPCustomer;
using HospitioApi.Core.HandleCustomers.Commands.UpdateCustomer;
using HospitioApi.Core.HandleCustomers.Commands.UpdateERPServicePack;
using HospitioApi.Core.HandleCustomersConcierge.Commands.CreateCustomerConcierge;
using HospitioApi.Core.HandleCustomersConcierge.Commands.UpdateCustomerConcierge;
using HospitioApi.Core.HandleNotifications.Commands.CreateNotifications;
using HospitioApi.Core.HandleReplicateDataForGuestApp.Commands.ReplicateGuestData;
using HospitioApi.Core.HandleReplicateDateForGuestApp.Commands.ReplicateGuestData;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.Services.Common;

public interface ICommonDataBaseOprationService
{
    Task<Customer> CustomersAdd(CreateCustomerIn request, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<Customer> ERPCustomersAdd(CreateERPCustomerIn request, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<Customer> CustomersUpdate(UpdateCustomerIn request, Customer customer, ApplicationDbContext _db, CancellationToken cancellationToken, ISendEmail _mail);
    Task<Customer> ERPCustomerServiceUpdate(UpdateERPCustomer request, Customer customer, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<List<CustomerRoomName>> CustomerRoomNamesUpdate(List<UpdateCustomerRoomNamesIn> request, int CustomerId, ApplicationDbContext _db, CancellationToken cancellationToken, UserTypeEnum UserType);
    Task<CustomerUser> CustomersUserAdd(CustomerUserIn request, int customerId, ApplicationDbContext _db, CancellationToken cancellationToken);
    //Task CustomersReceptionDelete(CustomerGuestAppReceptionCategory customerReceptionCategory, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task CustomersCategoryItemExtraDelete(List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra> customerGuestAppEnhanceYourStayCategoryItems, ApplicationDbContext _db, CancellationToken cancellationToken);



    //Task CustomersHouseKeepingDelete(CustomerGuestAppHousekeepingCategory customerHouseKeepingCategory, ApplicationDbContext _db, CancellationToken cancellationToken);

    /* Task<CustomerGuestAppHousekeepingCategory> CustomersHouseKeepingCategoryUpdate(CustomerGuestAppHousekeepingCategory existingData, UpdateCustomerHouseKeepingIn request, ApplicationDbContext _db, CancellationToken cancellationToken);
     Task<List<CustomerGuestAppHousekeepingItem>> CustomersHouseKeepingItemsUpdate(List<UpdateCustomerHouseKeepingItemIn> request, int customerHouseKeepingCategoryId, ApplicationDbContext _db, CancellationToken cancellationToken);*/

    Task<Notification> NotificationsAdd(CreateNotificationsIn request, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>> CustomersGuestAppEnhanceYourStayCategoryItemExtra(CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraIn request, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<List<CustomerGuestAppEnhanceYourStayCategory>> CustomerGuestAppEnhanceYourStay(CreateCustomerEnhanceYourStayIn request, ApplicationDbContext _db, CancellationToken cancellationToken);

    Task CustomersConciergeDelete(CustomerGuestAppConciergeCategory customerConciergeCategory, ApplicationDbContext _db, CancellationToken cancellationToken);

    Task<List<NotificationHistory>> NotificationsHistoryAdd(CreateNotificationsIn request, ApplicationDbContext _db, List<UserNotification> GetCustomerForNotification, int Id, CancellationToken cancellationToken, int UserType);

    Task<List<CustomerGuestAppConciergeItem>> CustomersConciergeItemsUpdateV2(List<UpdateCustomerConciergeItemIn> request, int customerConciergeCategoryId, ApplicationDbContext _db, CancellationToken cancellationToken);

    Task<List<CustomerGuestAppConciergeCategory>> CustomersConciergeMultipleAddWithItems(List<CreateCustomerConciergeCategoryIn> request, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<CustomerGuestAppConciergeCategory> CustomersConciergeMultipleUpdateWithItems(UpdateCustomerConciergeCategoryIn request, int CustomerId, ApplicationDbContext _db, CancellationToken cancellationToken);

    Task<List<CustomerGuestAppHousekeepingCategory>> CustomersHouseKeepingMultipleAddWithItems(List<CreateCustomerHouseKeepingCategoryIn> request, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<CustomerGuestAppHousekeepingCategory> CustomersHouseKeepingMultipleUpdateWithItems(UpdateCustomerHouseKeepingCategoryIn request, int CustomerId, ApplicationDbContext _db, CancellationToken cancellationToken);

    Task<List<CustomerGuestAppReceptionCategory>> CustomersReceptionMultipleAddWithItems(List<CreateCustomerReceptionCategoryIn> request, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<CustomerGuestAppReceptionCategory> CustomersReceptionMultipleUpdateWithItems(UpdateCustomerReceptionCategoryIn request, int CustomerId, ApplicationDbContext _db, CancellationToken cancellationToken);

    Task<CustomerGuestAppRoomServiceCategory> CustomersRoomServiceAdd(CreateCustomerRoomServiceCategoryIn request, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<List<CustomerGuestAppRoomServiceItem>> CustomersRoomServiceItemsAdd(List<CreateCustomerRoomServiceItemIn> request, int customerRoomServiceCategoryId, ApplicationDbContext _db, CancellationToken cancellationToken);

    Task CustomersRoomServiceDelete(CustomerGuestAppRoomServiceCategory customerRoomServiceCategory, ApplicationDbContext _db, CancellationToken cancellationToken);

    Task<CustomerGuestAppRoomServiceCategory> CustomersRoomServiceCategoryUpdate(CustomerGuestAppRoomServiceCategory existingData, UpdateCustomerRoomServiceCategoryIn request, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<List<CustomerGuestAppRoomServiceItem>> CustomersRoomServiceItemsUpdate(List<UpdateCustomerRoomServiceItemIn> request, int customerRoomServiceCategoryId, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<List<CreateCustomerRoomServiceCategoryIn>> CustomerRoomServiceMultipleAddWithItems(CreateCustomerRoomServiceIn request, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<CustomerGuestAppRoomServiceCategory> CustomersRoomServiceMultipleUpdateWithItems(UpdateCustomerRoomServiceCategoryIn request, int CustomerId, ApplicationDbContext _db, CancellationToken cancellationToken);

    Task<CustomerPropertyService> CustomerPropertyServiceAdd(CreateCustomerPropertyServiceIn request, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<List<CustomerPropertyServiceImage>> CustomerPropertyServiceImageAdd(List<CustomerPropertyServiceImageIn> request, int Id, ApplicationDbContext _db, CancellationToken cancellationToken);

    Task<List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>> CustomersEnhanceYourStayCategoryItemExtraAdd(List<CreateEnhanceYourStayCategoryItemExtraIn> request, int CustomerGuestAppEnhanceYourStayItemId, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<List<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>> CustomersEnhanceYourStayCategoryItemExtraUpdate(List<UpdateEnhanceYourStayCategoryItemExtraIn> request, int CustomerGuestAppEnhanceYourStayItemId, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<List<CustomerGuestAppEnhanceYourStayItemsImage>> CustomersEnhanceYourStayItemImageAdd(List<CreateEnhanceYourStayItemAttachementIn> request, int CustomerGuestAppEnhanceYourStayItemId, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<List<CustomerGuestAppEnhanceYourStayItemsImage>> CustomersEnhanceYourStayItemImageUpdate(List<UpdateEnhanceYourStayItemAttachementIn> request, int CustomerGuestAppEnhanceYourStayItemId, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<CustomerGuestAppEnhanceYourStayItem> CustomerEnhanceYourStayItemAdd(CreateCustomerEnhanceYourStayItemIn request, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<CustomerGuestAppEnhanceYourStayItem> CustomerEnhanceYourStayItemUpdate(UpdateCustomerEnhanceYourStayItemIn request, CustomerGuestAppEnhanceYourStayItem existingItem, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<CustomerPropertyExtra> CustomerPropertyExtraAddEdit(CustomerPropertyExtrasIn request, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<List<CustomerPropertyExtraDetails>> CustomerPropertyExtraDetailsAddEdit(List<CustomerPropertyExtraDetailsIn> request, int Id, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<CustomerPaymentProcessorCredentials> GetPaymentProcessorCredential(int CustomerId, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<CustomerGuest> GetGuestBuyer(int GuestId, string merchantid, string token, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<CustomerGuest> GetAdminGuestBuyer(int GuestId, string merchantid, string token, ApplicationDbContext _db, CancellationToken cancellationToken);

    Task<List<ReplicateDateModelForOldData>> GetOldGuestAppBuilderData(int id, CancellationToken cancellationToken, IDapperRepository _dapper);

    Task<List<CustomerGuestAppBuildersOutId>> GetNewGusestAppBuilderData(int id, CancellationToken cancellationToken, IDapperRepository _dapper);
    Task<bool> DeleteGuestAppBuilderData(List<ReplicateDateModelForOldData> replicateDateModelForOldData, ApplicationDbContext _db, CancellationToken cancellationToken);
    Task<bool> AddGuestAppBuilderData(List<CustomerGuestAppBuildersOutId> replicateDateModelForNewData, ApplicationDbContext _db, CancellationToken cancellationToken, ReplicateDataIn In);

    Task<string> ResizeImageFromUrlAsync(string imageUrl, int width, int height, IUserFilesService _userFilesService);

}
