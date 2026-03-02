namespace HospitioApi.Core.HandleCustomerUsers.Commands.UpdateMuteStatus;

public class UpdateMuteStatusIn
{
	public int? CustomerUserId { get; set; }
	public bool IsMuted { get; set; }
}
