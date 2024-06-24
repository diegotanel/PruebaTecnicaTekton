namespace Shared.External
{
    public interface IApiExterna
    {
        Task<string> GetApiDataAsync(string url);
    }
}
