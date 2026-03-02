using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Data.Models;

public partial class CustomerPropertyExtraDetails : Auditable
{
    public int? CustomerPropertyExtraId { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public bool? IsPublish { get; set; }
    public string? JsonData { get; set; }
    public virtual CustomerPropertyExtra? CustomerPropertyExtra { get; set; } 
}
