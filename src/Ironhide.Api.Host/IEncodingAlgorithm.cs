using System;

namespace Ironhide.Api.Host
{
    public interface IEncodingAlgorithm
    {
        string Encode(string[] words);
    }
}