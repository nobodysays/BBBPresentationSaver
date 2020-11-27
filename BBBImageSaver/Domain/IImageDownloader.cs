using System;
namespace BBBImageSaver.Domain
{
    public interface IImageDownloader
    {
        event Action OnBeginDownload;
        event Action OnEndDownload;
        string Download(string url, string savePath, string name);
    }
}
