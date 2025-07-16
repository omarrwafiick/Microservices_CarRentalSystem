using Common.Models;

namespace ChatSupportApi.Models
{
    public class ChatMessage : BaseEntity
    {
        private ChatMessage()
        { 
        }
        private ChatMessage(string message, string connectionID, Guid chatId)
        {  
            Message = message;
            ConnectionID = connectionID;
            ChatId = chatId;
            SentAt = DateTime.UtcNow;
        } 
        public string Message { get; set; }
        public string ConnectionID { get; set; }
        public DateTime SentAt { get; set; } 
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
        public static ChatMessage Factory(string message, string connectionID, Guid chatId) => new ChatMessage(message, connectionID, chatId);
    }
}
