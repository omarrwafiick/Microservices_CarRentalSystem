using Common.Interfaces;

namespace ChatSupportApi.Models
{
    public class ChatMessage : IBaseEntity
    {
        private ChatMessage()
        { 
        }
        private ChatMessage(string message, string connectionID, Guid chatId)
        { 
            Id = Guid.NewGuid();
            Message = message;
            ConnectionID = connectionID;
            ChatId = chatId;
            SentAt = DateTime.UtcNow;
        }
        public Guid Id { get; set; }  
        public string Message { get; set; }
        public string ConnectionID { get; set; }
        public DateTime SentAt { get; set; } 
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
        public static ChatMessage Factory(string message, string connectionID, Guid chatId) => new ChatMessage(message, connectionID, chatId);
    }
}
