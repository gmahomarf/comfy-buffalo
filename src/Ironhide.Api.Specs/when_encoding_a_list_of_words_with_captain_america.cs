using FluentAssertions;
using Ironhide.Api.Host;
using Ironhide.Api.Host.Algorithms;
using Machine.Specifications;

namespace Ironhide.Api.Specs
{
    public class when_encoding_a_list_of_words_with_captain_america
    {
        static IEncodingAlgorithm _encodingAlgorithm;
        static string _result;
        static string _expectedEncodedString;
        static string[] _listOfWords;
        
        Establish context =
            () =>
            {
                _encodingAlgorithm = new CaptainAmericaAlgorithm(3, new VowelEncoder(new FibonacciGenerator()),
                    new VowelShifter(), new AsciiValueDelimiterAdder());
                _listOfWords = new[] { "superman", "don", "byron", "sommardahl" };
                _expectedEncodedString = "sp3r5mn898sm13mr21dh34l115dn55115br89n144100";
            };

        Because of =
            () => _result = _encodingAlgorithm.Encode(_listOfWords);

        It should_return_the_expected_encoded_string =
            () => _result.Should().Be(_expectedEncodedString);
    }
}