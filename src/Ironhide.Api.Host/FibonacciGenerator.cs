using System;
using System.Collections.Generic;

namespace Ironhide.Api.Host
{
    public class FibonacciGenerator : INumberSeriesGenerator
    {
        public IEnumerable<double> Generate(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return Fib(i);
            }
        }

        public Int64 GetNext(double previousNumber)
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                long num = Fib(i);
                if (Convert.ToInt64(previousNumber) == Convert.ToInt64(num))
                {
                    return Fib(i + 1);
                }
            }
            throw new Exception("Previous number was not found in the fibonacci sequence.");
        }

        static Int64 Fib(double n)
        {
            double sqrt5 = Math.Sqrt(5);
            double p1 = (1 + sqrt5)/2;
            double p2 = -1*(p1 - 1);

            double n1 = Math.Pow(p1, n + 1);
            double n2 = Math.Pow(p2, n + 1);
            return Convert.ToInt64((n1 - n2)/sqrt5);
        }
    }
}