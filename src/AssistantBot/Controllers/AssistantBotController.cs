using AssistantBot.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AssistantBot.Models.AssistantBot;
using AssistantBot.DataTypes;
using AssistantBot.Exceptions;

namespace AssistantBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssistantBotController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;
        private readonly IIndexedVectorStorage<EmbeddedTextVector> _indexedVectorStorage;

        public AssistantBotController(
            IChatBotService chatBotService,
            IIndexedVectorStorage<EmbeddedTextVector> indexedVectorStorage)
        {
            _chatBotService = chatBotService;
            _indexedVectorStorage = indexedVectorStorage;
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

        [HttpPost("AskToKnowledgeBase")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<IActionResult> AskToKnowledgeBase(AskToKnowledgeBaseRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Question?.Trim()))
                {
                    return Ok("Empty text.");
                }

                var questionEmbedding = await _chatBotService.GetEmbedding(request.Question);

                var knowledgeBaseTopResults = _indexedVectorStorage.SearchDataBySimilarVector(
                    new EmbeddedTextVector
                    {
                        Values = questionEmbedding.ToArray()
                    }, 
                    5);

                if (!knowledgeBaseTopResults.Any())
                    throw new AssistantBotException("Error at question processing.");

                var userQuestionPrompt = PromptTemplate.GetPromptFromTemplate(
                    string.Join("\n", knowledgeBaseTopResults.Select(x => x.ToString())),
                    request.Question);
                
                var result = await _chatBotService.SendMessage(userQuestionPrompt);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
