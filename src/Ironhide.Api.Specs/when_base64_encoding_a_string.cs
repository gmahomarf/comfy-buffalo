using FluentAssertions;
using Ironhide.Api.Host;
using Machine.Specifications;

namespace Ironhide.Api.Specs
{
    public class when_base64_encoding_a_string
    {
        static Base64StringEncoder _encoder;
        static string _result;

        Establish context =
            () => { _encoder = new Base64StringEncoder(); };

        Because of =
            () => _result = _encoder.Encode("hello world");

        It should_return_the_same_value_in_base_64 =
            () => _result.Should().Be("aGVsbG8gd29ybGQ=");
    }
}