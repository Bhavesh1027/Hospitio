namespace HospitioApi.Core.Services.Chat.Models.Chat;

public class GetReceiverUserWP
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? UserType { get; set; }
    public string? Phonenumber { get; set; }
    public string? AppId { get; set; }
    public string? AppPrivatKey { get; set; }
}
