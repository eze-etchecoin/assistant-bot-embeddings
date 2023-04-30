using AssistantBot.DataTypes;
using AssistantBot.Exceptions;
using AssistantBot.Models.KnowledgeBase;
using AssistantBot.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace AssistantBot.Controllers
{
    public class KnowledgeBaseController : Controller
    {
        private readonly IChatBotService _chatBotService;
        private readonly IIndexedVectorStorage<EmbeddedTextVector> _indexedVectorStorage;

        public KnowledgeBaseController(
            IChatBotService chatBotService,
            IIndexedVectorStorage<EmbeddedTextVector> indexedVectorStorage)
        {
            _chatBotService = chatBotService;
            _indexedVectorStorage = indexedVectorStorage;
        }

        [HttpPost("AddParagraphToKnowledgeBase")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<ActionResult> AddParagraphToKnowledgeBase([FromBody]AddParagraphToKnowledgeBaseRequest request)
        {   
            try
            {
                if (string.IsNullOrEmpty(request.Paragraph))
                    return Ok("Empty text.");

                    var embedding = await _chatBotService.GetEmbedding(request.Paragraph);
                if (embedding == null)
                    throw new AssistantBotException("An error has ocurred getting the embedding for given text.");

                var storedKey = _indexedVectorStorage.AddVector(new EmbeddedTextVector
                {
                    Values = embedding.ToArray(),
                    ParagraphWithPage = new ParagraphWithPage(1, request.Paragraph)
                });

                return Ok(storedKey);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
