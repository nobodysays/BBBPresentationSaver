using System;
namespace BBBImageSaver.Domain
{
    public interface IFormDataSender
    {
        event Action<string, string> OnSent;
    }
}
