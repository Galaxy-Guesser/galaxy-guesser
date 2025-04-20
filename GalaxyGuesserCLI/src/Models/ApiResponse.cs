using System.Collections.Generic;

namespace GalaxyGuesserCLI.Models
{
  /// <summary>
  /// Standard API response model for consistent responses across all endpoints
  /// </summary>
  /// <typeparam name="T">The type of data being returned</typeparam>
  public class ApiResponse<T>
  {
    /// <summary>
    /// Indicates if the request was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The data returned by the API
    /// </summary>
    public T Data { get; set; } = default;

    /// <summary>
    /// A message describing the result of the operation
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Any errors that occurred during the operation
    /// </summary>
    public List<string> Errors { get; set; } = new List<string>();

    /// <summary>
    /// Creates a successful response with data
    /// </summary>
    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully")
    {
      return new ApiResponse<T>
      {
        Success = true,
        Data = data,
        Message = message
      };
    }

    /// <summary>
    /// Creates a failed response with error messages
    /// </summary>
    public static ApiResponse<T> ErrorResponse(string message, List<string> errors = null)
    {
      return new ApiResponse<T>
      {
        Success = false,
        Message = message,
        Errors = errors ?? new List<string>()
      };
    }
  }
}