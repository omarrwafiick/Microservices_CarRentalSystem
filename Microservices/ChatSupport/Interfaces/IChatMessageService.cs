using ChatSupportApi.Models;

namespace ChatSupportApi.Interfaces
{
    public interface IChatMessageService
    {
        Task<Chat> GetUserChatId(Guid userId);
        Task<List<ChatMessage>> GetMessages(Guid userId);
        Task<bool> StoreMessage(ChatMessage message, Guid userId, Guid supportId);
    }
}
