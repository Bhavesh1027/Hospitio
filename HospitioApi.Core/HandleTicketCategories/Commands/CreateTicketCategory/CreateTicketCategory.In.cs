namespace HospitioApi.Core.HandleTicketCategories.Commands.CreateTicketCategory;

public class CreateTicketCategoryIn
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int CreatedBy { get; set; }
}
