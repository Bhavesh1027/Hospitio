

namespace HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Commands.CreateCustomerGuestsCheckInFormBuilder
{
    public class CreateCustomerGuestsCheckInFormBuilderIn
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? Color { get; set; }
        public string? Name { get; set; }
        public byte? Stars { get; set; }
        public string? Logo { get; set; }
        public string? AppImage { get; set; }
        public string? SplashScreen { get; set; }
        public bool IsOnlineCheckInFormEnable { get; set; } = true;
        public bool IsRedirectToGuestAppEnable { get; set; } = true;
        public string? SubmissionMail { get; set; }
        public string? TermsLink { get; set; }
        public bool IsActive { get; set; } = true;
        public string? GuestWelcomeMessage { get; set; }
        public List<CustomerGuestsCheckInFormFieldIn> CustomerGuestsCheckInFormFieldIn { get; set; } = new();
    }
    public class CustomerGuestsCheckInFormFieldIn
    {
        public string Name { get; set; } = String.Empty;
        public string FieldType { get; set; } = String.Empty;
        public bool RequiredFields { get; set; }
        public bool IsActive { get; set; } = true;
        public int? DisplayOrder { get; set; }
    }

}
