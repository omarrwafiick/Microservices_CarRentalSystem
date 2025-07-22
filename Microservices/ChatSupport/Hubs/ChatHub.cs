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

        public async Task SendMessage(int chatId, int userId, int supportId, string message)
        {
            var senderId = Context.UserIdentifier!;
            var receiverId = senderId == userId.ToString() ? supportId.ToString() : userId.ToString();

            var msg = ChatMessage.Factory(message, senderId, chatId);
            await _chatService.StoreMessage(msg, userId, supportId);
             
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
             
            await Clients.User(senderId).SendAsync("ReceiveMessage", senderId, message);
        }
    }
}
