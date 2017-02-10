using System.Net;

namespace FireSharpSimple.Response
{
    public class SetResponse : FirebaseResponse
    {
        public SetResponse(string body, HttpStatusCode statusCode)
            : base(body, statusCode)
        {
        }
    }
}
