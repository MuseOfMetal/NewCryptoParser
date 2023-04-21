using NewCryptoParser.Models;

namespace NewCryptoParser.Abstract;

public interface IParserManager
{
    void AddParser(string code, string name);
    CryptoParserScheduledTask? GetParser(string name);
    void UpdateParser(string code, string name);
    void RemoveParser(string name);
}
