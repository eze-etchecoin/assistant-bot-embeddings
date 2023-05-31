using Microsoft.AspNetCore.Mvc;

namespace AssistantBot.Demo.Controllers
{
    public class QueryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
