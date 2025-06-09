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

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetMessages(Guid userId)
        { 
            var messages = await _chatService.GetMessages(userId);
            return Ok(messages.Select(x => new { Messages = x.Message, ChatId = x.ChatId}));
        }
    }
}
