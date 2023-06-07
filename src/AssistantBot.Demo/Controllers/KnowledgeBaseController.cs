using Microsoft.AspNetCore.Mvc;

namespace AssistantBot.Demo.Controllers
{
    public class KnowledgeBaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UploadFile()
        {
            return View();
        }
    }
}
