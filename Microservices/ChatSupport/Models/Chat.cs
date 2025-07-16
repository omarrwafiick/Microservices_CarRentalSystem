using Common.Interfaces;
using Common.Models;

namespace ChatSupportApi.Models
{
    public class Chat : BaseEntity
    {
        private Chat()
        {  
        }
        private Chat(Guid userId, Guid supportId)
        {  
            UserId = userId;
            SupportId = supportId;
            CreatedAt = DateTime.UtcNow;
        } 
        public Guid UserId { get; set; }
        public Guid SupportId { get; set; }
        public DateTime CreatedAt { get; set; }  
        public List<ChatMessage> ChatMessages { get; set; }
        public static Chat Factory(Guid userId, Guid supportId) => new Chat(userId, supportId); 

    }
}
