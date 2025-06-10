using ChatSupportApi.Interfaces;
using ChatSupportApi.Models;
using Common.Interfaces;

namespace ChatSupportApi.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IGetAllRepository<ChatMessage> _getMessagesRepository;
        private readonly IGetRepository<ChatMessage> _getMessageRepository;
        private readonly ICreateRepository<ChatMessage> _createMessageRepository;
        private readonly ICreateRepository<Chat> _createChatRepository;
        private readonly IGetRepository<Chat> _getChatRepository;

        public ChatMessageService(
            IGetAllRepository<ChatMessage> getMessagesRepository,
            IGetRepository<ChatMessage> getMessageRepository,
            ICreateRepository<ChatMessage> createMessageRepository,
            ICreateRepository<Chat> createChatRepository,
            IGetRepository<Chat> getChatRepository
        )
        {
            _getMessagesRepository = getMessagesRepository;
            _getMessageRepository = getMessageRepository;
            _createMessageRepository = createMessageRepository;
            _createChatRepository = createChatRepository;
            _getChatRepository = getChatRepository;
        }
        public async Task<List<ChatMessage>> GetMessages(Guid userId)
        {
            var chatId = _getChatRepository.Get(x => x.UserId == userId).Result.Id;
            var result = await _getMessagesRepository.GetAll(x => x.ChatId == chatId);
            return result.ToList(); 
        } 

        public async Task<bool> StoreMessage(ChatMessage message, Guid userId, Guid supportId)
        {
            var chat = await _getChatRepository.Get(x => x.Id == message.ChatId);
            if(chat is null)
            {
                var ID = Guid.NewGuid();
                await _createChatRepository.CreateAsync(Chat.Factory(ID, userId, supportId));
                message.ChatId = ID;
            } 
            return await _createMessageRepository.CreateAsync(message);
        }

        public async Task<Chat> GetUserChatId(Guid userId)
            => await _getChatRepository.Get(x => x.UserId == userId);   
    }
}
