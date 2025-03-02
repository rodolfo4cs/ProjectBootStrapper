using System.Collections;

namespace Core.CommandLine
{
    internal sealed class Command : IEnumerable
    {
        public List<Option> Options { get; private set; } = [];

        public IEnumerator GetEnumerator()
        {
            return Options.GetEnumerator();
        }

        public void Add(Option option)
        {
            Options.Add(option);
        }
    }
}
