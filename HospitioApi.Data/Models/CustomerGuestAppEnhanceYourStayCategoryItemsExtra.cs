using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerGuestAppEnhanceYourStayCategoryItemsExtra : Auditable
{
    public int? CustomerGuestAppEnhanceYourStayItemId { get; set; }
    /// <summary>
    /// 1: Text,    2: MultiSelect,    3: Checkbox,    4: DropDwon,    5: Date,    6: Time
    /// </summary>
    public byte? QueType { get; set; }
    /// <summary>
    /// For type TextDate, Time, values will be null
    ///     For type Multi select, Dropdown and Checkbox values will be like 
    ///     
    ///       {question: string, hasQuantityBar: false, required: true}
    ///     
    /// </summary>
    public string? Questions { get; set; }
    /// <summary>
    /// For type TextDate, Time, values will be null
    ///     For type Multi select, Dropdown and Checkbox values will be like 
    ///     [
    ///       {option: &apos;option value 1&apos;, hasQuantityBar: false, required: true},
    ///       {option: &apos;option value 2&apos;, hasQuantityBar: false, required: true}
    ///     ]
    /// </summary>
    public string? OptionValues { get; set; }
    public string? JsonData { get; set; }
    public bool? IsPublish { get; set; }
    public virtual CustomerGuestAppEnhanceYourStayItem? CustomerGuestAppEnhanceYourStayItem { get; set; }
}
