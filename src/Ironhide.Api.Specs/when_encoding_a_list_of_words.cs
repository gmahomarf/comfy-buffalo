using FluentAssertions;
using Ironhide.Api.Host;
using Machine.Specifications;

namespace Ironhide.Api.Specs
{
    public class when_encoding_a_list_of_words
    {
        static IEncodingAlgorithm _encodingAlgorithm;
        static string _result;
        static string _expectedEncodedString;
        static string[] _listOfWords;
        static int _firstFibonacciNumber;

        Establish context =
            () =>
            {
                _encodingAlgorithm = new SuperSecretEncodingAlgorithm(new VowelEncoder(new FibonacciGenerator()),
                    new VowelShifter(), new CapsAlternator(), new AsciiValueDelimiterAdder());
                _listOfWords = new[] {"don", "byron", "sommardahl"};
                _firstFibonacciNumber = 3;
                _expectedEncodedString = string.Join("", new[] { "Sm13Mr21Dh34L", "98", "Dn8", "83", "bR3n5", "68" });
            };

        Because of =
            () => _result = _encodingAlgorithm.Encode(_firstFibonacciNumber, _listOfWords);

        It should_return_the_expected_encoded_string =
            () => _result.Should().Be(_expectedEncodedString);
    }
}