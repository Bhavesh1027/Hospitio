namespace HospitioApi.Data.Models;

public partial class UsersPermission : Auditable
{

    public int? PermissionId { get; set; }
    public int? UserId { get; set; }
    public bool? IsView { get; set; }
    public bool? IsEdit { get; set; }
    public bool? IsUpload { get; set; }
    public bool? IsReply { get; set; }
    public bool? IsSend { get; set; }


    public virtual Permission? Permission { get; set; }
    public virtual User? User { get; set; }
}
