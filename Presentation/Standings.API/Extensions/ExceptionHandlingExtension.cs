using Microsoft.AspNetCore.Diagnostics;
using Serilog; // Serilog is included
using Standings.Application.Models;
using System.Net;
using System.Text.Json;

namespace MentorApi.Extensions
{
    public static class ExceptionHandlingExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    // Log the fact that the exception handler has been triggered
                    Log.Information("Exception handler triggered.");

                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        // Log the exception details
                        Log.Error(contextFeature.Error, "An unhandled exception occurred.");

                        await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = $"Internal Server Error: {contextFeature.Error.Message}",
                        }));
                    }
                });
            });
        }
    }
}
