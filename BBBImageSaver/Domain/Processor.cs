using System;
using System.Collections.Generic;
using System.Net;
namespace BBBImageSaver.Domain
{
    public class Processor : IFormDataReciever
    {

        private IImageDownloader _downloader;
        private IConverter _converter;

        public Processor(IImageDownloader downloader, IConverter converter)
        {
            _downloader = downloader;
            _converter = converter;
        }

        public event Action<uint> OnCountOfPagesGot;

        public void OnRecieved(string url, string pathToSave)
        {
#if RELEASE
            var count = GetCountOfPages(url);
#endif
#if DEBUG
            uint count = 5;
#endif
            OnCountOfPagesGot?.Invoke(count);

            var imagesPath = new List<string>();

            for (uint i = 1; i < count; i++)
            {
                 imagesPath.Add(_downloader.Download($"{url}{i}", pathToSave, $"page_{i}"));
            }

            _converter.Convert(imagesPath, pathToSave);
        }



        private uint GetCountOfPages(string url)
        {
            uint count = 1;

            //делаем запрос серверу пока не будет 404 (конец презентации)
            while (true)
            {
                WebRequest webr = WebRequest.Create($"{url}{count}");

                try
                {
                    var resp = (HttpWebResponse)webr.GetResponse();
                }
                catch (WebException e)
                {
                    break;
                }
                count++;
            }
            return count;
        }
    }
}
