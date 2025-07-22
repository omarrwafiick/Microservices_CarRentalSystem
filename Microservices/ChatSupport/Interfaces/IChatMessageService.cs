using ChatSupportApi.Models;

namespace ChatSupportApi.Interfaces
{
    public interface IChatMessageService
    {
        Task<Chat> GetUserChatId(int userId);
        Task<List<ChatMessage>> GetMessages(int userId);
        Task<bool> StoreMessage(ChatMessage message, int userId, int supportId);
    }
}
