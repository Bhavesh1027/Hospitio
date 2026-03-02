using HospitioApi.Shared;

namespace HospitioApi.Core.HandleUserAccount.Queries.GetUserProfile
{
    public class GetUserProfileOut : BaseResponseOut
    {
        public GetUserProfileOut(string message, GetProfileOut getProfileOut) : base(message)
        {
            GetProfileOut = getProfileOut;
        }

        public GetProfileOut GetProfileOut { get; set; } = new GetProfileOut();
    }

    public class GetProfileOut
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Title { get; set; }
        public string? ProfilePicture { get; set; }
        public string? PhoneCountry { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DepartmentName { get; set; }
        public string? IncomingTranslationLangage { get; set; }
        public string? NoTranslateWords { get; set; }
        public string? UserName { get; set; }
        public bool? IsActive { get; set; }
        public string? Password { get; set; }
        public string? GroupName { get; set; }
        public string? SupervisorName { get; set; }
        public string? LevelName { get; set; }
        public string? UserUniqueId { get; set; }
        public int? TaxiTransferCommission {  get; set; }
    }
}
