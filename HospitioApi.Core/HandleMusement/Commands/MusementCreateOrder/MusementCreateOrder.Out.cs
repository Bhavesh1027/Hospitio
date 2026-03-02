using HospitioApi.Shared;
using System.Collections.Generic;

namespace HospitioApi.Core.HandleMusement.Commands.MusementCreateOrder;

public class MusementCreateOrderOut : BaseResponseOut
{
    public MusementCreateOrderOut(string message, string musementCreateOrderResponseOut) : base(message)
    {
        musementCreateOrderResponse = musementCreateOrderResponseOut;
    }
    public string musementCreateOrderResponse { get; set; }
}
public class order
{
    public string? identifier { get; set; }
    public string uuid { get; set; }
    //public DateTime date { get; set; }
    public string? status { get; set; }
    public string? trustpilot_url { get; set; }
    public customer? customer { get; set; }
    public List<orderitem>? items { get; set; }
    public totalprice? total_price { get; set; }
    public discountamount? discount_amount { get; set; }
    public string? extra_data { get; set; }
    public string? market { get; set; }
    public bool? is_agency { get; set; }
    public bool? is_paid { get; set; }
}

public class customer
{
    public int? id { get; set; }
    //public DateTime created_at { get; set; }
    public string? email { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
}

public class orderitem
{
    public string? uuid { get; set; }
    public string? cart_item_uuid { get; set; }
    public string? transaction_code { get; set; }
    public product? product { get; set; }
    public int? quantity { get; set; }
    public totalprice? retail_price_in_order_currency { get; set; }
    public totalprice? total_retail_price_in_order_currency { get; set; }
    public string? status { get; set; }
    public List<voucher>? vouchers { get; set; }
   // public List<string>? extra_customer_data { get; set; }
    public bool? is_error_status { get; set; }
}

public class product
{
    public string? type { get; set; }
    public string? max_confirmation_time { get; set; }
    public pricetag? price_tag { get; set; }
    //public DateTime date { get; set; }
    public string? id { get; set; }
    public string? title { get; set; }
    public string? activity_uuid { get; set; }
    public string? api_url { get; set; }
    public string? url { get; set; }
    public string? cover_image_url { get; set; }
    public totalprice? original_retail_price { get; set; }
    public totalprice? original_retail_price_without_service_fee { get; set; }
    public totalprice? retail_price { get; set; }
    public totalprice? retail_price_without_service_fee { get; set; }
    public discountamount? discount_amount { get; set; }
    public servicefee? service_fee { get; set; }
    public string? meeting_point { get; set; }
    public string? meeting_point_markdown { get; set; }
    public string? meeting_point_html { get; set; }
}

public class pricetag
{
    public string? price_feature { get; set; }
    public string? ticket_holder { get; set; }
    public string? price_feature_code { get; set; }
    public string? ticket_holder_code { get; set; }
}

public class totalprice
{
    public string? currency { get; set; }
    public decimal? value { get; set; }
    public string? formatted_value { get; set; }
    public string? formatted_iso_value { get; set; }
}

public class discountamount
{
    public string? currency { get; set; }
    public decimal? value { get; set; }
    public string? formatted_value { get; set; }
    public string? formatted_iso_value { get; set; }
}

public class voucher
{
    public string? uuid { get; set; }
    public string? code { get; set; }
    //public DateTime createdAt { get; set; }
    public discountamount? discountAmount { get; set; }
    public string? status { get; set; }
    public string? type { get; set; }
}

public class servicefee
{
    public string? currency { get; set; }
    public decimal? value { get; set; }
    public string? formatted_value { get; set; }
    public string? formatted_iso_value { get; set; }
}