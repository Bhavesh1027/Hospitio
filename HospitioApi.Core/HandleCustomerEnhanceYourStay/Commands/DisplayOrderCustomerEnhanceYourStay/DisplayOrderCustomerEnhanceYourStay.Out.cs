using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.DisplayOrderCustomerEnhanceYourStay
{
    public class DisplayOrderCustomerEnhanceYourStayOut : BaseResponseOut
    {
        public DisplayOrderCustomerEnhanceYourStayOut(string message) : base(message)
        {
            
        }
    }
    //public class CustomerEnhanceYourStayJsonOut
    //{
    //    public int CustomerId { get; set; }
    //    public int? CustomerGuestAppBuilderId { get; set; }

    //    public int? DisplayOrder { get; set; }

    //    public CategoryName categoryNames { get; set; } = new();
    //}
    public class CustomerEnhanceYourStayJsonOut
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public int? CategoryDisplayOrder { get; set; }
        public bool? IsPublish { get; set; }
        public int? DisplayOrder { get; set; }
        public List<CategoryItem> categoryItems { get; set; } = new();

    }
    public class CategoryItem
    {
        public int CategoryItemId { get; set; }
        public bool? IsActive { get; set; }
        public int? ItemDisplayOrder { get; set; }

    }
}
