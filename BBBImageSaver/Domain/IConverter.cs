using System.Collections.Generic;
namespace BBBImageSaver.Domain
{
    public interface IConverter
    {
        void Convert(IEnumerable<string> paths, string pathToSave);
    }
}
