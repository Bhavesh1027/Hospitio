using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Data.Models;

public partial class CustomerUsersPermission : Auditable
{

    public int? CustomerPermissionId { get; set; }
    public int? CustomerUserId { get; set; }
    public bool? IsView { get; set; }
    public bool? IsEdit { get; set; }
    public bool? IsUpload { get; set; }
    public bool? IsReply { get; set; }
    public bool? IsDownload { get; set; }


    public virtual CustomerPermission? CustomerPermission { get; set; }
    public virtual CustomerUser? CustomerUser { get; set; }
}
