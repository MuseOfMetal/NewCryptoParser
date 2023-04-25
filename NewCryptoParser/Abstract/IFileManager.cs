namespace NewCryptoParser.Abstract
{
    public interface IFileManager
    {
        public string GetWorkPath();
        public string[] GetFiles();
        public void AddFile();
        public void RemoveFile();

    }
}
