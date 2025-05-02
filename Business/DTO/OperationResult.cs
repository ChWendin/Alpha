

namespace Business.DTO;

    public class OperationResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public static OperationResult Ok() => new() { Success = true };
        public static OperationResult Fail(string err) => new() { Success = false, Error = err };
    }

