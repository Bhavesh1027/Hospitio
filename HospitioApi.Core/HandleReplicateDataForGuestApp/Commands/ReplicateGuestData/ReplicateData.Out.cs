using HospitioApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Core.HandleReplicateDateForGuestApp.Commands.ReplicateGuestData
{
    public class ReplicateDataOut : BaseResponseOut
    {
        public ReplicateDataOut(string message) : base(message)
        {
            //CustomerGuestAppBuilders = customerGuestAppBuilders;
        }
        //public List<CustomerGuestAppBuildersOutId> CustomerGuestAppBuilders { get; set; } = new List<CustomerGuestAppBuildersOutId>();

    }

    public class CustomerGuestAppBuildersOutId
    {
        public  int? Id { get; set; }

        public int? CustomerId { get; set; }

        public int? CustomerRoomNameId { get; set; }

        public string? Message { get; set; }

        public string? SecondaryMessage { get; set; }

        public bool? LocalExperience { get; set; }

        public bool? Ekey { get; set; }

        public bool? PropertyInfo { get; set; }

        public bool? EnhanceYourStay { get; set; }

        public bool? Reception { get; set; }

        public bool? Housekeeping { get; set; }
        public bool? RoomService { get; set; }
        public bool? Concierge { get; set; }
        public bool? TransferServices { get; set; }
        public bool? OnlineCheckIn { get; set; }
        public bool? IsActive { get; set; }
        public byte? IsWork { get; set; }
        public List<ScreenDisplayOrderAndStatusBuilderOut>? DisplayOrderForGuestBuilder { get; set; }
        public List<PropertyInfoOut>? CustomerPropertyinfo { get; set; }

        public List<CustomerGuestAppEnhanceYourStayCategoryOut> CustomerGuestAppEnhanceYourStayCategories { get; set; }

        public List<CustomerGuestAppReceptionCategoryOut> CustomerGuestAppReceptionCategories { get; set; }

        public List<CustomerGuestAppHousekeepingCategoryOut> CustomerGuestAppHousekeepingCategories { get; set; }

        public List<CustomerGuestAppRoomServiceCategoryOut> CustomerGuestAppRoomServiceCategories { get; set; }

        public List<CustomerGuestAppConciergeCategoryOut> CustomerGuestAppConciergeCategories { get; set; }
    }
    #region DisplayOrder 

    public class ScreenDisplayOrderAndStatusBuilderOut
    {
        public int? Id { get; set; }

        public int? ScreenName { get; set; }
        public string? JsonData { get; set; }
        public int? RefrenceId { get; set; }
        public bool? IsActive { get; set; }
    }

    #endregion
    #region propertyinfo



    public class PropertyInfoOut
    {
        public int? Id { get;set; }
        public int? CustomerId { get; set; }
        public int? CustomerGuestAppBuilderId { get; set; }
        
        public string? WifiUsername { get; set; }
        
        public string? WifiPassword { get; set; }
        public string? Overview { get; set; }
        public string? CheckInPolicy { get; set; }
        public string? TermsAndConditions { get; set; }
        
        public string? Street { get; set; }
       
        public string? StreetNumber { get; set; }
        
        public string? City { get; set; }
        
        public string? Postalcode { get; set; }
        
        public bool? IsActive { get; set; }
        public string? Country { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public bool? IsPublish { get; set; }
        public List<CustomerPropertyServiceOut>? CustomerPropertyServices { get; set; }

        public List<CustomerPropertyGalleryOut>? CustomerPropertyGallery { get; set; }

        public List<CustomerPropertyExtraOut>? CustomerPropertyExtras { get; set; }

        public List<CustomerPropertyEmergencyNumberOut>? CustomerPropertyEmergencyNo { get; set; }

        public List<ScreenDisplayOrderAndStatusOut>? DisplayOrderForPropertyInfo { get; set; }

    }

    public class ScreenDisplayOrderAndStatusOut
    {
        public int? Id { get; set; }

        public int? ScreenName { get; set; }
        public string? JsonData { get; set; }
        public int? RefrenceId { get; set; }
        public bool? IsActive { get; set; }

    }
    public class CustomerPropertyServiceOut
    {
        public int? Id { get; set; }
        public int? CustomerPropertyInformationId { get; set; }
       
        public string? Name { get; set; }

        public bool? IsActive { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public bool? IsPublish { get; set; }

        public List<CustomerPropertyServiceImageOut>? CustomerPropertyServiceImages { get; set; } = new List<CustomerPropertyServiceImageOut>();
    }

    public class CustomerPropertyServiceImageOut
    {
        public int? Id { get; set; }
        public int? CustomerPropertyServiceId { get; set; }
       
        public string? ServiceImages { get; set; }
        public bool? IsPublish { get; set; }
        public bool? IsActive { get; set; }
    }

    public class CustomerPropertyGalleryOut
    {
        public int? Id { get; set; }
        public int? CustomerPropertyInformationId { get; set; }
        public string? PropertyImage { get; set; }
        public bool? IsPublish { get; set; }

        public bool? IsActive { get; set; }
    }

    public class CustomerPropertyExtraOut
    {
        public int? Id { get; set; }
        public int? CustomerPropertyInformationId { get; set; }
       
        public byte? ExtraType { get; set; }
       
        public string? Name { get; set; }
      
        public bool? IsPublish { get; set; }
       
        public bool? IsActive { get; set; }

        public int? DisplayOrder { get; set; }
        public List<CustomerPropertyExtraDetailsOut> CustomerPropertyExtraDetails { get; set; } = new List<CustomerPropertyExtraDetailsOut>();
    }

    public class CustomerPropertyExtraDetailsOut
    {
        public int? Id { get; set; }
        public int? CustomerPropertyExtraId { get; set; }
        
        public string? Description { get; set; }

        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public bool? IsPublish { get; set; }
       
        public bool? IsActive { get; set; }
    }

    public class CustomerPropertyEmergencyNumberOut
    {
        public int? Id { get; set; }
        public int? CustomerPropertyInformationId { get; set; }
       
        public string? Name { get; set; }
      
        public string? PhoneNumber { get; set; }
        public bool? IsPublish { get; set; }
     
        public bool? IsActive { get; set; }

        public int? DisplayOrder { get; set; }
    }
    #endregion

    #region EnhanchyourStay

    public class CustomerGuestAppEnhanceYourStayItemOut
    {
        public int? Id { get; set; }
        public int? CustomerGuestAppBuilderId { get; set; }
        public int? CustomerId { get; set; }
        public int? CustomerGuestAppBuilderCategoryId { get; set; }
        
        public byte? Badge { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        
        public byte? ButtonType { get; set; }
 
        public string? ButtonText { get; set; }
        public byte? ChargeType { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }
        public string? Currency { get; set; }
        public int? DisplayOrder { get; set; }
         public bool? IsPublish { get; set; }

        public List<CustomerGuestAppEnhanceYourStayItemsImageOut>? CustomerGuestAppEnhanceYourStayItemsImages { get; set; }

        public List<CustomerGuestAppEnhanceYourStayCategoryItemsExtraOut>? CustomerGuestAppEnhanceYourStayCategoryItemsExtras { get; set; }

    }

    public class CustomerGuestAppEnhanceYourStayCategoryOut
    {
        public int? Id { get; set; }
        public int? CustomerGuestAppBuilderId { get; set; }
        public int? CustomerId { get; set; }

        public bool? IsActive { get; set; }
        public string? CategoryName { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsPublish { get; set; }

        public List<CustomerGuestAppEnhanceYourStayItemOut>? CustomerGuestAppEnhanceYourStayItems { get; set; }

       

    }


    public class CustomerGuestAppEnhanceYourStayItemsImageOut
    {
        public int? Id { get; set; }
        public int? CustomerGuestAppEnhanceYourStayItemId { get; set; }

        public bool? IsActive { get; set; }
        public string? ItemsImages { get; set; }
        public int? DisaplayOrder { get; set; }
        public bool? IsPublish { get; set; }
    }

    public class CustomerGuestAppEnhanceYourStayCategoryItemsExtraOut
    {
        public int? Id { get; set; }
        public int? CustomerGuestAppEnhanceYourStayItemId { get; set; }
        
        public byte? QueType { get; set; }
      
        public string? Questions { get; set; }
       
        public string? OptionValues { get; set; }
        public bool? IsPublish { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region reception

    public class CustomerGuestAppReceptionItemOut
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public int? CustomerGuestAppBuilderId { get; set; }
        public int? CustomerGuestAppReceptionCategoryId { get; set; }
       
        public string? Name { get; set; }
       
        public bool? ItemsMonth { get; set; }
       
        public bool? ItemsDay { get; set; }
       
        public bool? ItemsMinute { get; set; }
       
        public bool? ItemsHour { get; set; }
       
        public bool? QuantityBar { get; set; }
       
        public bool? ItemLocation { get; set; }
       
        public bool? Comment { get; set; }
       
        public bool? IsPriceEnable { get; set; }
        public decimal? Price { get; set; }
       
        public string? Currency { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsPublish { get; set; }
        public bool? IsActive { get; set; }

    }


    public class CustomerGuestAppReceptionCategoryOut
    {
        public int? Id { get; set; }
        public int? CustomerGuestAppBuilderId { get; set; }
        public int? CustomerId { get; set; }
        
        public string? CategoryName { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsPublish { get; set; }
        public bool? IsActive { get; set; }

        public List<CustomerGuestAppReceptionItemOut>? ReceptionItem { get; set; }
    }
    #endregion

    #region HouseKeeping 


    public class CustomerGuestAppHousekeepingItemOut
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public int? CustomerGuestAppBuilderId { get; set; }
        public int? CustomerGuestAppHousekeepingCategoryId { get; set; }

        public string? Name { get; set; }
        
        public bool? ItemsMonth { get; set; }
        
        public bool? ItemsDay { get; set; }
        
        public bool? ItemsMinute { get; set; }
        
        public bool? ItemsHour { get; set; }
        
        public bool? QuantityBar { get; set; }
        
        public bool? ItemLocation { get; set; }
        
        public bool? Comment { get; set; }
        
        public bool? IsPriceEnable { get; set; }
        public decimal? Price { get; set; }
        
        public string? Currency { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsPublish { get; set; }
      
        public bool? IsActive { get; set; }


    }

    public class CustomerGuestAppHousekeepingCategoryOut
    {
        public int? Id { get; set; }
        public int? CustomerGuestAppBuilderId { get; set; }
        public int? CustomerId { get; set; }
       
        public string? CategoryName { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsPublish { get; set; }
       
        public bool? IsActive { get; set; }

        public List<CustomerGuestAppHousekeepingItemOut>? HouseItem { get; set; }
    }
    #endregion


    #region Roomservice

    public class CustomerGuestAppRoomServiceItemOut
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public int? CustomerGuestAppBuilderId { get; set; }
        public int? CustomerGuestAppRoomServiceCategoryId { get; set; }
        
        public string? Name { get; set; }
        
        public bool? ItemsMonth { get; set; }
        
        public bool? ItemsDay { get; set; }
        
        public bool? ItemsMinute { get; set; }
        
        public bool? ItemsHour { get; set; }
        
        public bool? QuantityBar { get; set; }
        
        public bool? ItemLocation { get; set; }
        
        public bool? Comment { get; set; }
     
        public bool? IsPriceEnable { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsPublish { get; set; }
    
        public bool? IsActive { get; set; }


    } 

    public class CustomerGuestAppRoomServiceCategoryOut
    {
        public int? Id { get; set; }
        public int? CustomerGuestAppBuilderId { get; set; }
        public int? CustomerId { get; set; }
        
        public string? CategoryName { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsPublish { get; set; }
     
        public bool? IsActive { get; set; }

        public List<CustomerGuestAppRoomServiceItemOut>? RoomItem { get; set; }
    }

    #endregion


    #region concirage

    public class CustomerGuestAppConciergeItemOut
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public int? CustomerGuestAppBuilderId { get; set; }
        public int? CustomerGuestAppConciergeCategoryId { get; set; }
        
        public string? Name { get; set; }
        
        public bool? ItemsMonth { get; set; }
        
        public bool? ItemsDay { get; set; }
        
        public bool? ItemsMinute { get; set; }
        
        public bool? ItemsHour { get; set; }
        
        public bool? QuantityBar { get; set; }
        
        public bool? ItemLocation { get; set; }
        
        public bool? Comment { get; set; }
        
        public bool? IsPriceEnable { get; set; }
        public decimal? Price { get; set; }
        
        public string? Currency { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsPublish { get; set; }
      
        public bool? IsActive { get; set; }

    }

    public class CustomerGuestAppConciergeCategoryOut
    {
        public int? Id { get; set; }
        public int? CustomerGuestAppBuilderId { get; set; }
        public int? CustomerId { get; set; }
        
        public string? CategoryName { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsPublish { get; set; }
       
        public bool? IsActive { get; set; }

        public List<CustomerGuestAppConciergeItemOut>? Conciergeitem { get; set; }
    }
    #endregion
}
