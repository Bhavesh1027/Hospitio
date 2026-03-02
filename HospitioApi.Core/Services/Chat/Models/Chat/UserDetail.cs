using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.Services.Chat.Models.Chat
{
    public class UserDetail
    {
        public string BusinessName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string PhoneCountry { get; set; }
        public string PhoneNumber { get; set; }
        public string IncomingTranslationLangage { get; set; }
        public string? NoOfRooms { get; set; }
        public string BizType { get; set; }
        public string ServicePackageName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserType { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DeActivated { get; set; }
        public string Status { get; set; }
        public DateTime? CheckinDate { get; set; }
        public DateTime? CheckoutDate { get; set; }
        public string? BlePinCode { get; set; }
    }
}
