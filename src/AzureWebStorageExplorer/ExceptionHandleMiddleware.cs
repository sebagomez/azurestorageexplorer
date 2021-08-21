using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AzureWebStorageExplorer
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandleMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.Clear();
                context.Response.ContentType = @"application/json";
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { description = $"{ex.GetType().FullName}: '{ex.Message}'", statusText = $"{ex.Message}" }));
            }
        }
    }
}
