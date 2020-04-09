using Penguin.Extensions.Strings;

namespace Penguin.Web
{
    public class HttpServerResponse : HttpInteractionBase
    {
        public override string HttpVersion => HeaderLine.To(" ");

        public int StatusCode => int.Parse(HeaderLine.From(" ").To(" "));

        public string StatusMessage => HeaderLine.From(" ").From(" ");

        public HttpServerResponse(byte[] raw) : base(raw)
        {
        }
    }
}