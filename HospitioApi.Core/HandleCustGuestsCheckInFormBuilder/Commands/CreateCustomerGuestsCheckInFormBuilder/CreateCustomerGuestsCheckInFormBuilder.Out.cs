using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Commands.CreateCustomerGuestsCheckInFormBuilder;
public class CreateCustomerGuestsCheckInFormBuilderOut : BaseResponseOut
{
    public CreateCustomerGuestsCheckInFormBuilderOut(string message, CreatedCustomerGuestsCheckInFormBuilderOut createdCustomerGuestsCheckInFormBuilderOut) : base(message)
    {
        CreatedCustomerGuestsCheckInFormBuilderOut = createdCustomerGuestsCheckInFormBuilderOut;
    }
    public CreatedCustomerGuestsCheckInFormBuilderOut CreatedCustomerGuestsCheckInFormBuilderOut { get; set; }

}
public class CreatedCustomerGuestsCheckInFormBuilderOut
{
    public int Id { get; set; }
}

public class GuestsCheckInFormBuilderJsonOut
{
    public string? short_name { get;set; }
    public string? name { get;set; }
    public string? scope { get;set; }
    public string? start_url { get; set; }
    public string? display { get; set; }
    public List<dynamic>? icons { get;set; }
    public string? theme_color { get; set; }
    public string? background_color { get; set; }

}

public class GuestsCheckInFormBuilderJsonIconOut
{
    public string? src { get; set; }
    public string? type { get; set; }
    public string? sizes { get; set; }
}