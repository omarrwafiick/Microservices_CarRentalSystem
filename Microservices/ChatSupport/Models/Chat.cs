 
using Common.Models;

namespace ChatSupportApi.Models
{
    public class Chat : BaseEntity
    {
        private Chat()
        {  
        }
        public static Chat Factory(int userId, int supportId) => new Chat
        {
            UserId = userId,
            SupportId = supportId,
            CreatedAt = DateTime.UtcNow
        };

        public int UserId { get; set; }
        public int SupportId { get; set; }
        public DateTime CreatedAt { get; set; }  
        public List<ChatMessage> ChatMessages { get; set; }
        
    }
}
