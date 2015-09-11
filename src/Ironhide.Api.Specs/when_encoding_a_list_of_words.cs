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
                    new VowelShifter(), new CapsAlternator(), new AsciiValueDelimiterAdder(), new WordSplitter(new StaticDictionary()));
                _listOfWords = new[] {"superman", "don", "byron", "sommardahl"};
                _firstFibonacciNumber = 3;
                _expectedEncodedString = string.Join("", new[] { "sP89r144", "98", "Sm21Mr34Dh55L", "115", "Mn13", "83", "Dn8", "77", "bR3n5", "68" });
            };

        Because of =
            () => _result = _encodingAlgorithm.Encode(_firstFibonacciNumber, _listOfWords);

        It should_return_the_expected_encoded_string =
            () => _result.Should().Be(_expectedEncodedString);
    }
}