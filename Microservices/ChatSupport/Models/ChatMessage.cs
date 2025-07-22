using Common.Models;

namespace ChatSupportApi.Models
{
    public class ChatMessage : BaseEntity
    {
        private ChatMessage()
        {
        }
        public static ChatMessage Factory(string message, string connectionID, int chatId) =>
            new ChatMessage
            {
                Message = message,
                ConnectionID = connectionID,
                ChatId = chatId,
                SentAt = DateTime.UtcNow
            };

        public string Message { get; set; }
        public string ConnectionID { get; set; }
        public DateTime SentAt { get; set; } 
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
      
    }
}
