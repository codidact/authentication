using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http.Extensions;

namespace Codidact.Authentication.Web.Models
{
    public class DummyModel : PageModel
    {
        private readonly ILogger<DummyModel> _logger;

        public DummyModel(ILogger<DummyModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            _logger.LogWarning(Request.GetDisplayUrl());
            return Page();
        }

        public IActionResult OnPost()
        {
            _logger.LogWarning(Request.GetDisplayUrl());
            return Page();
        }
    }
}

