using System.Text.Json.Serialization;

namespace conservation_backend.Shared
{
    public class ResponseWithPagination<T>
    {
        public bool Success { get; init; }

        public string Message { get; init; } = "";

        public T? Data { get; init; }

        public PaginationMeta? Meta { get; init; }
    }

    public class ResponseWithData<T>
    {
        public bool Success { get; init; }

        public string Message { get; init; } = "";

        public T? Data { get; init; }
    }

    public class ResponseWithMessage
    {
        public bool Success { get; init; }

        public string Message { get; init; } = "";
    }

    public class PaginationMeta
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }

    public class ErrorResponse
    {
        public bool Success { get; init; } = false;

        public string Message { get; init; } = "";

        public Dictionary<string, string[]>? Errors { get; init; }
    }

    public static class ResponseMessages
    {
        public const string Ok = "OK";
        public const string Success = "The operation has been successful";
        public const string Created = "Resource created successfully";
        public const string Updated = "Resource updated successfully";
        public const string Deleted = "Resource deleted successfully";

        public const string NotFound = "The requested resource was not found";
        public const string BadRequest = "Invalid request";
        public const string Unauthorized = "Sorry, you are not authorized. Please log in again";
        public const string Forbidden = "Sorry, You don't have permission to access this resource";
        public const string Error = "Unable to complete your request due to system error. Please contact support if this continues";
        public const string DatabaseError = "Something went wrong while accessing the database. Please contact support if this continues.";
    }

    public static class ApiHttpResponse
    {
        public static ResponseWithData<object> Message(string message, bool success = true)
        {
            return new ResponseWithData<object> { Success = success, Message = message };
        }

        public static ResponseWithData<T> Data<T>(T data, string message = ResponseMessages.Ok)
        {
            return new ResponseWithData<T> { Success = true, Message = message, Data = data };
        }

        public static ResponseWithPagination<List<T>> Page<T>(PagedList<T> result, string message = ResponseMessages.Ok)
        {
            var list = result.Data is List<T> r ? r : result.Data.ToList();

            var totalPages = (int)Math.Ceiling(result.TotalCount / (double)result.PageSize);

            return new ResponseWithPagination<List<T>>
            {
                Success = true,
                Message = message,
                Data = list,
                Meta = new PaginationMeta
                {
                    Page = result.Page,
                    PageSize = result.PageSize,
                    TotalItems = result.TotalCount,
                    TotalPages = totalPages
                }
            };
        }

        public static ErrorResponse Error(string message, Dictionary<string, string[]>? errors = null)
        {
            return new ErrorResponse
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }

    }
}
