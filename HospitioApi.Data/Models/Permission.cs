using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class Permission : Auditable
{
    public Permission()
    {
        UsersPermissions = new HashSet<UsersPermission>();
    }
    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(50)]

    public string? NormalizedName { get; set; }
    public bool? IsView { get; set; }
    public bool? IsEdit { get; set; }
    public bool? IsReply { get; set; }
    public bool? IsUpload { get; set; }
    public bool? IsSend { get; set; }

    public virtual ICollection<UsersPermission> UsersPermissions { get; set; }
}