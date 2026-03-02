using System.Text.Json;

namespace HospitioApi.Core.HandlePaymentServiceDefinitions.Commands.SyncPaymentServiceDefinitions;

public class ObjectStringify
{
    public static string ConvertObjectToString<T>(T obj)
    {
        return JsonSerializer.Serialize(obj);
    }
}
