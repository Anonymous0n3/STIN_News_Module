using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using STIN_News_Module.Logic.Logging;

namespace STIN_News_Module.Pages.Api
{
    public class LogsModel : PageModel
    {
        public IActionResult OnGet()
        {
            var logs = LoggingService.GetLogs();
            return new JsonResult(logs);
        }
    }
}

