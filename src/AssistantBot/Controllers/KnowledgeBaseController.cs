using AssistantBot.DataTypes;
using AssistantBot.Models.KnowledgeBase;
using AssistantBot.Services;
using AssistantBot.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace AssistantBot.Controllers
{
    public class KnowledgeBaseController : Controller
    {
        private readonly KnowledgeBaseService _service;

        public KnowledgeBaseController(
            IChatBotService chatBotService,
            IIndexedVectorStorage<EmbeddedTextVector> indexedVectorStorage)
        {
            _service = new KnowledgeBaseService(chatBotService, indexedVectorStorage);
        }

        [HttpPost("AddParagraphToKnowledgeBase")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<ActionResult> AddParagraphToKnowledgeBase([FromBody]AddParagraphToKnowledgeBaseRequest request)
        {   
            try
            {
                var storedKey = await _service.AddParagraphToKnowledgeBase(request.Paragraph);

                return Ok(storedKey);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
