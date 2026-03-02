using HospitioApi.Shared;

namespace HospitioApi.Core.HandleQaCategories.Commands.DeleteQaCategory;

public class DeleteQaCategoryOut : BaseResponseOut
{
    public DeleteQaCategoryOut(string message, DeletedQaCategory deletedQaCategory) : base(message)
    {
        DeletedQaCategory = deletedQaCategory;
    }
    public DeletedQaCategory DeletedQaCategory { get; set; } = new();
}
public class DeletedQaCategory
{
    public long QaCategoryId { get; set; }
}
