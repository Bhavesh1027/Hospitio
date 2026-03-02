using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.EditCustomerGuest
{
    public class EditCustomerGuestIn
    {
        public int Id { get; set; }
        public int? CustomerReservationId { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? PhoneCountry { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }
        public string? Pin { get; set; }
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public string? City { get; set; }
        public string? Postalcode { get; set; }
        public DateTime? CheckinDate { get; set; }
        public DateTime? CheckoutDate { get; set; }
        public string? BlePinCode { get; set; }
    }
}
