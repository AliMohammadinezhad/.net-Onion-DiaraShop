namespace AccountManagement.Infrastructure.EfCore.Middleware;

using AccountManagement.Application.Contract.VisitorService;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

public class VisitorTrackingMiddleware
{
    private readonly RequestDelegate _next;

    public VisitorTrackingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IVisitorService visitorService)
    {
        // Check for the Unique Visitor cookie
        var visitorCookie = context.Request.Cookies["UniqueVisitorId"];

        if (string.IsNullOrEmpty(visitorCookie))
        {
            // Generate a new visitor ID
            var uniqueVisitorId = Guid.NewGuid().ToString();

            // Save the new visitor ID in a cookie
            context.Response.Cookies.Append("UniqueVisitorId", uniqueVisitorId, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

            // Log the new visitor through the service
            visitorService.TrackVisitor(uniqueVisitorId);
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }
}
