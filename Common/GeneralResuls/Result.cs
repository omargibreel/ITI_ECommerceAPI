using Common.GeneralResuls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Common.Result
{

    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        //=======================================================================================================================

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IReadOnlyList<Error>? Errors { get; }

        //=======================================================================================================================

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, List<string>>? FieldErrors =>
         Errors == null || !Errors.Any(e => e.Field != null) ? null
         : Errors.Where(e => e.Field != null)
         .GroupBy(e => e.Field!)
         .ToDictionary(g => g.Key, g => g.Select(e =>
        e.Message).ToList());

        //=======================================================================================================================


        private Result(IReadOnlyList<Error> errors, bool isSuccess)
        {
            Errors = errors;
            IsSuccess = isSuccess;
        }

        public static Result Success() => new(Array.Empty<Error>(),
     true);
        public static Result Fail(Error error) => new(new[] { error }, false);
        public static Result Fail(IReadOnlyList<Error> errors) => new(errors,
       false);
        public static Result NotFound(string entity, object id)
        => Fail(Error.NotFound(entity, id));
        public static Result FailOperation(string message)
        => Fail(Error.OperationFailed(message));

    }




    //=======================================================================================================================
    //=======================================================================================================================
    //=======================================================================================================================
    //=======================================================================================================================


    public class Result<T> where T : class
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Value { get; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IReadOnlyList<Error>? Errors { get; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, List<string>>? FieldErrors =>
        Errors == null || !Errors.Any(e => e.Field != null) ? null
        : Errors.Where(e => e.Field != null)
        .GroupBy(e => e.Field!)
        .ToDictionary(g => g.Key, g => g.Select(e =>
       e.Message).ToList());
        private Result(T? value, IReadOnlyList<Error> errors, bool isSuccess)
        { Value = value; Errors = errors; IsSuccess = isSuccess; }
        public static Result<T> Success(T value) => new(value,
       Array.Empty<Error>(), true);
        public static Result<T> Fail(Error error) => new(null, new[] { error },
       false);
        public static Result<T> Fail(IReadOnlyList<Error> errors) => new(null,
       errors, false);
        public static Result<T> NotFound(string entity, object id)
        => Fail(Error.NotFound(entity, id));
        public static Result<T> FailOperation(string message)
        => Fail(Error.OperationFailed(message));
        public static Result<T> ValidationFail(string message)
        => Fail(Error.Validation(message));
        public static Result<T> ValidationFail(IReadOnlyList<Error> errors)
        => Fail(errors);
    }

}
