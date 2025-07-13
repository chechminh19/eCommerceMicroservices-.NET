
namespace eCommerceLibrary.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }

        public ApiResponse(bool success,int statusCode, string message, T? data, Dictionary<string, string[]>? errors = null)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
            Data = data; 
            Errors = errors;
        }
    }
}
