using Penguin.Extensions.String;
using Penguin.Web.Abstractions.Interfaces;
using System;

namespace Penguin.Web
{
    public class HttpServerRequest : HttpInteractionBase, IHttpServerRequest
    {
        public string HttpString => HeaderLine;

        public override string HttpVersion => HttpString.FromLast(" ");

        public string Method => HeaderLine.To(" ");

        public string Path { get; set; }

        public string Url
        {
            get
            {
                string toReturn = string.Empty;

                string origin = Headers["origin"];

                if (!(origin is null))
                {
                    toReturn += origin;
                }

                toReturn += Path;

                return toReturn;
            }
            set
            {
                Uri uri = new Uri(value);

                string origin = uri.GetLeftPart(UriPartial.Authority);

                if (!string.IsNullOrWhiteSpace(origin))
                {
                    Headers["origin"] = origin;
                }

                Path = uri.PathAndQuery;
            }
        }

        public HttpServerRequest(byte[] raw) : base(raw)
        {
        }

        protected override void Fill(byte[] raw)
        {
            base.Fill(raw);

            Url = HttpString.From(" ").ToLast(" ");
        }
    }
}