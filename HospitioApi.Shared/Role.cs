namespace HospitioApi.Shared;

public static class Role
{
    public const string SuperAdmin = "SuperAdmin";
    public const string CEO = "CEO";
    public const string Manager = "Manager";
    public const string GroupLeader = "GroupLeader";
    public const string Staff = "Staff";

    public static List<string> AdministratorRoles() => new()
    {
        SuperAdmin
    };
}
