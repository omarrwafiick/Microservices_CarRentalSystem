using ChatSupportApi.Interfaces;
using ChatSupportApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatSupportApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatMessageService _chatService;
        public ChatHub(IChatMessageService chatService)
        {
            _chatService = chatService;
        }
        public async Task SendMessage(string toUserIdRoute, Guid userId, Guid chatId, string userName, string message)
        {
            var ID = Context.UserIdentifier;
            await _chatService.StoreMessage(ChatMessage.Factory(message, ID, chatId), userId);
            await Clients.User(toUserIdRoute).SendAsync("ReceiveMessage", userName, ID, message);
        }
    }
}
