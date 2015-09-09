namespace Ironhide.Users.Domain
{
    public class ValidationFailure
    {
        public ValidationFailure(string property, ValidationFailureType failureType)
        {
            Property = property;
            FailureType = failureType;
        }

        public string Property { get; private set; }
        public ValidationFailureType FailureType { get; private set; }
    }
}