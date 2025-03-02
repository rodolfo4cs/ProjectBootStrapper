namespace Core.CommandLine
{
    internal readonly struct Option(string value, string acceptedValue, string description)
    {
        public string OptionValue { get; } = value;
        public string AcceptedValue { get; } = acceptedValue;
        public string Description { get; } = description;
    }
}
