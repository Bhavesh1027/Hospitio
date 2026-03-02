namespace HospitioApi.Core.HandlePaymentServiceDefinitions.Commands.SyncPaymentServiceDefinitions;

public class SyncPaymentServiceDefinitionsIn
{
    public Gr4vyConnectionDefinitionModel Item { get; set; } = new Gr4vyConnectionDefinitionModel();
}
public class Gr4vyConnectionDefinitionModel
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public string? Display_name { get; set; }
    public string? Method { get; set; }
    public List<FieldModel> Fields { get; set; } = new();
    public List<string> Supported_currencies { get; set; } = new();
    public List<string> Supported_countries { get; set; } = new();
    public string? Mode { get; set; }
    public string? Icon_url { get; set; }
    public SupportedFeaturesModel Supported_features { get; set; } = new SupportedFeaturesModel();
    public ConfigurationModel Configuration { get; set; } = new ConfigurationModel();
}

public class FieldModel
{
    public string? Key { get; set; }
    public string? Display_name { get; set; }
    public bool Required { get; set; }
    public string? Format { get; set; }
    public bool Secret { get; set; }
}

public class SupportedFeaturesModel
{
    public bool Delayed_capture { get; set; }
    public bool Network_tokens { get; set; }
    public bool Network_tokens_default { get; set; }
    public bool Network_tokens_toggle { get; set; }
    public bool Open_loop { get; set; }
    public bool Open_loop_toggle { get; set; }
    public bool Partial_capture { get; set; }
    public bool Partial_refunds { get; set; }
    public bool Payment_method_tokenization { get; set; }
    public bool Payment_method_tokenization_toggle { get; set; }
    public bool Refunds { get; set; }
    public bool Requires_webhook_setup { get; set; }
    public bool Three_d_secure_hosted { get; set; }
    public bool Three_d_secure_pass_through { get; set; }
    public bool Verify_credentials { get; set; }
    public bool Void { get; set; }
    public bool Zero_auth { get; set; }
}

public class ConfigurationModel
{
    public string? Approval_ui_target { get; set; }
    public string? Approval_ui_height { get; set; }
    public string? Approval_ui_width { get; set; }
    public int Cart_items_limit { get; set; }
    public bool Cart_items_required { get; set; }
    public bool Cart_items_should_match_amount { get; set; }
}
