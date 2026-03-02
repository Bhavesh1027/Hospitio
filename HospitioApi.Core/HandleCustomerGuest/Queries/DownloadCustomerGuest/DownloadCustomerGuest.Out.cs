using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.DownloadCustomerGuest;

public class DownloadCustomerGuestOut : BaseResponseOut
{
    public DownloadCustomerGuestOut(string message, string customerGuestsOut) : base(message)
    {
        CustomerGuestsOut = customerGuestsOut;
    }
    public string CustomerGuestsOut { get; set; }
}   
public class DownloadCustomerGuestsOut
{
    public byte[]? ExcelData { get; set; }
}

public class CustomerGuestDownloadOut
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? RoomNumber { get; set; }
    public DateTime? CheckinDate { get; set; }
    public DateTime? CheckoutDate { get; set; }
    public string? GuestToken { get; set; }
}