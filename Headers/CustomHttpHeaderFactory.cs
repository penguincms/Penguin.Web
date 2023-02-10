using System;
using System.Collections.Generic;
using System.Linq;

namespace Penguin.Web.Headers
{
    public class CustomHttpHeaderFactory
    {
        private readonly Dictionary<string, Type> CustomTypes = new(StringComparer.OrdinalIgnoreCase);

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

            HttpHeader header = CustomTypes.TryGetValue(Key.Replace("-", "").Trim(), out Type t)
                ? Activator.CreateInstance(t) as HttpHeader
                : new HttpHeader();
            header.Key = Key;
            header.Value = Value;

            return header;
        }
    }
}