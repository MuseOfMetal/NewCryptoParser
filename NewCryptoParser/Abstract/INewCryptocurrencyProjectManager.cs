using CryptoParserSdk.Models;
using NewCryptoParser.Models;

namespace NewCryptoParser.Abstract
{
    public interface INewCryptocurrencyProjectManager
    {
        void AddNewProjects(string parserName, List<ParsingResult> projects);
        NewCryptocurrencyProject? GetLatestProject();
        NewCryptocurrencyProject? GetProjectById(int id);
        List<NewCryptocurrencyProject>? GetProjectsByIdRange(int startId, int endId);
    }
}
