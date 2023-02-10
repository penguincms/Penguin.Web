namespace Penguin.Web
{
    public class HttpServerInteraction
    {
        public HttpServerRequest Request { get; set; }

        public HttpServerResponse Response { get; set; }

        public override string ToString()
        {
            return Request?.Url?.ToString();
        }
    }
}