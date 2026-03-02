using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTicketCategories.Commands.DeleteTicketCategory;

public class DeleteTicketCategoryOut : BaseResponseOut
{
    public DeleteTicketCategoryOut(string message, DeletedtTicketCategoryOut deletedtTicketCategoryOut) : base(message)
    {
        DeletedtTicketCategoryOut = deletedtTicketCategoryOut;
    }
    public DeletedtTicketCategoryOut DeletedtTicketCategoryOut { get; set; } = new();
}
public class DeletedtTicketCategoryOut
{
    public int Id { get; set; }
}
