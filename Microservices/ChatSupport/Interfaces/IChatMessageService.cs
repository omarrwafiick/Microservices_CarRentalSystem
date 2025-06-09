using ChatSupportApi.Models;

namespace ChatSupportApi.Interfaces
{
    public interface IChatMessageService
    {
        Task<List<ChatMessage>> GetMessages(Guid userId);
        Task<bool> StoreMessage(ChatMessage message, Guid userId);
    }
}
