using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using HospitioApi.Data;
using HospitioApi.Data.MultiTenancy;
using HospitioApi.Test.EntityFactories;
using System.Data.Common;

namespace HospitioApi.Test;

public abstract class DbFixture : IDisposable
{
    private readonly DbConnection _connection;
    public DbContextOptions<ApplicationDbContext> DbContextOptions { get; }
    public ITenantService TenantService { get; }
    public CustomerGuestFactory CustomerGuestFactory { get; }
    public CustomerReservationFactory CustomerReservationFactory { get; }
    public CustomerGuestAlertFactory CustomerGuestAlertFactory { get; }
    public CustomerFactory CustomerFactory { get; }
    public CustomerEnhanceYourStayCategoryFactory customerEnhanceYourStayCategoryFactory { get; }
    public CustomerGuestAppBuilderFactory CustomerGuestAppBuilderFactory { get; }
    public CustomerFactory customerFactory { get; }
    public CustomerGuestAppEnhanceYourStayItemFactory CustomerGuestAppEnhanceYourStayItemFactory { get; }
    public CustomerGuestAppEnhanceYourStayCategoryItemsExtraFactory CustomerGuestAppEnhanceYourStayCategoryItemsExtraFactory { get; }
    public ProductFactory ProductFactory { get; }
    public ProductModuleFactory ProductModuleFactory { get; }
    public ProductModuleServiceFactory ProductModuleServiceFactory { get; }
    public ModuleFactory ModuleFactory { get; }
    public ModuleServiceFactory ModuleServiceFactory { get; }
    public UserFactory UserFactory { get; }
    public CustomerRoomNamesRepository CustomerRoomNamesRepository { get; }
    public CustomerLoginFactory CustomerLoginFactory { get; }
    public LoginFactory LoginFactory { get; }
    public CustomerUserFactory CustomerUserFactory { get; }
    public HospitioOnboardingsFactory HospitioOnboardingsFactory { get; }
    public CustomerStaffAlertFactory CustomerStaffAlertFactory { get; }
    public BusinessTypeFactory BusinessTypeFactory { get; }
    public CustomerGuestsCheckInFormBuildersFactory CustomerGuestsCheckInFormBuildersFactory { get; }
    public TicketFactory TicketFactory { get; }
    
    public PaymentProcessorFactory PaymentProcessorFactory { get; }
    public CustomerPaymentProcessorFactory CustomerPaymentProcessorFactory { get; }
    public CustomerLevelFactory CustomerLevelFactory { get; }

    public DepartmentFactory DepartmentFactory { get; }

    public LeadsFactory LeadsFactory { get; }
    public CustomersDigitalAssistantsFactory CustomersDigitalAssistantsFactory { get; }
    public TicketCategoryFactory TicketCategoryFactory { get; }

    public CustomerGuestJourneyFactory CustomerGuestJourneyFactory { get; }
    public GuestJourneyMessagesTemplatesFactory GuestJourneyMessagesTemplatesFactory { get; }
    public QuestionAnswerCategoryFactory QuestionAnswerCategoryFactory { get; }
    public QuestionAnswerFactory QuestionAnswerFactory { get; }
    public PermissionFactory PermissionFactory { get; }
    public CustomerGuestAppHousekeepingCategoryFactory housekeepingCategoryFactory { get; }
    public CustomerPropertyExtraFactory propertyExtraFactory { get; }
    public CustomerProperyInformationFactory customerProperyInformationFactory { get; }
    public CustomerPropertyExtraDetailFactory customerPropertyExtraDetailFactory { get; }
    public UserLevelFactory UserLevelFactory { get; }
    public GroupsFactory GroupsFactory { get; }
    public CustomerReceptionCategoryFactory CustomerReceptionCategoryFactory { get; }
    public CustomerRoomServiceCategoryFactory CustomerRoomServiceCategoryFactory { get; }
    public CustomerConciergeCategoryFactory CustomerConciergeCategoryFactory { get; }
    public CustomerPropertyServiceFactory CustomerPropertyServiceFactory { get; }
    public CustomerPropertyServiceImageFactory CustomerPropertyServiceImageFactory { get; }
    public GuestRequestFactory GuestRequestFactory { get; }

    public NotificationFactory NotificationFactory { get; }
    public ScreenDisplayOrderAndStatusesFactory ScreenDisplayOrderAndStatusesFactory { get; }

    public CustomerPropertyGalleryFactory CustomerPropertyGalleryFactory { get; }
    public HospitioPaymentProcessorFactory HospitioPaymentProcessorFactory { get; }
    public CustomerPropertyEmergencyNumberFactory CustomerPropertyEmergencyNumberFactory { get; }
    public VonageCredentialFactory  VonageCredentialFactory { get; }
    public AdminCustomerAlertFactory AdminCustomerAlertFactory { get; }
    public AdminStaffAlertFactory AdminStaffAlertFactory { get; }
    public ChannelUserCustomerUserFactory ChannelUserCustomerUserFactory { get; }
    public ChannelUserTypeCustomerUserFactory ChannelUserTypeCustomerUserFactory { get; }
    public CustomerDepartmentsFactory CustomerDepartmentsFactory { get; }
    public PaymentProcessorsDefinationsFactory PaymentProcessorsDefinationsFactory { get; }
    public MusementGuestInfoFactory MusementGuestInfoFactory { get; }   
    public MusementItemInfoFactory MusementItemInfoFactory { get; }
    public MusementOrderInfoFactory MusementOrderInfoFactory { get; }
    public MusementPaymentInfoFactory MusementPaymentInfoFactory  { get; }
    public CustomerGuestAppEnhanceYourStayItemsImagesFactory CustomerGuestAppEnhanceYourStayItemsImagesFactory { get; }
    public CustomerPermissionFactory customerPermissionFactory { get; }
    public CustomerUsersPermissionFactory customerUsersPermissionFactory { get; }
    public CustomerGroupFactory customerGroupFactory { get; }
    public DbFixture()
    {
        DbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options;
        var connection = RelationalOptionsExtension.Extract(DbContextOptions).Connection;
        if (connection is null)
        {
            throw new ArgumentNullException(nameof(connection));
        }
        _connection = connection;
        TenantService = new TenantService();

        CustomerGuestFactory = new CustomerGuestFactory();
        CustomerReservationFactory = new CustomerReservationFactory();
        CustomerGuestAlertFactory = new CustomerGuestAlertFactory();
        CustomerFactory = new CustomerFactory(); 
        customerEnhanceYourStayCategoryFactory = new CustomerEnhanceYourStayCategoryFactory();
        customerFactory = new CustomerFactory();
        CustomerGuestAppBuilderFactory = new CustomerGuestAppBuilderFactory();
        CustomerGuestAppEnhanceYourStayItemFactory = new CustomerGuestAppEnhanceYourStayItemFactory();
        CustomerGuestAppEnhanceYourStayCategoryItemsExtraFactory = new CustomerGuestAppEnhanceYourStayCategoryItemsExtraFactory();
        ProductFactory = new ProductFactory();
        ProductModuleFactory = new ProductModuleFactory();
        ProductModuleServiceFactory = new ProductModuleServiceFactory();
        ModuleFactory = new ModuleFactory();
        ModuleServiceFactory = new ModuleServiceFactory();
        UserFactory = new UserFactory();
        CustomerRoomNamesRepository = new CustomerRoomNamesRepository();
        CustomerGuestsCheckInFormBuildersFactory = new CustomerGuestsCheckInFormBuildersFactory();
        CustomerLoginFactory = new CustomerLoginFactory();
        LoginFactory = new LoginFactory();
        CustomerUserFactory = new CustomerUserFactory();
        HospitioOnboardingsFactory = new HospitioOnboardingsFactory();
        CustomerStaffAlertFactory = new CustomerStaffAlertFactory();
        LeadsFactory = new LeadsFactory();
        DepartmentFactory = new DepartmentFactory();
        CustomerLevelFactory = new CustomerLevelFactory();
        CustomersDigitalAssistantsFactory = new CustomersDigitalAssistantsFactory();        
        TicketFactory = new TicketFactory();
        CustomerGuestJourneyFactory = new CustomerGuestJourneyFactory();
        GuestJourneyMessagesTemplatesFactory = new GuestJourneyMessagesTemplatesFactory();
        PaymentProcessorFactory = new PaymentProcessorFactory();
        PermissionFactory = new PermissionFactory();
        CustomerPaymentProcessorFactory = new CustomerPaymentProcessorFactory();
        BusinessTypeFactory = new BusinessTypeFactory();
        TicketCategoryFactory = new TicketCategoryFactory();
        housekeepingCategoryFactory = new CustomerGuestAppHousekeepingCategoryFactory();
        propertyExtraFactory = new CustomerPropertyExtraFactory();
        customerProperyInformationFactory = new CustomerProperyInformationFactory();
        customerPropertyExtraDetailFactory = new CustomerPropertyExtraDetailFactory();
        QuestionAnswerCategoryFactory = new QuestionAnswerCategoryFactory();
        QuestionAnswerFactory = new QuestionAnswerFactory();
        UserLevelFactory = new UserLevelFactory();
        GroupsFactory = new GroupsFactory();
        CustomerReceptionCategoryFactory = new CustomerReceptionCategoryFactory();
        CustomerRoomServiceCategoryFactory = new CustomerRoomServiceCategoryFactory();
        CustomerConciergeCategoryFactory = new CustomerConciergeCategoryFactory();
        GuestRequestFactory = new GuestRequestFactory();
        CustomerPropertyServiceFactory = new CustomerPropertyServiceFactory();
        CustomerPropertyServiceImageFactory = new CustomerPropertyServiceImageFactory();
        NotificationFactory = new NotificationFactory();
        ScreenDisplayOrderAndStatusesFactory = new ScreenDisplayOrderAndStatusesFactory();
        CustomerPropertyGalleryFactory = new CustomerPropertyGalleryFactory();
        HospitioPaymentProcessorFactory = new HospitioPaymentProcessorFactory();
        CustomerPropertyEmergencyNumberFactory = new CustomerPropertyEmergencyNumberFactory();
        VonageCredentialFactory = new VonageCredentialFactory();
        AdminCustomerAlertFactory = new AdminCustomerAlertFactory();
        AdminStaffAlertFactory = new AdminStaffAlertFactory();
        ChannelUserCustomerUserFactory = new ChannelUserCustomerUserFactory();
        ChannelUserTypeCustomerUserFactory = new ChannelUserTypeCustomerUserFactory();
        CustomerDepartmentsFactory = new CustomerDepartmentsFactory();
        PaymentProcessorsDefinationsFactory = new PaymentProcessorsDefinationsFactory();
        MusementGuestInfoFactory = new MusementGuestInfoFactory();
        MusementItemInfoFactory = new MusementItemInfoFactory();
        MusementOrderInfoFactory = new MusementOrderInfoFactory();
        MusementPaymentInfoFactory = new MusementPaymentInfoFactory();
        CustomerGuestAppEnhanceYourStayItemsImagesFactory = new CustomerGuestAppEnhanceYourStayItemsImagesFactory();
        customerPermissionFactory = new CustomerPermissionFactory();
        customerUsersPermissionFactory = new CustomerUsersPermissionFactory();
        customerGroupFactory = new CustomerGroupFactory();
        Seed();
    }

    private static DbConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        connection.CreateFunction("newid", () => Guid.NewGuid());
        connection.CreateFunction("getdate", () => DateTime.Now);
        return connection;
    }

    protected virtual void Seed()
    {
        throw new Exception("Seed method must be overridden by derived class.");
    }

    public void Dispose()
    {
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
