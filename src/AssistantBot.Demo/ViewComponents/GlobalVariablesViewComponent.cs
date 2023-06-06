using Microsoft.AspNetCore.Mvc;

namespace AssistantBot.Demo.ViewComponents
{
    public class GlobalVariablesViewComponent : ViewComponent
    {
        private readonly IConfiguration _config;

        public GlobalVariablesViewComponent(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var apiUrl = _config.GetSection("ApiUrl").Value;
            return View((object)apiUrl);
        }
    }
}
