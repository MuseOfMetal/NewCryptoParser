using CryptoParserSdk;

namespace NewCryptoParser.Models
{
    public class CryptoParserScheduledTask : IDisposable
    {
        public ICryptoParser CryptoParser { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public Task PeriodicTask { get; set; }


        public CryptoParserScheduledTask(ICryptoParser cryptoParser, CancellationTokenSource cancellationTokenSource, Task periodicTask)
        {
            CryptoParser = cryptoParser;
            CancellationTokenSource = cancellationTokenSource;
            PeriodicTask = periodicTask;
        }

        public void Dispose()
        {
            CryptoParser = null;
            GC.Collect();
            CancellationTokenSource.Cancel();
            PeriodicTask.Dispose();
            CancellationTokenSource.Dispose();
        }

        ~CryptoParserScheduledTask() 
        {
            Dispose();
        }
    }
}
