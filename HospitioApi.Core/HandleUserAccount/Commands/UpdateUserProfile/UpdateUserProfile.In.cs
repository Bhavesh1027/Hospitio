namespace HospitioApi.Core.HandleUserAccount.Commands.UpdateUserProfile
{
    public class UpdateUserProfileIn
    {
        public int? UserId { get; set; } 
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Title { get; set; }
        public string? ProfilePicture { get; set; }
        public string? PhoneCountry { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserName { get; set; }         
        public string? Password { get; set; }
        public int? UserType { get; set; }   
        public string? IncomingTranslationLangage { get; set; }
        public string? NoTranslateWords { get; set; }
        public int? CustomerId { get; set; }
        public int? TaxiTransferCommission {  get; set; }
    }
}
