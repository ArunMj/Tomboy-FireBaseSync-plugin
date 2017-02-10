using System.Net;

namespace FireSharpSimple
{
    public class SetResponse : FirebaseResponse
    {
        public SetResponse(string body, HttpStatusCode statusCode)
            : base(body, statusCode)
        {
        }
    }
}
