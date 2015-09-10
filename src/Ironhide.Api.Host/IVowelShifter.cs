using System.Collections.Generic;

namespace Ironhide.Api.Host
{
    public interface IVowelShifter
    {
        IEnumerable<string> ShiftRight(IEnumerable<string> words, int numberToShift);
    }
}