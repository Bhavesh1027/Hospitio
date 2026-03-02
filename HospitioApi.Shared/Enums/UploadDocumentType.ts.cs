

namespace HospitioApi.Shared.Enums;

public enum UploadDocumentType
{
    profile = 0,
    propertyservice = 1,
    yourstayitems = 2,//GuestAppEnhanceYourStayItemsImages
    articles = 3,//Question and Answers
    documentattachment = 4, // PMS Guest Document 
    basic = 5, // Customer Logo, Main App Image, Splash Screen Image
    propertyinfogallary = 6, // For Propertyinfo Gallary
    enhanceyourstay = 7, // For Enhance your Stay Item
    appBuilderImages = 8, // CustomerGuest APP Builder
    guestProfile = 9, // CustomerGuest Profile Picture
    communicationattachment = 10, // For Communication File Upload
    customerProfile = 11, // For Customer Profile Picture
    customerGuestXMLSheet = 12, // For download CustomerGuests Sheet ( export guests list )
    downloadMusementData = 13,// Download Musement Data For Admin 
    downloadCustomerLeads = 14, // Download Customer Leads For Admin 
    downloadVonageRecordsReports = 15, // For Vonage Logs of Messages and SMS,
    downloadTaxiTransferData = 16 // Download Guest Taxi Transfer Data
}
