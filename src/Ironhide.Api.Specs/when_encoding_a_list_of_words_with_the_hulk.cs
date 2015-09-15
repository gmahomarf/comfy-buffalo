using FluentAssertions;
using Ironhide.Api.Host;
using Ironhide.Api.Host.Algorithms;
using Machine.Specifications;

namespace Ironhide.Api.Specs
{
    public class when_encoding_a_list_of_words_with_the_hulk
    {
        static IEncodingAlgorithm _encodingAlgorithm;
        static string _result;
        static string _expectedEncodedString;
        static string[] _listOfWords;

        Establish context =
            () =>
            {
                _encodingAlgorithm = new TheIncredibleHulkAlgorithm(new VowelShifter());
                _listOfWords = new[] { "superman", "don", "byron", "sommardahl" };
                _expectedEncodedString = "spuremna*smomradhal*dno*bryno";
            };

        Because of =
            () => _result = _encodingAlgorithm.Encode(_listOfWords);

        It should_return_the_expected_encoded_string =
            () => _result.Should().Be(_expectedEncodedString);
    }
}