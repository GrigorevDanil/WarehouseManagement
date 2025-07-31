
using WarehouseManagement.SharedKernel.Enums;

namespace WarehouseManagement.SharedKernel
{
    public class Error
    {
        private const string SEPARATOR = "||";
        
        public string Code { get; }
        public string Message { get; }
        public ErrorType Type { get; }
        public string? InvalidField { get; }

        private Error(string code, string message, ErrorType type, string? invalidField = null)
        {
            Code = code;
            Message = message;
            Type = type;
            InvalidField = invalidField;
        }

        public static Error Validation(string code, string message, string? invalidField = null) =>
            new(code, message, ErrorType.Validation, invalidField);

        public static Error NotFound(string code, string message) =>
            new(code, message, ErrorType.NotFound);

        public static Error Failure(string code, string message) =>
            new(code, message, ErrorType.Failure);

        public static Error Conflict(string code, string message) =>
            new(code, message, ErrorType.Conflict);
        
        public string Serialize()
        {
            return string.Join(SEPARATOR, Code, Message, Type, InvalidField ?? string.Empty);
        }

        public static Error Deserialize(string serialized)
        {
            var parts = serialized.Split(SEPARATOR);

            if (parts.Length < 3 || Enum.TryParse<ErrorType>(parts[2], out var type) == false)
            {
                throw new ArgumentException("Invalid format serialized");
            }
            
            return new Error(parts[0], parts[1], type, parts.Length > 3 ? parts[3] : null);
        }
        
        public ErrorList ToErrorList() => new([this]);
    }

    
}
