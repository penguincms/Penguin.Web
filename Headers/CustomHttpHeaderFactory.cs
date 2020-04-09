using System;
using System.Collections.Generic;
using System.Linq;

namespace Penguin.Web.Headers
{
    public class CustomHttpHeaderFactory
    {
        private Dictionary<string, Type> CustomTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public CustomHttpHeaderFactory(IEnumerable<Type> CustomHttpHeaders)
        {
            foreach (Type t in CustomHttpHeaders.Distinct())
            {
                if (!typeof(HttpHeader).IsAssignableFrom(t))
                {
                    throw new ArgumentException($"Type {t} does not derive from HttpHeader.");
                }

                CustomTypes.Add(t.Name, t);
            }
        }

        public HttpHeader GetHeader(string Key, string Value)
        {
            if (Key is null)
            {
                return new HttpHeader();
            }

            HttpHeader header;

            if (CustomTypes.TryGetValue(Key.Replace("-", "").Trim(), out Type t))
            {
                header = Activator.CreateInstance(t) as HttpHeader;
            }
            else
            {
                header = new HttpHeader();
            }

            header.Key = Key;
            header.Value = Value;

            return header;
        }
    }
}