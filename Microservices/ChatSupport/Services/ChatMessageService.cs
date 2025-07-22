using ChatSupportApi.Interfaces;
using ChatSupportApi.Models; 

namespace ChatSupportApi.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IChatUnitOfWork _chatUnitOfWork;

        public ChatMessageService(IChatUnitOfWork chatUnitOfWork)
        {
            _chatUnitOfWork = chatUnitOfWork;
        }

        public async Task<List<ChatMessage>> GetMessages(int userId)
        {
            var chatId = _chatUnitOfWork.GetChatRepository.Get(x => x.UserId == userId).Result.Id;
            var result = await _chatUnitOfWork.GetAllChatMessagesRepository.GetAll(x => x.ChatId == chatId);
            return result.ToList(); 
        } 

        public async Task<bool> StoreMessage(ChatMessage message, int userId, int supportId)
        {
            var chat = await _chatUnitOfWork.GetChatRepository.Get(x => x.Id == message.ChatId);
            if(chat is null)
            {
                var newModel = Chat.Factory(userId, supportId);
                await _chatUnitOfWork.CreateChatRepository.CreateAsync(newModel);
                message.ChatId = newModel.Id;
            } 
            return await _chatUnitOfWork.CreateChatMessageRepository.CreateAsync(message);
        }

        public async Task<Chat> GetUserChatId(int userId)
            => await _chatUnitOfWork.GetChatRepository.Get(x => x.UserId == userId);   
    }
}
