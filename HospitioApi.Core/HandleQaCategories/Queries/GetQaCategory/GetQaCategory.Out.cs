using HospitioApi.Shared;

namespace HospitioApi.Core.HandleQaCategories.Queries.GetQaCategory;

public class GetQaCategoryOut : BaseResponseOut
{
    public GetQaCategoryOut(string message, QaCategoryOut qaCategoryOut) : base(message)
    {
        QaCategoryOut = qaCategoryOut;
    }
    public QaCategoryOut QaCategoryOut { get; set; }
}
public class QaCategoryOut
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}
