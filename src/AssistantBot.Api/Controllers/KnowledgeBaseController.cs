using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Exceptions;
using AssistantBot.Common.Interfaces;
using AssistantBot.Models.KnowledgeBase;
using AssistantBot.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace AssistantBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KnowledgeBaseController : ControllerBase
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

        [HttpPost("UploadFile")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult UploadFile(IFormFile file)
        {
            try
            {
                var fileName = "";
                var uploadedFilePath = "";

                // This is for debugging purposes only.
                //Thread.Sleep(3_000);

                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition)
                        .FileName.ToString().Trim('"');

                    var extension = Path.GetExtension(Path.GetFileName(fileName));

                    var validExtensions = new List<string>{ ".pdf", ".docx", ".doc" };
                    if (!validExtensions.Contains(extension.ToLower()))
                        throw new AssistantBotException("Invalid file extension.");

                    var uploadedFilesFolder = Path.Combine(".", "UploadedFiles");
                    if (!Directory.Exists(uploadedFilesFolder))
                    {
                        Directory.CreateDirectory(uploadedFilesFolder);
                    }

                    uploadedFilePath = Path.Combine(uploadedFilesFolder, fileName);

                    if (System.IO.File.Exists(uploadedFilePath))
                    {
                        throw new AssistantBotException("File already exists.");
                    }

                    using var fileStream = new FileStream(uploadedFilePath, FileMode.Create);
                    var inputStream = file.OpenReadStream();
                    inputStream.CopyTo(fileStream);
                }

                if (string.IsNullOrEmpty(uploadedFilePath))
                    throw new Exception("No file was uploaded.");

                _ = Task.Run(() => _service.LoadFileToKnowledgeBase(uploadedFilePath, fileName), CancellationToken.None);

                return Ok(fileName);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("CheckProgress")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(KnowledgeBaseFileInfo))]
        //public IActionResult CheckProgress(string fileName)
        //{
        //    var progress = _service.GetDocumentProcessingStatus(fileName);
        //    return Ok(progress);
        //}

        [HttpGet("GetKnowledgeBaseFileInfo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(KnowledgeBaseFileInfo))]
        public IActionResult GetKnowledgeBaseFileInfo(string fileName)
        {
            var fileInfo = _service.GetKnowledgeBaseFileInfo(fileName);
            return Ok(fileInfo);
        }

        [HttpGet("GetLastUploadedFileInfo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(KnowledgeBaseFileInfo))]
        public IActionResult GetLastUploadedFileInfo()
        {
            var fileInfo = _service.GetLastUploadedFileInfo();
            return Ok(fileInfo);
        }

        //[HttpGet("Test")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        //public IActionResult Test()
        //{
        //    return Ok("Hello from API!");
        //}
    }
}
