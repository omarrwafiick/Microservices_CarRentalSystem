using ChatSupportApi.Interfaces; 
using Microsoft.AspNetCore.Mvc;

namespace ChatSupportApi.Controllers
{
    [Route("api/supportchat")]
    [ApiController]
    public class SupportChatController : ControllerBase
    {
        private readonly IChatMessageService _chatService;
        public SupportChatController(IChatMessageService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("history/{userId:int}")]
        public async Task<IActionResult> GetMessages([FromRoute] int userId)
        { 
            var messages = await _chatService.GetMessages(userId);
            return Ok(messages.Select(x => new { Messages = x.Message, ChatId = x.ChatId}));
        }

        [HttpGet("chat/{userId:int}")]
        public async Task<IActionResult> GetChatId([FromRoute] int userId)
        {
            var chat = await _chatService.GetUserChatId(userId);
            return Ok(chat.Id);
        }
    }
}
