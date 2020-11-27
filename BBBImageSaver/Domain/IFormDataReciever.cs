namespace BBBImageSaver.Domain
{
    public interface IFormDataReciever
    {
        void OnRecieved(string url, string pathToSave);
    }
}
