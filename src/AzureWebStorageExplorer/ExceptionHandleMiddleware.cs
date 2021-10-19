using System;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Prometheus;

namespace AzureWebStorageExplorer
{
    public class ExceptionHandleMiddleware
    {
        private readonly RequestDelegate next;

        private static readonly Counter ExceptionCounter = Metrics.CreateCounter("unhandledexceptions_counter_total", "Keep unhandled exception count");

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
            catch (RequestFailedException rfex)
            {
                ExceptionCounter.Inc();

                context.Response.Clear();
                context.Response.ContentType = @"application/json";
                context.Response.StatusCode = rfex.Status;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { description = $"{rfex.GetType().FullName}: '{rfex.Message}'", statusText = $"{rfex.ErrorCode}" }));
            }
            catch (Exception ex)
            {
                ExceptionCounter.Inc();

                context.Response.Clear();
                context.Response.ContentType = @"application/json";
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { description = $"{ex.GetType().FullName}: '{ex.Message}'", statusText = $"{ex.Message}" }));
            }
        }
    }
}
