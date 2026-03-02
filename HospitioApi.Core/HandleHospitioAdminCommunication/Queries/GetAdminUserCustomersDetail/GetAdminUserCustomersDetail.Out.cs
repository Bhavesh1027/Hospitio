using HospitioApi.Shared;

namespace HospitioApi.Core.HandleHospitioAdminCommunication.Queries.GetAdminUserCustomersDetail;
public class GetAdminUserCustomersDetailOut : BaseResponseOut
{
    public GetAdminUserCustomersDetailOut(string message, GetAdminUserCustomersDetailResponseOut getAdminUserCustomersDetailResponseOut) : base(message)
    {
        GetAdminUserCustomersDetailResponseOut = getAdminUserCustomersDetailResponseOut;
    }
    public GetAdminUserCustomersDetailResponseOut GetAdminUserCustomersDetailResponseOut { get; set; }
}
public class GetAdminUserCustomersDetailResponseOut
{
    public int? UserId { get; set; }
    public string? BusinessName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? ProfilePicture { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public string? IncomingTranslationLangage { get; set; }
    public int? NoOfRooms { get; set; }
    public string? BizType { get; set; } 
    public string? ServicePackageName { get; set; } 
    public DateTime? CreatedAt { get; set; } 
    public string? UserType { get; set; } 

}
