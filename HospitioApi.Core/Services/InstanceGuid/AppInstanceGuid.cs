namespace HospitioApi.Core.Services.InstanceGuid;

public class AppInstanceGuid : IAppInstanceGuid
{
    private readonly string _guid;

    public AppInstanceGuid()
    {
        _guid = Guid.NewGuid().ToString();
    }

    public AppInstanceGuid(string guid)
    {
        _guid = guid;
    }

    public string Value { get { return _guid; } }
}
