using CryptoParserSdk;

namespace NewCryptoParser.Models
{
    public class CryptoParserScheduledTask : IDisposable
    {
        public ICryptoParser CryptoParser { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public Timer ScheduledTask { get; set; }


        public CryptoParserScheduledTask(ICryptoParser cryptoParser, CancellationTokenSource cancellationTokenSource, Timer scheduledTask)
        {
            CryptoParser = cryptoParser;
            CancellationTokenSource = cancellationTokenSource;
            ScheduledTask = scheduledTask;
        }

        public void Dispose()
        {
            ScheduledTask.Dispose();
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
            CryptoParser = null;
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
        }

        ~CryptoParserScheduledTask() 
        {
            Dispose();
        }
    }
}
