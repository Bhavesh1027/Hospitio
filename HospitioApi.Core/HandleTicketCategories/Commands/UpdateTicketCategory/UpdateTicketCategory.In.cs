namespace HospitioApi.Core.HandleTicketCategories.Commands.UpdateTicketCategory;

public class UpdateTicketCategoryIn
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? UpdateAt { get; set; }
}
