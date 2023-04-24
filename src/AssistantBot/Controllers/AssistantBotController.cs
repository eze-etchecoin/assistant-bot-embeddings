using AssistantBot.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using QAAssistantBot.Models;

namespace QAAssistantBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssistantBotController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;

        public AssistantBotController(IChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        [HttpPost("SendMessage")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<IActionResult> SendMessage(SendMessageRequest request)
        {
            try
            {
                var result = await _chatBotService.SendMessage(request.Message);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
