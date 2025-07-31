
namespace WarehouseManagement.SharedKernel
{
    public class Errors
    {
        public static class General
        {
            public static Error ValueIsInvalid(string? value = null, string? invalidField = null)
            {
                var valueString = value == null ? "" : $"`{value}` ";
                return Error.Validation("VALUE_IS_INVALID", $"Value {valueString}is invalid", invalidField);
            }
            
            public static Error NotFound(Guid? id = null)
            {
                var forId = id == null ? "" : $" for Id '{id}'";
                return Error.NotFound("RECORD_NOT_FOUND", $"Record not found{forId}");
            }

            public static Error ArrayIsEmpty(string? arrayName = null)
            {
                var withName = arrayName == null ? "" : $"with name '{arrayName}' ";
                return Error.Validation("ARRAY_IS_EMPTY", $"Array {withName}is empty");
            }
            
            public static Error AlreadyExists(string name, string key, string? value = null)
            {
                var withValue = value == null ? "" : $" = {value}";
            
                return Error.Conflict("RECORD_ALREADY_EXISTS", $"{name} already exists with {key + withValue}");
            }

        }
        
        public static class Server
        {
            public static Error InternalServer(string message)
            {
                return Error.Failure("INTERNAL_SERVER_ERROR", message);
            }

        }

    }
}
