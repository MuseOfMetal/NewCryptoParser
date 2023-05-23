using CryptoParserSdk;

namespace NewCryptoParser.Models
{
    public class CryptoParserScheduledTask : IDisposable
    {
        public CryptoParserAbstract CryptoParser { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public Task PeriodicTask { get; set; }

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
