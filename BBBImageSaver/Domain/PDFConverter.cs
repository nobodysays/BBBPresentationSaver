using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;
using Image = iTextSharp.text.Image;

namespace BBBImageSaver.Domain
{
    public class PDFConverter : IConverter
    {
        public void Convert(IEnumerable<string> paths, string pathToSave)
        {
            Document document = new Document();
            using (var stream = new FileStream(pathToSave, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter.GetInstance(document, stream);
                document.Open();
                foreach (var item in paths)
                {
                    using (var imageStream = new FileStream(item, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        var image = Image.GetInstance(imageStream);
                        image.ScaleToFit(document.PageSize);
                        image.SetAbsolutePosition(0, image.AbsoluteY);
                        document.Add(image);
                    }
                }
                document.Close();
            }
        }
    }
}
