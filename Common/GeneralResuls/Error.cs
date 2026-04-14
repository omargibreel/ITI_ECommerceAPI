using System;
using System.Collections.Generic;
using System.Text;

namespace Common.GeneralResuls
{
    public record Error(string Code, string Message, string? Field = null)
    {
        public static Error None = new Error(string.Empty, string.Empty);
        public static Error NotFound(string entity, object id)
            => new Error($"{entity}.NotFound", $"{entity} with id {id} was not found.");
        public static Error Validation(string field, string message)
            => new Error("Validation.Failed", message, field);
        public static Error Validation(string message)
            => new Error("Validation.Failed", message);
        public static Error Conflict(string message)
            => new Error("Conflict", message);
        public static Error OperationFailed(string message)
            => new Error("Operation.Failed", message);
        public static Error Unauthorized()
            => new("Auth.Unauthorized", "You are not authorized to perform this action.");
    }
}
