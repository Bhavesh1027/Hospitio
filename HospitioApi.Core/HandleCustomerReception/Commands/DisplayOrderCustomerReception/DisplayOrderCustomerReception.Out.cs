using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomerReception.Commands.DisplayCustomerReception;

public class DisplayOrderCustomerReceptionOut : BaseResponseOut
{
    public DisplayOrderCustomerReceptionOut(string message) : base(message)
    {
    }
}
public class DisplayOrderCustomerReceptionJsonOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public List<CustomerReceptionItem> CustomerReceptionItems { get; set; } = new();
}
public class CustomerReceptionItem
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? Name { get; set; }
    public bool? ItemsMonth { get; set; }
    public bool? ItemsDay { get; set; }
    public bool? ItemsMinute { get; set; }
    public bool? ItemsHour { get; set; }
    public bool? QuantityBar { get; set; }
    public bool? ItemLocation { get; set; }
    public bool? Comment { get; set; }
    public bool? IsPriceEnable { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsDeleted { get; set; }
    public bool? IsActive { get; set; }
}
