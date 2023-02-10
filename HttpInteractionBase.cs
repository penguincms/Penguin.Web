using Penguin.SystemExtensions.Abstractions.Interfaces;
using Penguin.Web.Abstractions.Interfaces;
using Penguin.Web.Headers;
using Penguin.Web.Readers;
using System.Collections.Generic;
using System.Text;

namespace Penguin.Web
{
    public abstract class HttpInteractionBase : IConvertible<string>, IHttpInteractionBase
    {
        public string BodyText { get; set; }
        public HttpHeaderCollection Headers { get; protected set; } = new HttpHeaderCollection();
        public abstract string HttpVersion { get; }
        public string ContentType => Headers["Content-Type"];
        protected string HeaderLine { get; set; }

        protected byte[] Raw { get; set; }
        IDictionary<string, string> IHttpInteractionBase.Headers => Headers;

        protected HttpInteractionBase(byte[] raw)
        {
            Fill(raw);
        }

        protected HttpInteractionBase()
        {
        }

        public virtual string Convert()
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in Raw)
            {
                sb.Append(b);
            }
            return sb.ToString();
        }

        public virtual void Convert(string fromT)
        {
        }

        protected virtual void Fill(byte[] raw)
        {
            Raw = raw;

            HttpReader reader = new HttpReader(raw);

            foreach (HttpHeader header in reader.Headers)
            {
                Headers.Add(header);
            }

            HeaderLine = reader.HeaderLine;
            BodyText = reader.BodyText;
        }
    }
}