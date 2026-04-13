using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Common.Response;

namespace TaskFlow.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(NotFoundException ex)
            {
                await WriteErrorResponse(context, StatusCodes.Status404NotFound, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await WriteErrorResponse(context, StatusCodes.Status409Conflict, ex.Message);
            }
            catch (ArgumentException ex)
            {
                await WriteErrorResponse(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception)
            {
                await WriteErrorResponse(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        private static async Task WriteErrorResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                Message = message
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
        private static async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = ex switch
            {
                ValidationException valEx => new ApiResponse<object>
                {
                    Success = false,
                    Error = string.Join(", ", valEx.Message.Select(e => e.ToString()))
                },

                UnauthorizedAccessException => new ApiResponse<object>
                {
                    Success = false,
                    Error = "Unauthorized"
                },

                _ => new ApiResponse<object>
                {
                    Success = false,
                    Error = "Internal server error"
                }
            };

            context.Response.StatusCode = ex switch
            {
                ValidationException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
