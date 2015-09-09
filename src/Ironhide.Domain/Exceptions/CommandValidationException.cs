using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironhide.Users.Domain.Exceptions
{
    public class CommandValidationException : Exception
    {
        public CommandValidationException(IEnumerable<ValidationFailure> failures)
            : base(
                string.Format("The command was issued with the following properties in invalid state: {0}",
                    string.Join(",", (failures ?? new List<ValidationFailure>()).Select(x => string.Format("{0} {1}", x.Property, x.FailureType)))))
        {
            ValidationFailures = failures;
        }

        public IEnumerable<ValidationFailure> ValidationFailures { get; private set; }
    }
}