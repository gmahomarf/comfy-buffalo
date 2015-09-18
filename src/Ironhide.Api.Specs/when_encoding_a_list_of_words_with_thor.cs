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
                    new ConsonantCapsAlternator(), new WordSplitter(new StaticDictionary()));
                _listOfWords = new[] {"superman", "don", "byron", "sommardahl"};
                _expectedEncodedString = "b3R5n*D8n*M13n*S21mM34rD55hL*s89P144r";
            };

        Because of =
            () => _result = _encodingAlgorithm.Encode(_listOfWords);

        It should_return_the_expected_encoded_string =
            () => _result.Should().Be(_expectedEncodedString);
    }
}