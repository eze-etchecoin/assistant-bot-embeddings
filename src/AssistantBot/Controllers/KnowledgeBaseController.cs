﻿using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;
using AssistantBot.Models.KnowledgeBase;
using AssistantBot.Services;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("/UploadFileToKnowledgeBase")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        //[ServiceFilter(typeof(ValidateMimeMultipartContentFilter))]
        public IActionResult UploadFileToKnowledgeBase(FileDescriptionShort fileDescriptionShort)
        {
            var uploadedFilePath = "";
            //var contentTypes = new List<string>();

            foreach (var file in fileDescriptionShort.File)
            {
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition)
                        .FileName.ToString().Trim('"');

                    //contentTypes.Add(file.ContentType);

                    //names.Add(fileName);

                    var extension = Path.GetExtension(Path.GetFileName(fileName));

                    var fileName2 = Path.Combine(".", "UploadedFiles", fileDescriptionShort.Name + extension);

                    //if (formFile == null)
                    //{
                    //    throw new ArgumentNullException(nameof(formFile));
                    //}

                    using var fileStream = new FileStream(fileName2, FileMode.Create);
                    var inputStream = file.OpenReadStream();
                    inputStream.CopyTo(fileStream);

                    uploadedFilePath = fileName2;
                }
            }

            if(string.IsNullOrEmpty(uploadedFilePath))
                throw new Exception("No file was uploaded.");

            _ = Task.Run(() => _service.LoadFileToKnowledgeBase(uploadedFilePath), CancellationToken.None);

            return Ok(uploadedFilePath);
        }

        [HttpGet("GetDocumentProcessingStatus")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public IActionResult GetDocumentProcessingStatus(string filePath)
        {
            var progress = _service.GetDocumentProcessingStatus(filePath);
            return Ok(progress);
        }

        public class FileDescriptionShort
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public string Name { get; set; }
            public ICollection<IFormFile> File { get; set; }
        }
    }
}
