namespace Ironhide.Api.Host
{
    public interface IEncodingAlgorithm
    {
        string Encode(double startingNumber, string[] words);
    }
}