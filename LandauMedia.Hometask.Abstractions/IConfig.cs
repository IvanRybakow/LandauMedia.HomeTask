namespace LandauMedia.Hometask.Abstractions
{
    public interface IConfig
    {
        string PathToFilesFolder { get; }
        string PathToCacheFolder { get; }
    }
}