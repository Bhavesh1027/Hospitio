using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTicketCategories.Commands.UpdateTicketCategory;

public class UpdateTicketCategoryOut : BaseResponseOut
{
    public UpdateTicketCategoryOut(string message, UpdatedTicketCategoryOut updatedTicketCategoryOut) : base(message)
    {
        UpdatedTicketCategoryOut = updatedTicketCategoryOut;
    }
    public UpdatedTicketCategoryOut UpdatedTicketCategoryOut { get; set; }
}
public class UpdatedTicketCategoryOut
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
