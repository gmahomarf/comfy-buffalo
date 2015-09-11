using System;
using System.Collections.Generic;

namespace Ironhide.Api.Host
{
    public interface INumberSeriesGenerator
    {
        IEnumerable<double> Generate(int count);
        Int64 GetNext(double previousNumber);
    }
}