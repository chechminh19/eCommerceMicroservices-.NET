

using eCommerceLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace eCommerceLibrary.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // Declare default variables
            string mess = "Sorry, internal server error occurre. Kindly try again.";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string title = "Error";
            try
            {
                await next(context);
                /// check if Response is too Many Request //429
                if(context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    mess = "Too many request";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, title, mess, statusCode);
                }
                //if Response is UnAuthorized // 401
                if(context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    mess = "You are not Authorized";
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context, title, mess, statusCode);

                }
                //if Response is Forbidden // 403
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Out of Access";
                    mess = "You are not allowed to access";
                    statusCode = (int)StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, title, mess, statusCode);

                }
            }catch (Exception ex)
            {
                //Log Original Exceptions /File, Debugger, Console
                LogExceptions.LogException(ex);
                // check if Exception is Timeout
                if(ex is TaskCanceledException || ex is TimeoutException) 
                {
                    title = "Out of time";
                    mess = "Request time...try again";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }
                //if none of the exception || Exception caught then do the default
                await ModifyHeader(context, title, mess, statusCode);
            }
            
        }

        private static async Task ModifyHeader(HttpContext context, string title, string mess, int statusCode)
        {
            // display scary-free messagee to client
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails() 
            {
              Detail = mess,
              Status = statusCode,
              Title = title

            }), CancellationToken.None);
            return;
        }
    }
    
}
