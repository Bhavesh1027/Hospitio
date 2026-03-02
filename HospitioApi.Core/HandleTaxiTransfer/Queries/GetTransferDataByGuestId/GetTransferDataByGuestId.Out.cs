using HospitioApi.Shared;

namespace HospitioApi.Core.HandleTaxiTransfer.Queries.GetTransferDataByGuestId;

public class GetTransferDataByGuestIdOut : BaseResponseOut
{
    public GetTransferDataByGuestIdOut(string message, List<TransferResponse> response) : base(message)
    {
        TransferResponse = response;
    }
    public List<TransferResponse> TransferResponse { get; set; }
}
public class TransferResponse
{
    public int Id { get; set; }
    public int? GuestId { get; set; }
    public string? TransferId {  get; set; }
    public string? TransferStatus { get; set; }
    public int? RefundAmount {  get; set; }
    public string? GRPaymentId { get; set; }
    public string? GRPaymentDetails { get;  set; }
    public string? TransferJson { get;set; }
    public string? RefundId { get; set; }
    public string? RefundStatus { get;set; }
    public decimal? FareAmount { get; set; }
    public decimal? HospitioFareAmount { get; set; }
    public TransferModel? transferModel { get; set; }
    public string? ExtraDetailsJson { get; set; }
    public ExtraDetails? ExtraDetails { get; set; }
}

public class TransferModel
{
    public Data? data { get; set; }
}
public class Data
{
    public string? id { get; set; }
    public string? type { get; set; }
    public Attributes? attributes { get; set; }
}
public class Attributes
{
    public string? quote_id { get; set; }
    public string? passenger_booking_reference { get; set; }
    public string? transfer_status { get; set; }
    public string? traveler_no_show_up {  get; set; }
    public string? cancellation_reason { get; set; }
    public string? traveler_cancellation_reason { get; set; }
    public DateTime? cancelled_at { get; set; }
    public DateTime? pickup_date_time { get; set; }
    public DateTime? booked_date_time { get; set; }
    public Location? from_location { get; set; }
    public Location? to_location { get; set; }
    public Passenger? passenger { get; set; }
    public Agent? agent { get; set; }
    public ServiceInfo? service_info { get; set; }
    public Fare? fare { get; set; }
    public string? driver {  get; set; }
    public string? additional_notes { get; set; }
    public List<transferevents>? transfer_events {  get; set; }
}

public class Location
{
    public string? type { get; set; }
    public string? description { get; set; }
    public double? lat { get; set; }
    public double? lng { get; set; }
}

public class Passenger
{
    public int? count { get; set; }
    public string? name { get; set; }
    public string? email { get; set; }
    public string? mobile { get; set; }
}

public class Agent
{
    public string? name { get; set; }
    public string? email { get; set; }
    public string? phone { get; set; }
}

public class ServiceInfo
{
    public string? type { get; set; }
    public string? vehicle_type { get; set; }
    public int? max_pax { get; set; }
    public int? max_lug { get; set; }
    public string? photo_url { get; set; }
    public List<string>? photo_urls { get; set; }
    public string? description { get; set; }
    public Supplier? supplier { get; set; }
    public PassengerReviews? passenger_reviews { get; set; }
}

public class Supplier
{
    public string? id { get; set; }
    public string? name { get; set; }
    public string? photo_url { get; set; }
    public string? description { get; set; }
}

public class PassengerReviews
{
    public int? count { get; set; }
    public double? average_rating { get; set; }
}

public class Fare
{
    public double? price { get; set; }
    public string? currency_code { get; set; }
    public string? type { get; set; }
    public string? refund_cancellation_policy { get; set; }
    public List<Policy>? refund_policies { get; set; }
    public Refund? refund { get; set; }
}

public class Refund
{
    public Policy? policy { get; set; }
    public double? amount { get; set; }
}

public class Policy
{
    public hoursfromoperation? hours_from_operation { get; set; }
    public string? refund_percentage { get; set; }
}
public class hoursfromoperation
{
    public  string? min {  get; set; }
    public string? max { get; set; }
}

public class transferevents
{
    public string? name { get; set; }
    public DateTime? timestamp {  get; set; }
}

public class ExtraDetails
{
    public string? Luggage { get; set; }
    public string? passenger { get; set; }
    public string? FromLocation { get; set; }
    public string? ToLocation { get; set; }
    public DateTime? RefundDate { get; set; }
}


