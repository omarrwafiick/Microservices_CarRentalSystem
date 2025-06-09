using Common.Interfaces;

namespace ChatSupportApi.Models
{
    public class Chat : IBaseEntity
    {
        private Chat()
        {  
        }
        private Chat(Guid userId)
        { 
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
        private Chat(Guid id, Guid userId)
        { 
            Id = id;
            CreatedAt = DateTime.UtcNow;
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }  
        public List<ChatMessage> ChatMessages { get; set; }
        public static Chat Factory(Guid userId) => new Chat(userId);
        public static Chat Factory(Guid id, Guid userId) => new Chat(id, userId);

    }
}
