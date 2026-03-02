using HospitioApi.Shared;

namespace HospitioApi.Core.HandleMusement.Commands.MusementNoPaymentFlow;

public class MusementNoPaymentFlowOut : BaseResponseOut
{
    public MusementNoPaymentFlowOut(string message, string musementNoPaymentFlowOutResponseOut) : base(message)
    {
        musementNoPaymentFlowOutResponse = musementNoPaymentFlowOutResponseOut;
    }
    public string musementNoPaymentFlowOutResponse { get;set; }
}

public class Root
{
    public Customer? customer { get; set; }
    public string? date { get; set; }
    public DiscountAmount? discount_amount { get; set; }
    public string? extra_data { get; set; }
    public string? identifier { get; set; }
    public List<Item>? items { get; set; }
    public string? market { get; set; }
    public string? status { get; set; }
    public DiscountAmount? total_price { get; set; }
    public string? trustpilot_url { get; set; }
    public string? uuid { get; set; }
    public DiscountAmount? total_retail_price_in_order_currency { get; set; }
    public DiscountAmount? total_supplier_original_retail_price_in_supplier_currency { get; set; }
    public DiscountAmount? total_supplier_price_in_supplier_currency { get; set; }
    public Affiliate? affiliate { get; set; }
    public string? affiliate_channel { get; set; }
    public List<PromoCode>? promo_codes { get; set; }
    public string? source { get; set; }
}

public class Customer
{
    public string? email { get; set; }
    public string? events_related_newsletter { get; set; }
    public Dictionary<string, ExtraCustomerData>? extra_customer_data { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
    public string? musement_newsletter { get; set; }
    public string? thirdparty_newsletter { get; set; }
}
public class DiscountAmount
{
    public string? currency { get; set; }
    public string? formatted_value { get; set; }
    public string? formatted_iso_value { get; set; }
    public int value { get; set; }
}

public class Item
{
    public int quantity { get; set; }
    public DiscountAmount? b2b_price { get; set; }
    public string? cancellation_additional_info { get; set; }
    public string? cancellation_reason { get; set; }
    public bool error_status { get; set; }
    public List<ExtraCustomerData>? extra_customer_data { get; set; }
    public bool is_gift_redeem { get; set; }
    public List<ParticipantsInfo>? participants_info { get; set; }
    public Product? product { get; set; }
    public string? status { get; set; }
    public string? transactionCode { get; set; }
    public string? uuid { get; set; }
    public List<Voucher>? vouchers { get; set; }
    public DiscountAmount? retail_price_in_order_currency { get; set; }
    public DiscountAmount? total_retail_price_in_order_currency { get; set; }
    public DiscountAmount? original_retail_price_in_supplier_currency { get; set; }
    public DiscountAmount? total_original_retail_price_in_supplier_currency { get; set; }
}

public class ExtraCustomerData
{
    public long? name { get; set; }
}
public class ParticipantsInfo
{
    public string? salutation { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
    public string? date_of_birth { get; set; }
    public string? passport { get; set; }
    public string? email { get; set; }
    public string? passport_expiry_date { get; set; }
    public string? nationality { get; set; }
    public string? medicalNotes { get; set; }
    public string? address { get; set; }
    public string? fan_card { get; set; }
    public int weight { get; set; }
    public string? phoneNumber { get; set; }
}

public class Product
{
    public string? activity_uuid { get; set; }
    public string? api_url { get; set; }
    public string? cover_image_url { get; set; }
    public string? date { get; set; }
    public DiscountAmount? discount_amount { get; set; }
    public string? id { get; set; }
    public Language? language { get; set; }
    public string? max_confirmation_time { get; set; }
    public OriginalRetailPrice? original_retail_price { get; set; }
    public OriginalRetailPrice? original_retail_price_without_service_fee { get; set; }
    public DiscountAmount? retail_price { get; set; }
    public DiscountAmount? retail_price_without_service_fee { get; set; }
    public DiscountAmount? service_fee { get; set; }
    public string? title { get; set; }
    public string? type { get; set; }
    public string? url { get; set; }
}

public class Language
{
    public string? code { get; set; }
    public string? name { get; set; }
}



public class OriginalRetailPrice
{
    public string? currency { get; set; }
    public string? formattedValue { get; set; }
    public string? formattedIsoValue { get; set; }
    public int value { get; set; }
}




public class Voucher
{
    public string? url { get; set; }
}


public class PromoCode
{
    public string? code { get; set; }
    public bool active { get; set; }
    public bool percentage { get; set; }
    public int discount { get; set; }
    public int max_usage { get; set; }
    public string? valid_from { get; set; }
    public string? valid_until { get; set; }
    public int? minimum_amount { get; set; }
}

public class Affiliate
{
    public string? uuid { get; set; }
    public string? email { get; set; }
    public string? first_name { get; set; }
    public string? last_name { get; set; }
    public string? code { get; set; }
    public string? name { get; set; }
    public string? logoUrl { get; set; }
    public string? secondary_logo_url { get; set; }
    public string? header { get; set; }
    public string? customer_care_phone_number { get; set; }
    public string? customer_care_email { get; set; }
    public bool whitelabel { get; set; }
    public bool show_cobranded_header { get; set; }
    public bool show_cobranded_voucher { get; set; }
    public bool show_cobranded_item_confirmation_email { get; set; }
    public bool setup_cookie_after_first_visit { get; set; }
    public List<Translation>? translations { get; set; }
}

public class Translation
{
    public string? locale { get; set; }
}
