using System.Text.Json;

namespace GalaxyGuesserCLI.Models
{

    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Data { get; }

        public string Message { get; }
        public List<string> Errors { get; }

        public static Result<T> Success(T data, string message = "Operation completed successfully")
        {
            return new Result<T>(true, data, message, new List<string>());
        }

        public static Result<T> Failure(string message, List<string> errors = null)
        {
            return new Result<T>(false, default, message, errors ?? new List<string>());
        }

       
        public static async Task<Result<T>> FromApiResponseAsync(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                {
                    return Failure($"HTTP {(int)response.StatusCode} {response.StatusCode}", new List<string> { content });
                }

                if (string.IsNullOrEmpty(content) || content == "null")
                {
                    return Success(default, "Operation completed successfully");
                }

                try
                {
                    using var doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;

                    if (!root.TryGetProperty("success", out var successElement) || !successElement.GetBoolean())
                    {
                        var errorMessage = root.TryGetProperty("message", out var messageElement) ? messageElement.GetString() : "Operation failed";
                        var errors = new List<string>();
                        if (root.TryGetProperty("errors", out var errorsElement))
                        {
                            foreach (var error in errorsElement.EnumerateArray())
                            {
                                errors.Add(error.GetString());
                            }
                        }
                        return Failure(errorMessage, errors);
                    }

                    if (!root.TryGetProperty("data", out var dataElement))
                    {
                        return Success(default, root.TryGetProperty("message", out var msgElement) ? msgElement.GetString() : "Operation completed successfully");
                    }

                    var data = JsonSerializer.Deserialize<T>(dataElement.GetRawText());
                    var successMessage = root.TryGetProperty("message", out var successMsgElement) ? successMsgElement.GetString() : "Operation completed successfully";
                    return Success(data, successMessage);
                }
                catch (JsonException ex)
                {
                    return Failure("Failed to parse response", new List<string> { ex.Message, content });
                }
            }
            catch (Exception ex)
            {
                return Failure("Failed to process HTTP response", new List<string> { ex.Message });
            }
        }

        public Result<T> OnSuccess(Action<T> action)
        {
            if (IsSuccess)
            {
                action(Data);
            }
            return this;
        }

        public Result<T> OnFailure(Action<List<string>> action)
        {
            if (!IsSuccess)
            {
                action(Errors);
            }
            return this;
        }

        private Result(bool isSuccess, T data, string message, List<string> errors)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
            Errors = errors;
        }
    }

    public class Result
    {

        public bool IsSuccess { get; }

       
        public string Message { get; }

      
        public List<string> Errors { get; }


        public static Result Success(string message = "Operation completed successfully")
        {
            return new Result(true, message, new List<string>());
        }


        public static Result Failure(string message, List<string> errors = null)
        {
            return new Result(false, message, errors ?? new List<string>());
        }

        public static async Task<Result> FromHttpResponseAsync(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                {
                    return Failure($"HTTP {(int)response.StatusCode} {response.StatusCode}", new List<string> { content });
                }

                return Success("Operation completed successfully");
            }
            catch (Exception ex)
            {
                return Failure("Failed to process HTTP response", new List<string> { ex.Message });
            }
        }


        public static Result FromException(Exception ex, string message = "An error occurred")
        {
            return Failure(message, new List<string> { ex.Message });
        }

        public Result OnSuccess(Action action)
        {
            if (IsSuccess)
            {
                action();
            }
            return this;
        }

    
        public Result OnFailure(Action<List<string>> action)
        {
            if (!IsSuccess)
            {
                action(Errors);
            }
            return this;
        }

        private Result(bool isSuccess, string message, List<string> errors)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors = errors;
        }
    }
} 