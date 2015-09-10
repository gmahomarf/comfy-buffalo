using System.Collections.Generic;

namespace Ironhide.Api.Host
{
    public interface INumberSeriesGenerator
    {
        IEnumerable<double> Generate(int count);
        double GetNext(double previousNumber);
    }
}