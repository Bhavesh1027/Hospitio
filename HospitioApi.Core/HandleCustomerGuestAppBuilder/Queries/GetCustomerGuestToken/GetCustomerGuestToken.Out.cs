using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestToken;

public class GetCustomerGuestTokenOut : BaseResponseOut
{
    public GetCustomerGuestTokenOut(string message , GetCustomerGuestTokenClass getCustomerTokenOut) : base(message)
    {
        getCustomerGuestTokenClass = getCustomerTokenOut;
    }
    public GetCustomerGuestTokenClass getCustomerGuestTokenClass {  get; set; }
}

public class GetCustomerGuestTokenClass
{
    public string? CustomerGuestToken { get; set; }
}