

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironhide.Api.Host
{
    public class FibonacciGenerator : INumberSeriesGenerator
    {
        static double Fib(double n)
        {
            double sqrt5 = Math.Sqrt(5);
            double p1 = (1 + sqrt5) / 2;
            double p2 = -1 * (p1 - 1);

            double n1 = Math.Pow(p1, n + 1);
            double n2 = Math.Pow(p2, n + 1);
            return Convert.ToDouble((n1 - n2)/sqrt5);
        }

        public IEnumerable<double> Generate(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return Fib(i);
            }            
        }

        public double GetNext(double previousNumber)
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                var num = Fib(i);
                if (Convert.ToInt64(previousNumber) == Convert.ToInt64(num))
                {
                    return Fib(i + 1);
                }
            }
            throw new Exception("Previous number was not found in the fibonacci sequence.");            
        }

        //from: http://www.dotnetperls.com/fibonacci
        public static double Fibonacci(int n)
        {
            double a = 0;
            double b = 1;
            // In N steps compute Fibonacci sequence iteratively.
            for (int i = 0; i < n; i++)
            {
                double temp = a;
                a = b;
                b = temp + b;
            }
            return a;
        }
    }
}