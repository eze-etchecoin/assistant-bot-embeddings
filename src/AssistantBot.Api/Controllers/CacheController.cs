using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;
using AssistantBot.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace AssistantBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly InDiskCache<Dictionary<string, double[]>> _inDiskCache;
        private readonly IIndexedVectorStorage<EmbeddedTextVector> _indexedVectorStorage;

        public CacheController(
            InDiskCache<Dictionary<string, double[]>> inDiskCache,
            IIndexedVectorStorage<EmbeddedTextVector> indexedVectorStorage)
        {
            _inDiskCache = inDiskCache;
            _indexedVectorStorage = indexedVectorStorage;
        }

        [HttpPatch("Reload")]
        public async Task<IActionResult> Reload()
        {
            try
            {
                var embeddings = await _inDiskCache.LoadAsync();
                if (embeddings.Count == 0)
                    return Ok();

                foreach (var embedding in embeddings)
                {
                    _indexedVectorStorage.AddVector(
                        new EmbeddedTextVector(embedding.Value, new ParagraphWithPage(1, embedding.Key)));
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
