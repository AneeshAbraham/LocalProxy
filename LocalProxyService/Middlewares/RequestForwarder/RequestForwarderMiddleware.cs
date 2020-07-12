using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LocalProxyService.Middlewares.RequestForwarder
{
    public class RequestForwarderMiddleware
    {
        private readonly RequestDelegate _next;
        // set the host later
        private readonly string host = "https://localhost:44325";

        public RequestForwarderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;
            var uri = new Uri($"{host}{path}");

            var requestMessage = httpContext.CreateProxyHttpRequest(uri);
            var httpClient = HttpClientFactory.Create();

            var httpResponseMessage = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, httpContext.RequestAborted);

            await httpContext.CopyProxyHttpResponse(httpResponseMessage);

        }
    }
}
