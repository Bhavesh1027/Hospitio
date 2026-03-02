namespace HospitioApi.Core.Services.Chat.Models.Tickets
{
    public class TicketDetail
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string Email { get; set; }
        public string Priority { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime CloseDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalUnReadCount { get; set; }
    }
}
