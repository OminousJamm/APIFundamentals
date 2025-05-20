using APIFundamentals.Interfaces;

namespace APIFundamentals.Implemetation
{
    public class ConsoleLogginService : ILogginService
    {
        public void Log(string message)
        {
          Console.WriteLine(message);
        }
    }
}
