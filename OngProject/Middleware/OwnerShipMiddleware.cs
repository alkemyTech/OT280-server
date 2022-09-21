using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using OngProject.Services.Interfaces;
using SendGrid.Helpers.Errors.Model;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OngProject.Middleware
{
    public class OwnerShipMiddleware
    {
        private readonly RequestDelegate _next;

        public OwnerShipMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string controller = context.Request.RouteValues["controller"] as string;
            string action = context.Request.RouteValues["action"] as string;
            
            if (controller == "User")
                if (action == "GetById" || action == "Delete" || action == "Patch")
                {
                    var currentUserId = context?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var currentUserRole = context?.User.FindFirstValue(ClaimTypes.Role);
                
                    // Retrieve the RouteData, and access the route values
                    var routeValues = context.GetRouteData().Values;
            
                    // Extract the values
                    var idValue = routeValues["id"] as string;

                    if (idValue != currentUserId && currentUserRole != "admin")
                    {
                        context.Response.StatusCode = 403;
                        return;
                    }
                     
                }
                        
            await _next(context);

        }
    }
}
