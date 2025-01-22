using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Backend.Core.Utilities
{
    /// <summary>
    /// Represents the status of an operation including success, failure, and warning states
    /// </summary>
    public class ResultStatus
    {
        public bool IsPassed { get; set; }
        public bool IsFailed { get; set; }
        public bool IsWarning { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
    }

    /// <summary>
    /// Generic version of ResultNotifier for strongly-typed result data
    /// </summary>
    public class ResultNotifier<T>
    {
        public ResultStatus ResultStatus { get; set; } = new()
        {
            IsPassed = false,
            IsFailed = true,
            IsWarning = false,
            Message = string.Empty,
            Detail = string.Empty
        };

        public T? ResultData { get; set; }

        public ResultNotifier() { }

        public ResultNotifier(ActionContext context)
        {
            ResultStatus = new ResultStatus
            {
                IsFailed = true,
                IsPassed = false,
                Message = BuildErrorMessages(context),
                Detail = BuildErrorMessages(context),
            };
        }

        public static ResultNotifier<T> Success(T data, string message = "Success")
        {
            return new ResultNotifier<T>
            {
                ResultStatus = new ResultStatus
                {
                    IsPassed = true,
                    IsFailed = false,
                    IsWarning = false,
                    Message = message,
                    Detail = message
                },
                ResultData = data
            };
        }

        public static ResultNotifier<T> Failure(string message, string? detail = null)
        {
            return new ResultNotifier<T>
            {
                ResultStatus = new ResultStatus
                {
                    IsPassed = false,
                    IsFailed = true,
                    IsWarning = false,
                    Message = message,
                    Detail = detail ?? message
                }
            };
        }

        public static ResultNotifier<T> Warning(T data, string message)
        {
            return new ResultNotifier<T>
            {
                ResultStatus = new ResultStatus
                {
                    IsPassed = true,
                    IsFailed = false,
                    IsWarning = true,
                    Message = message,
                    Detail = message
                },
                ResultData = data
            };
        }

        private string BuildErrorMessages(ActionContext context)
        {
            if (!context.ModelState.IsValid)
            {
                return string.Join(", ",
                    context.ModelState
                        .Where(ms => ms.Value?.Errors.Count > 0)
                        .SelectMany(kvp =>
                            kvp.Value?.Errors.Select(error =>
                                GetErrorMessage(error, kvp.Key)) ?? Array.Empty<string>())
                );
            }
            return string.Empty;
        }

        private static string GetErrorMessage(ModelError error, string key)
        {
            if (string.IsNullOrEmpty(error.ErrorMessage))
            {
                return "The input was not valid.";
            }

            var message = error.ErrorMessage.TrimEnd('.');
            return $"{key}: {message}";
        }
    }

    /// <summary>
    /// Non-generic version of ResultNotifier for backward compatibility
    /// </summary>
    public class ResultNotifier : ResultNotifier<object>
    {
        public ResultNotifier() : base() { }

        public ResultNotifier(ActionContext context) : base(context) { }

        public static ResultNotifier FromGeneric<T>(ResultNotifier<T> generic)
        {
            return new ResultNotifier
            {
                ResultStatus = generic.ResultStatus,
                ResultData = generic.ResultData
            };
        }

        public static new ResultNotifier Success(object? data = null, string message = "Success")
        {
            return new ResultNotifier
            {
                ResultStatus = new ResultStatus
                {
                    IsPassed = true,
                    IsFailed = false,
                    IsWarning = false,
                    Message = message,
                    Detail = message
                },
                ResultData = data
            };
        }

        public static new ResultNotifier Failure(string message, string? detail = null)
        {
            return new ResultNotifier
            {
                ResultStatus = new ResultStatus
                {
                    IsPassed = false,
                    IsFailed = true,
                    IsWarning = false,
                    Message = message,
                    Detail = detail ?? message
                }
            };
        }

        public static new ResultNotifier Warning(object? data, string message)
        {
            return new ResultNotifier
            {
                ResultStatus = new ResultStatus
                {
                    IsPassed = true,
                    IsFailed = false,
                    IsWarning = true,
                    Message = message,
                    Detail = message
                },
                ResultData = data
            };
        }
    }
}
