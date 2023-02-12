using Loxifi;
using Penguin.Extensions.String;
using Penguin.Web.Abstractions.Interfaces;

namespace Penguin.Web
{
    public class HttpServerResponse : HttpInteractionBase, IHttpServerResponse
    {
        public override string HttpVersion => HeaderLine.To(" ");

        public int StatusCode => int.Parse(HeaderLine.From(" ").To(" "));

        public string StatusMessage => HeaderLine.From(" ").From(" ");

        public HttpServerResponse(byte[] raw) : base(raw)
        {
        }
    }
}