using FluentAssertions;
using Ironhide.Api.Host;
using Ironhide.Api.Host.Algorithms;
using Machine.Specifications;

namespace Ironhide.Api.Specs
{
    public class when_encoding_a_list_of_words_with_thor
    {
        static IEncodingAlgorithm _encodingAlgorithm;
        static string _result;
        static string _expectedEncodedString;
        static string[] _listOfWords;
        const int FirstFibonacciNumber = 3;

        Establish context =
            () =>
            {
                _encodingAlgorithm = new ThorAlgorithm(FirstFibonacciNumber, new VowelEncoder(new FibonacciGenerator()),
                    new CapsAlternator(), new WordSplitter(new StaticDictionary()));
                _listOfWords = new[] {"superman", "don", "byron", "sommardahl"};
                _expectedEncodedString = "b3r5n*D8N*m13n*S21Mm34rD55Hl*S89P144R";
            };

        Because of =
            () => _result = _encodingAlgorithm.Encode(_listOfWords);

        It should_return_the_expected_encoded_string =
            () => _result.Should().Be(_expectedEncodedString);
    }
}