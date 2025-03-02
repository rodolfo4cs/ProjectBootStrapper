namespace FakeAppToTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("FakeApp Running");
                await Task.Delay(5000);
            }
        }
    }
}
