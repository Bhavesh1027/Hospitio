namespace HospitioApi.Core.HandleTicketCategories.Commands.DeleteTicketCategory;

public class DeleteTicketCategoryIn
{
    public int Id { get; set; }
    public DateTime? DeletedAt { get; set; }
}
