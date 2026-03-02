using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Data.Models;

public partial class CustomerPermission : Auditable
{
    public CustomerPermission()
    {
        CustomerUsersPermissions = new HashSet<CustomerUsersPermission>();
    }
    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(50)]

    public string? NormalizedName { get; set; }
    public bool? IsView { get; set; }
    public bool? IsEdit { get; set; }
    public bool? IsUpload { get; set; }
    public bool? IsReply { get; set; }
    public bool? IsDownload { get; set; }

    public virtual ICollection<CustomerUsersPermission> CustomerUsersPermissions { get; set; }
}
