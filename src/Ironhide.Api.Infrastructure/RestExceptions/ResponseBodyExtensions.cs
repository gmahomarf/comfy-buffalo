using System.Text;
using Nancy;
using Newtonsoft.Json;

namespace Ironhide.Api.Infrastructure.RestExceptions
{
    public static class ResponseBodyExtensions
    {
        public static Response WithBody(this Response response, object body)
        {
            string str = JsonConvert.SerializeObject(body);
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            response.Contents = s => s.Write(bytes, 0, bytes.Length);
            return response;
        }
    }
}