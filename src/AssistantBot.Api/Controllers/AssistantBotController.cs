using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AssistantBot.Models.AssistantBot;
using AssistantBot.Services;

namespace AssistantBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssistantBotController : ControllerBase
    {
        private static AssistantBotService _service;

        public AssistantBotController(
            IChatBotService chatBotService,
            IIndexedVectorStorage<EmbeddedTextVector> indexedVectorStorage)
        {
            _service ??= new AssistantBotService(chatBotService, indexedVectorStorage);
        }

        [HttpPost("SendMessage")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<IActionResult> SendMessage(SendMessageRequest request)
        {
            try
            {
                var result = await _service.SendMessage(request);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AskToKnowledgeBase")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<IActionResult> AskToKnowledgeBase(AskToKnowledgeBaseRequest request)
        {
            
            try
            {
                var result = await _service.AskToKnowledgeBase(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
