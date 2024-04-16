namespace JourneyHub.Domain.Abstractions

{
    public record ErrorType(string Code, string Name)
    {
        public readonly static ErrorType None = new(string.Empty, string.Empty);

        public readonly static ErrorType NullValue = new("Error.NullValue", "Null value was provided");
    }
}
