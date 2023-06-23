using Microsoft.AspNetCore.Mvc;

namespace AssistantBot.Demo.Controllers
{
    public class UploadedFiles : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
