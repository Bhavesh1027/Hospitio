using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTicketCategories.Commands.CreateTicketCategory;

public class CreateTicketCategoryOut : BaseResponseOut
{
    public CreateTicketCategoryOut(string message, CreatedTicketCategoryOut createdTicketCategoryOut) : base(message)
    {
        CreatedTicketCategoryOut = createdTicketCategoryOut;
    }
    public CreatedTicketCategoryOut CreatedTicketCategoryOut { get; set; }
}
public class CreatedTicketCategoryOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
