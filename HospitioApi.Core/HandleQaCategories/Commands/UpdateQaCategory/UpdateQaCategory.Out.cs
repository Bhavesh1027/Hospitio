using HospitioApi.Shared;

namespace HospitioApi.Core.HandleQaCategories.Commands.UpdateQaCategory;

public class UpdateQaCategoryOut : BaseResponseOut
{
    public UpdateQaCategoryOut(string message, UpdatedQaCategoryOut updatedQaCategoryOut) : base(message)
    {
        UpdatedQaCategoryOut = updatedQaCategoryOut;
    }
    public UpdatedQaCategoryOut UpdatedQaCategoryOut { get; set; } = new();
}

public class UpdatedQaCategoryOut
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
