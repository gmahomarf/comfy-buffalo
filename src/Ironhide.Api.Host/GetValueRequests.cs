using System;
using System.Collections.Generic;

namespace Ironhide.Api.Host
{
    public class GetValueRequests
    {
        public Guid Guid { get; private set; }
        public List<string> Words { get; private set; }
        public double StartingFibonacciNumber { get; private set; }

        public GetValueRequests(Guid guid, List<string> words, double startingFibonacciNumber)
        {
            Guid = guid;
            Words = words;
            StartingFibonacciNumber = startingFibonacciNumber;
        }
    }
}