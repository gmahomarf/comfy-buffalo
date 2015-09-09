namespace Ironhide.Api.Modules.Login
{
    public class SuccessfulLoginResponse<T>
    {
        public SuccessfulLoginResponse()
        {

        }

        public SuccessfulLoginResponse(T token)
        {
            Token = token;
        }

        public T Token { get; set; }
    }
}