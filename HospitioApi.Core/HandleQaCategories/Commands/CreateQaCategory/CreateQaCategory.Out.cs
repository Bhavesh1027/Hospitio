using HospitioApi.Shared;

namespace HospitioApi.Core.HandleQaCategories.Commands.CreateQaCategory;

public class CreateQaCategoryOut : BaseResponseOut
{
    public CreateQaCategoryOut(string message, CreatedQaCategoryOut createdQaCategoryOut) : base(message)
    {
        CreatedQaCategoryOut = createdQaCategoryOut;
    }
    public CreatedQaCategoryOut CreatedQaCategoryOut { get; set; }

}
public class CreatedQaCategoryOut
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

