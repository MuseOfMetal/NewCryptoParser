using CryptoParserSdk;

namespace NewCryptoParser.Abstract
{
    public interface IParserManager
    {
        void AddParser(string code, string name);
        ICryptoParser GetParser(string name);
        void UpdateParser(string code, string name);
        void RemoveParser(string name);
    }
}
