namespace HospitioApi.Core.HandleConnectionDefinations.Commands.SyncConnectionDefinations;

public class SyncConnectionDefinationsIn
{
    public List<ConnectionDefinition> Items { get; set; } = new List<ConnectionDefinition>();
}
public class ConnectionDefinition
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public int Count { get; set; }
    public string? Icon_Url { get; set; }
    public string? Name { get; set; }
    public string? Group { get; set; }
    public string? Category { get; set; }
    public string? Provider { get; set; }
}