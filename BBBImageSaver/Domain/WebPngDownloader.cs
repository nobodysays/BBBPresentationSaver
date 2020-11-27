using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Svg;
namespace BBBImageSaver.Domain
{
    public class WebPngDownloader : IImageDownloader
    {
        public event Action OnBeginDownload;
        public event Action OnEndDownload;

        public string Download(string url, string savePath, string name)
        {
            WebClient client = new WebClient();
            OnBeginDownload?.Invoke();
            var svgPath = $"{Environment.CurrentDirectory}/{name}.svg";
            var pngPath = $"{Environment.CurrentDirectory}/{name}.png";
            client.DownloadFile(url, svgPath);


            var svgDocument = SvgDocument.Open(svgPath);
            var bitmap = svgDocument.Draw();
            bitmap.Save(pngPath);
            System.IO.File.Delete(svgPath);
            OnEndDownload?.Invoke();
            return pngPath;
        }
    }
}
