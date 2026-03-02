using HospitioApi.Shared;

namespace HospitioApi.Core.HandleQaCategories.Queries.GetQaCategories;

public class GetQaCategoriesOut : BaseResponseOut
{
    public GetQaCategoriesOut(string message, List<QaCategoriesOut> qaCategories) : base(message)
    {
        QaCategories = qaCategories;
    }
    public List<QaCategoriesOut> QaCategories { get; set; } = new();

}
public class QaCategoriesOut
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;

}
