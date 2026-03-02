using HospitioApi.Shared;

namespace HospitioApi.Core.HandleUserLevels.Queries.GetUserLevels;

public class GetUserLevelsOut : BaseResponseOut
{
    public GetUserLevelsOut(string message, List<UserLevelsOut> userLevelsOut) : base(message)
    {
        UserLevelsOut = userLevelsOut;
    }
    public List<UserLevelsOut> UserLevelsOut { get; set; } = new();
}
public class UserLevelsOut
{
    public int Id { get; set; }
    public string? LevelName { get; set; }
    public string? NormalizedLevelName { get; set; }
}
