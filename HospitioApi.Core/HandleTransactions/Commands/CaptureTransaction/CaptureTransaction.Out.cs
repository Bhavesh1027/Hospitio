using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTransactions.Commands.CaptureTransaction;

public class CaptureTransactionOut : BaseResponseOut
{
    public CaptureTransactionOut(string message, CapturedTransactionOut captured) : base(message)
    {
        capturedTransactionOut = captured;
    }
    public CapturedTransactionOut capturedTransactionOut { get; set; }
}
public class CapturedTransactionOut
{
    public string? type { get; set; }
    public string? id { get; set; }
    public string? merchant_account_id { get; set; }
    public string? status { get; set; }
    public string? intent { get; set; }
    public int? amount { get; set; }
    public int? captured_amount { get; set; }
    public int? refunded_amount { get; set; }
    public string? currency { get; set; }
    public string? country { get; set; }
    public PaymentMethod? payment_method { get; set; }
    public Buyer? buyer { get; set; }
    public DateTime? created_at { get; set; }
    public string? external_identifier { get; set; }
    public string? updated_at { get; set; }
    public PaymentService? payment_service { get; set; }
    public bool? pending_review { get; set; }
    public bool? merchant_initiated { get; set; }
    public string? payment_source { get; set; }
    public bool? is_subsequent_payment { get; set; }
    public StatementDescriptor? statement_descriptor { get; set; }
    public List<CartItem>? cart_items { get; set; }
    public string? scheme_transaction_id { get; set; }
    public string? raw_response_code { get; set; }
    public string? raw_response_description { get; set; }
    public string? avs_response_code { get; set; }
    public string? cvv_response_code { get; set; }
    public string? method { get; set; }
    public string? payment_service_transaction_id { get; set; }
    public Dictionary<string, string>? metadata { get; set; }
    public ShippingDetails? shipping_details { get; set; }
    public ThreeDSecure? three_d_secure { get; set; }
    public DateTime? authorized_at { get; set; }
    public DateTime? captured_at { get; set; }
    public DateTime? voided_at { get; set; }
    public string? checkout_session_id { get; set; }
}
public class PaymentMethod
{
    public string? type { get; set; }
    public string? id { get; set; }
    public string? method { get; set; }
    public string? external_identifier { get; set; }
    public string? label { get; set; }
    public string? scheme { get; set; }
    public string? expiration_date { get; set; }
    public string? approval_target { get; set; }
    public string? approval_url { get; set; }
    public string? currency { get; set; }
    public string? country { get; set; }
    public Details? details { get; set; }
}
public class Buyer
{
    public string? type { get; set; }
    public string? id { get; set; }
    public string? external_identifier { get; set; }
    public string? display_name { get; set; }
    public BillingDetails? billing_details { get; set; }
}
public class BillingDetails
{
    public string? type { get; set; }
    public string? first_name { get; set; }
    public string? last_name { get; set; }
    public string? email_address { get; set; }
    public string? phone_number { get; set; }
    public Address? address { get; set; }
    public TaxId? tax_id { get; set; }
}
public class PaymentService
{
    public string? type { get; set; }
    public string? id { get; set; }
    public string? payment_service_definition_id { get; set; }
    public string? method { get; set; }
    public string? display_name { get; set; }
}
public class StatementDescriptor
{
    public string? name { get; set; }
    public string? description { get; set; }
    public string? city { get; set; }
    public string? phone_number { get; set; }
    public string? url { get; set; }
}
public class CartItem
{
    public string? name { get; set; }
    public int? quantity { get; set; }
    public int? unit_amount { get; set; }
    public object? discount_amount { get; set; }
    public object? tax_amount { get; set; }
    public string? external_identifier { get; set; }
    public string? sku { get; set; }
    public string? product_url { get; set; }
    public string? image_url { get; set; }
    public List<string>? categories { get; set; }
    public string? product_type { get; set; }
}
public class ShippingDetails
{
    public string? type { get; set; }
    public string? id { get; set; }
    public string? buyer_id { get; set; }
    public string? first_name { get; set; }
    public string? last_name { get; set; }
    public string? email_address { get; set; }
    public string? phone_number { get; set; }
    public Address? address { get; set; }
}
public class ThreeDSecure
{
    public string? version { get; set; }
    public string? status { get; set; }
    public string? method { get; set; }
    public ErrorData? error_data { get; set; }
    public object? response_data { get; set; }
}
public class Details
{
    public string? card_type { get; set; }
    public string? bin { get; set; }
}
public class Address
{
    public string? city { get; set; }
    public string? country { get; set; }
    public string? postal_code { get; set; }
    public string? state { get; set; }
    public string? state_code { get; set; }
    public string? house_number_or_name { get; set; }
    public string? line1 { get; set; }
    public string? line2 { get; set; }
    public string? organization { get; set; }
}
public class TaxId
{
    public string? value { get; set; }
    public string? kind { get; set; }
}
public class ErrorData
{
    public string? description { get; set; }
    public string? detail { get; set; }
    public string? code { get; set; }
    public string? component { get; set; }
}