using System.Security.Claims;
using HataChatSerives.Models;
using HataChatSerives.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using HataChatSerives.Utility;

namespace HataChatSerives.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(ChatService chatService, IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageDto messageDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? User.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized(new { Message = "nameid claim not found" });
            }

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { Message = "Invalid nameid value" });
            }

            var message = new ChatMessage
            {
                SenderId = userId,
                ReceiverId = messageDto.ReceiverId,
                Message = messageDto.Message,
                Timestamp = DateTime.UtcNow
            };

            try
            {
                var result = await _chatService.SendMessageAsync(message);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", userId, messageDto.Message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("messages/{userId}")]
        public async Task<IActionResult> GetMessages(int userId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var messages = await _chatService.GetMessagesAsync(currentUserId, userId);
            return Ok(messages);
        }

        [HttpGet("chats")]
        public async Task<IActionResult> GetChats()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var chats = await _chatService.GetUserChatsAsync(userId);
            return Ok(chats);
        }
    }
}
