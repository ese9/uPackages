namespace NineGames.AddressableExtensions
{
    public interface IDownloadProgress
    {
        void Report(long downloadedBytes, long totalBytes);
    }
}