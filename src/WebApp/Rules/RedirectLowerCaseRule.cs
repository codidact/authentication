using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Codidact.Authentication.WebApp.Rules
{
    public class RedirectLowerCaseRule : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            HttpRequest request = context.HttpContext.Request;
            string url = request.Scheme + "://" + request.Host + request.PathBase + request.Path;
            bool isGet = request.Method.ToLowerInvariant().Contains("get");

            if (isGet && url.Contains(".") == false && Regex.IsMatch(url, @"[A-Z]"))
            {
                HttpResponse response = context.HttpContext.Response;
                response.Clear();
                response.StatusCode = StatusCodes.Status301MovedPermanently;
                response.Headers[HeaderNames.Location] = url.ToLowerInvariant() + request.QueryString;
                context.Result = RuleResult.EndResponse;
            }
            else
            {
                context.Result = RuleResult.ContinueRules;
            }
        }
    }
}
