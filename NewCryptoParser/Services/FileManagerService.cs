namespace NewCryptoParser.Services
{
    public class FileManagerService
    {
        private FileSystemWatcher _watcher;

        private FileManagerService()
        {
            _watcher = new FileSystemWatcher();
            _watcher.Created += _file_created;
            _watcher.Deleted += _file_deleted;
        }

        private void _file_deleted(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _file_created(object sender, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
