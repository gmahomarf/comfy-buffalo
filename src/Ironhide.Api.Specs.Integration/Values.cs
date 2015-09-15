using System;
using System.Collections.Generic;
using Ironhide.Api.Host;

namespace Ironhide.Api.Specs.Integration
{
    public class Values
    {
        public List<string> Words { get; set; }
        public Int64 StartingFibonacciNumber { get; set; }
        public AlgorithmName Algorithm { get; set; }
    }
}