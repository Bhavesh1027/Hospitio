using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.Services.Chat.Models.Chat
{
    public class ChatList
    {
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public string LastMessage { get; set; }
        public string LastMessageType { get; set; }
        public DateTime LastMessageTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsActive { get; set; }
        public int TotalUnReadCount { get; set; }
        public string UserType { get; set; }
        public string PhoneNumber { get; set; }

    }
    public class ChatListResponse
    {
        public int ActiveUsers { get; set; }
        public List<ChatList> ChatList { get; set; }
    }
}
