using System.Collections.Generic;
using FluentAssertions;
using Ironhide.Api.Host;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Ironhide.Api.Specs
{
    public class when_encoding_vowels
    {
        static readonly INumberSeriesGenerator NumberSeriesGenerator = Mock.Of<INumberSeriesGenerator>();
        static readonly VowelEncoder VowelEncoder = new VowelEncoder(NumberSeriesGenerator);
        static IEnumerable<string> _result;

        Establish context =
            () =>
            {
                Mock.Get(NumberSeriesGenerator).Setup(x => x.GetNext(3)).Returns(4);
                Mock.Get(NumberSeriesGenerator).Setup(x => x.GetNext(4)).Returns(5);
                Mock.Get(NumberSeriesGenerator).Setup(x => x.GetNext(5)).Returns(6);
                Mock.Get(NumberSeriesGenerator).Setup(x => x.GetNext(6)).Returns(7);
            };

        Because of =
            () => _result = VowelEncoder.Encode(3, new[] {"byron", "sommardahl"});

        It should_return_the_expected_words_with_encoded_vowels =
            () => _result.Should().Equal(new[] {"b3r4n", "s5mm6rd7hl"});
    }
}