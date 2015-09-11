using System;

namespace Ironhide.Api.Host
{
    public interface IEncodingAlgorithm
    {
        string Encode(Int64 startingNumber, string[] words);
    }
}