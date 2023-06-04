using CryptoParserSdk.Models;
using NewCryptoParser.Models;

namespace NewCryptoParser.Abstract
{
    public interface INewCryptocurrencyProjectManager
    {
        void AddNewProjects(string parserName, List<ParsingResult> projects);
        IEnumerable<NewCryptocurrencyProject> GetProjects();
    }
}
