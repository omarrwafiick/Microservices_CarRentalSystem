using Common.Interfaces;

namespace ChatSupportApi.Models
{
    public class Chat : BaseEntity
    {
        private Chat()
        {  
        }
        private Chat(Guid userId, Guid supportId)
        { 
            Id = Guid.NewGuid();
            UserId = userId;
            SupportId = supportId;
            CreatedAt = DateTime.UtcNow;
        }
        private Chat(Guid id, Guid userId, Guid supportId)
        { 
            Id = id;
            UserId = userId;
            SupportId = supportId;
            CreatedAt = DateTime.UtcNow;
        }  
        public Guid UserId { get; set; }
        public Guid SupportId { get; set; }
        public DateTime CreatedAt { get; set; }  
        public List<ChatMessage> ChatMessages { get; set; }
        public static Chat Factory(Guid userId, Guid supportId) => new Chat(userId, supportId);
        public static Chat Factory(Guid id, Guid userId, Guid supportId) => new Chat(id, userId, supportId);

    }
}
