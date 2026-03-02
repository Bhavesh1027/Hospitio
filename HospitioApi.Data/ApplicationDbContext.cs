using Microsoft.EntityFrameworkCore;
using HospitioApi.Data.Models;
using HospitioApi.Data.MultiTenancy;

namespace HospitioApi.Data;
public partial class ApplicationDbContext : DbContext
{
    private readonly string tenant;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService iTenantService)
        : base(options)
    {
        tenant = iTenantService.TenantId;
    }

    public virtual DbSet<BusinessType> BusinessTypes { get; set; } = null!;
    public virtual DbSet<Customer> Customers { get; set; } = null!;
    public virtual DbSet<CustomerDigitalAssistant> CustomerDigitalAssistants { get; set; } = null!;
    public virtual DbSet<CustomerGuest> CustomerGuests { get; set; } = null!;
    public virtual DbSet<CustomerGuestAlert> CustomerGuestAlerts { get; set; } = null!;
    public virtual DbSet<CustomerGuestAppBuilder> CustomerGuestAppBuilders { get; set; } = null!;
    public virtual DbSet<CustomerGuestAppConciergeCategory> CustomerGuestAppConciergeCategories { get; set; } = null!;
    public virtual DbSet<CustomerGuestAppConciergeItem> CustomerGuestAppConciergeItems { get; set; } = null!;
    public virtual DbSet<CustomerGuestAppEnhanceYourStayCategory> CustomerGuestAppEnhanceYourStayCategories { get; set; } = null!;
    public virtual DbSet<CustomerGuestAppEnhanceYourStayCategoryItemsExtra> CustomerGuestAppEnhanceYourStayCategoryItemsExtras { get; set; } = null!;
    public virtual DbSet<CustomerGuestAppEnhanceYourStayItem> CustomerGuestAppEnhanceYourStayItems { get; set; } = null!;
    public virtual DbSet<CustomerGuestAppEnhanceYourStayItemsImage> CustomerGuestAppEnhanceYourStayItemsImages { get; set; } = null!;
    public virtual DbSet<CustomerGuestAppHousekeepingCategory> CustomerGuestAppHousekeepingCategories { get; set; } = null!;
    public virtual DbSet<CustomerGuestAppHousekeepingItem> CustomerGuestAppHousekeepingItems { get; set; } = null!;
    public virtual DbSet<CustomerGuestAppReceptionCategory> CustomerGuestAppReceptionCategories { get; set; } = null!;
    public virtual DbSet<CustomerGuestAppReceptionItem> CustomerGuestAppReceptionItems { get; set; } = null!;
    public virtual DbSet<CustomerGuestAppRoomServiceCategory> CustomerGuestAppRoomServiceCategories { get; set; } = null!;
    public virtual DbSet<CustomerGuestAppRoomServiceItem> CustomerGuestAppRoomServiceItems { get; set; } = null!;
    public virtual DbSet<CustomerGuestJourny> CustomerGuestJournies { get; set; } = null!;
    public virtual DbSet<CustomerGuestsCheckInFormBuilder> CustomerGuestsCheckInFormBuilders { get; set; } = null!;
    public virtual DbSet<CustomerGuestsCheckInFormField> CustomerGuestsCheckInFormFields { get; set; } = null!;
    public virtual DbSet<CustomerPaymentProcessor> CustomerPaymentProcessors { get; set; } = null!;
    public virtual DbSet<CustomerPropertyEmergencyNumber> CustomerPropertyEmergencyNumbers { get; set; } = null!;
    public virtual DbSet<CustomerPropertyExtra> CustomerPropertyExtras { get; set; } = null!;
    public virtual DbSet<CustomerPropertyExtraDetails> CustomerPropertyExtraDetails { get; set; } = null!;
    public virtual DbSet<CustomerPropertyGallery> CustomerPropertyGalleries { get; set; } = null!;
    public virtual DbSet<CustomerPropertyInformation> CustomerPropertyInformations { get; set; } = null!;
    public virtual DbSet<CustomerPropertyService> CustomerPropertyServices { get; set; } = null!;
    public virtual DbSet<CustomerPropertyServiceImage> CustomerPropertyServiceImages { get; set; } = null!;
    public virtual DbSet<CustomerReservation> CustomerReservations { get; set; } = null!;
    public virtual DbSet<CustomerRoomName> CustomerRoomNames { get; set; } = null!;
    public virtual DbSet<CustomerStaffAlert> CustomerStaffAlerts { get; set; } = null!;
    public virtual DbSet<CustomerUser> CustomerUsers { get; set; } = null!;
    public virtual DbSet<CustomerUserRefreshToken> CustomerUserRefreshTokens { get; set; } = null!;
    public virtual DbSet<Department> Departments { get; set; } = null!;
    public virtual DbSet<Group> Groups { get; set; } = null!;
    public virtual DbSet<GuestJourneyMessagesTemplate> GuestJourneyMessagesTemplates { get; set; } = null!;
    public virtual DbSet<GuestRequest> GuestRequests { get; set; } = null!;
    public virtual DbSet<Lead> Leads { get; set; } = null!;
    public virtual DbSet<Module> Modules { get; set; } = null!;
    public virtual DbSet<ModuleService> ModuleServices { get; set; } = null!;
    public virtual DbSet<Notification> Notifications { get; set; } = null!;
    public virtual DbSet<NotificationHistory> NotificationHistorys { get; set; } = null!;
    public virtual DbSet<PaymentProcessor> PaymentProcessors { get; set; } = null!;
    public virtual DbSet<Permission> Permissions { get; set; } = null!;
    public virtual DbSet<Product> Products { get; set; } = null!;
    public virtual DbSet<ProductModule> ProductModules { get; set; } = null!;
    public virtual DbSet<ProductModuleService> ProductModuleServices { get; set; } = null!;
    public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; } = null!;
    public virtual DbSet<QuestionAnswerRead> QuestionAnswersRead { get; set; } = null!;
    public virtual DbSet<QuestionAnswerAttachement> QuestionAnswerAttachements { get; set; } = null!;
    public virtual DbSet<QuestionAnswerCategory> QuestionAnswerCategories { get; set; } = null!;
    public virtual DbSet<HospitioOnboarding> HospitioOnboardings { get; set; } = null!;
    public virtual DbSet<HospitioPaymentProcessor> HospitioPaymentProcessors { get; set; } = null!;
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public virtual DbSet<Ticket> Tickets { get; set; } = null!;
    public virtual DbSet<TicketCategory> TicketCategorys { get; set; } = null!;
    public virtual DbSet<TicketReply> TicketReplies { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<UserLevel> UserLevels { get; set; } = null!;
    public virtual DbSet<UsersPermission> UsersPermissions { get; set; } = null!;

    public virtual DbSet<ScreenDisplayOrderAndStatus> ScreenDisplayOrderAndStatuses { get; set; } = null!;
    public virtual DbSet<Channels> Channels { get; set; } = null!;
    public virtual DbSet<ChannelMessages> ChannelMessages { get; set; } = null!;
    public virtual DbSet<ChannelUsers> ChannelUsers { get; set; } = null!;
    public virtual DbSet<ChannelUserTypeCustomerGuest> ChannelUserTypeCustomerGuest { get; set; } = null!;
    public virtual DbSet<ChannelUserTypeChatWidgetUser> ChannelUserTypeChatWidgetUser { get; set; } = null!;
    public virtual DbSet<ChannelUserTypeCustomerUser> ChannelUserTypeCustomerUser { get; set; } = null!;

    public virtual DbSet<ChannelUserTypeUser> ChannelUserTypeUser { get; set; } = null!;

    public virtual DbSet<ChannelUserCustomerGuest> ChannelUserCustomerGuest { get; set; } = null!;
    public virtual DbSet<ChannelUserChatWidgetUser> ChannelUserChatWidgetUser { get; set; } = null!;

    public virtual DbSet<ChannelUserCustomerUser> ChannelUserCustomerUser { get; set; } = null!;
    public virtual DbSet<ChannelUserHospitioUser> ChannelUserHospitioUser { get; set; } = null!;

    public virtual DbSet<AudioMessage> AudioMessage { get; set; } = null!;

    public virtual DbSet<VideoMessage> VideoMessage { get; set; } = null!;

    public virtual DbSet<PdfMessage> PDFMessage { get; set; } = null!;

    public virtual DbSet<TemplateMessage> TemplateMessage { get; set; } = null!;

    public virtual DbSet<TextMessage> TextMessage { get; set; } = null!;

    public virtual DbSet<ImageMessage> ImageMessage { get; set; } = null!;
    public virtual DbSet<EnhanceStayItemsGuestRequest> EnhanceStayItemsGuestRequests { get; set; } = null!;
    public virtual DbSet<EnhanceStayItemExtraGuestRequest> EnhanceStayItemExtraGuestRequests { get; set; } = null!;
    public virtual DbSet<PaymentProcessorsDefinations> PaymentProcessorsDefinations { get; set; } = null!;
    public virtual DbSet<CustomerPaymentProcessorCredentials> CustomerPaymentProcessorCredentials { get; set; } = null!;
    public virtual DbSet<HospitioPaymentProcessorCredentials> HospitioPaymentProcessorCredentials { get; set; } = null!;
    public virtual DbSet<AnonymousUsers> AnonymousUsers { get; set; } = null!;
    public virtual DbSet<AdminStaffAlert> AdminStaffAlerts { get; set; } = null!;
    public virtual DbSet<AdminCustomerAlert> AdminCustomerAlerts { get; set; } = null!;
    public virtual DbSet<VonageCredentials> VonageCredentials { get; set; } = null!;
    public virtual DbSet<CustomerPermission> CustomerPermissions { get; set; } = null!;
    public virtual DbSet<CustomerUsersPermission> CustomerUsersPermissions { get; set; }
    public virtual DbSet<CustomerDepartment> CustomerDepartments { get; set; } = null!;
    public virtual DbSet<CustomerGroup> CustomerGroups { get; set; } = null!;
    public virtual DbSet<CustomerLevel> CustomerLevels { get; set; } = null!;
    public virtual DbSet<MusementGuestInfo> MusementGuestInfos { get; set; } = null!;
    public virtual DbSet<MusementOrderInfo> MusementOrderInfos { get; set; } = null!;
    public virtual DbSet<MusementItemInfo> MusementItemInfos { get; set; } = null!;
    public virtual DbSet<MusementPaymentInfo> MusementPaymentInfos { get; set; } = null!;
    public virtual DbSet<ChatWidgetUser> ChatWidgetUsers { get; set; } = null!;
    public virtual DbSet<TaxiTransferGuestRequest> TaxiTransferGuestRequests { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Channels>()
       .HasDiscriminator<string>("CreateForm")
       .HasValue<ChannelUserCustomerGuest>("CustomerGuest")
       .HasValue<ChannelUserCustomerUser>("CustomerUser")
       .HasValue<ChannelUserHospitioUser>("HospitioUser")
       .HasValue<ChannelUserAnonymousUser>("AnonymousUser")
       .HasValue<ChannelUserChatWidgetUser>("ChatWidgetUser");



        modelBuilder.Entity<ChannelUsers>()
            .HasDiscriminator<string>("UserType")
            .HasValue<ChannelUserTypeCustomerGuest>("CustomerGuest")
            .HasValue<ChannelUserTypeCustomerUser>("CustomerUser")
            .HasValue<ChannelUserTypeUser>("HospitioUser")
            .HasValue<ChannelUserTypeAnonymousUser>("AnonymousUser")
            .HasValue<ChannelUserTypeChatWidgetUser>("ChatWidgetUser");


        modelBuilder.Entity<ChannelUserTypeCustomerGuest>()
             .Property(b => b.UserId)
             .HasColumnName("UserId");

        modelBuilder.Entity<ChannelUserTypeChatWidgetUser>()
       .Property(b => b.UserId)
       .HasColumnName("UserId");

        modelBuilder.Entity<ChannelUserTypeUser>()
        .Property(b => b.UserId)
        .HasColumnName("UserId");

        modelBuilder.Entity<ChannelUserTypeCustomerUser>()
       .Property(b => b.UserId)
       .HasColumnName("UserId");

        modelBuilder.Entity<ChannelUserTypeAnonymousUser>()
       .Property(b => b.UserId)
       .HasColumnName("UserId");

        modelBuilder.Entity<ChannelUsers>()
            .HasOne<Channels>(s => s.Channels)
            .WithMany(g => g.ChannelUsers)
            .HasForeignKey(s => s.ChannelId);

        modelBuilder.Entity<ChannelUsers>()
        .HasOne<ChannelMessages>(s => s.ChannelMessages)
        .WithOne(g => g.ChannelUsers)
        .HasForeignKey<ChannelUsers>(g=>g.LastMessageReadId)
        .OnDelete(DeleteBehavior.NoAction);


        modelBuilder.Entity<ChannelMessages>()
           .HasOne<Channels>(s => s.Channels)
           .WithMany(g => g.ChannelMessages)
           .HasForeignKey(s => s.ChannelId);


        modelBuilder.Entity<ChannelMessages>()
           .HasOne<GuestRequest>(s => s.GuestRequests)
           .WithMany(g => g.ChannelMessages)
           .HasForeignKey(s => s.RequestId);


        modelBuilder.Entity<ChannelMessages>()
         .HasDiscriminator(b => b.MessageType)
         .HasValue<AudioMessage>("Audio")
           .HasValue<VideoMessage>("Video")
             .HasValue<ImageMessage>("Image")
               .HasValue<TextMessage>("Text")
                .HasValue<PdfMessage>("Pdf")
                .HasValue<TemplateMessage>("Template");



        modelBuilder.Entity<VideoMessage>()
         .Property(b => b.Url)
         .HasColumnName("Url");

        modelBuilder.Entity<TextMessage>()
        .Property(b => b.Message)
        .HasColumnName("Message");

        modelBuilder.Entity<TemplateMessage>()
        .Property(b => b.Url)
        .HasColumnName("Url");

        modelBuilder.Entity<TemplateMessage>()
            .Property(b => b.Message)
            .HasColumnName("Message");

        modelBuilder.Entity<PdfMessage>()
       .Property(b => b.Url)
       .HasColumnName("Url");

        modelBuilder.Entity<AudioMessage>()
       .Property(b => b.Url)
       .HasColumnName("Url");

        modelBuilder.Entity<ImageMessage>()
       .Property(b => b.Url)
       .HasColumnName("Url");

        modelBuilder.Entity<Department>().HasOne(d => d.DepartmentManger)
            .WithMany(p => p.Departments)
            .HasForeignKey(d => d.DepartmentMangerId);

        modelBuilder.Entity<Group>().HasOne(d => d.GroupLeader)
            .WithMany(p => p.Groups)
            .HasForeignKey(d => d.GroupLeaderId);

        modelBuilder.Entity<CustomerDepartment>().HasOne(d => d.DepartmentManger)
            .WithMany(p => p.CustomerDepartments)
            .HasForeignKey(d => d.DepartmentMangerId);

        modelBuilder.Entity<CustomerGroup>().HasOne(d => d.GroupLeader)
            .WithMany(p => p.CustomerGroups)
            .HasForeignKey(d => d.GroupLeaderId);


        modelBuilder.Entity<BusinessType>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerDigitalAssistant>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuest>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestAlert>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestAppBuilder>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestAppEnhanceYourStayCategory>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestAppEnhanceYourStayCategoryItemsExtra>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestAppEnhanceYourStayItem>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestAppEnhanceYourStayItemsImage>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestAppHousekeepingCategory>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestAppHousekeepingItem>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestAppReceptionCategory>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestAppReceptionItem>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestAppRoomServiceCategory>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestAppRoomServiceItem>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestJourny>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestsCheckInFormBuilder>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestsCheckInFormField>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerPaymentProcessor>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerPropertyEmergencyNumber>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerPropertyExtra>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerPropertyExtraDetails>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerPropertyGallery>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        //modelBuilder.Entity<CustomerPropertyEmergencyNumber>(entity =>
        //{
        //    entity.HasQueryFilter(e => e.DeletedAt == null);
        //});

        modelBuilder.Entity<CustomerPropertyInformation>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerPropertyService>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerPropertyServiceImage>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerReservation>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerRoomName>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerStaffAlert>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerUser>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<GuestJourneyMessagesTemplate>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<GuestRequest>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<Lead>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<ModuleService>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<NotificationHistory>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<PaymentProcessor>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<ProductModule>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<ProductModuleService>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<QuestionAnswer>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<QuestionAnswerRead>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<QuestionAnswerAttachement>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<QuestionAnswerCategory>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<HospitioPaymentProcessor>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<TicketCategory>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<TicketReply>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<UserLevel>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<UsersPermission>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<ChannelMessages>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<Channels>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<ChannelUsers>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<EnhanceStayItemsGuestRequest>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<EnhanceStayItemExtraGuestRequest>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<PaymentProcessorsDefinations>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<CustomerPaymentProcessorCredentials>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<HospitioPaymentProcessorCredentials>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<CustomerGuestAppConciergeCategory>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<CustomerGuestAppConciergeItem>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<HospitioOnboarding>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<ScreenDisplayOrderAndStatus>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<VonageCredentials>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<AdminCustomerAlert>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<AdminStaffAlert>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });

        modelBuilder.Entity<AnonymousUsers>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<CustomerDepartment>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<CustomerGroup>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<CustomerUsersPermission>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });
        modelBuilder.Entity<CustomerLevel>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
        });


        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
