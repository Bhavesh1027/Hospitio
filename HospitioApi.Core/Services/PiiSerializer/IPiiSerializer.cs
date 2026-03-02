namespace HospitioApi.Core.Services.PiiSerializer;

public interface IPiiSerializer
{
    PiiSerializerHelper<T> Init<T>(T originalObject, bool useUnsafeRelaxedJsonEscaping);
}

