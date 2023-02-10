using Penguin.Extensions.String;
using Penguin.SystemExtensions.Abstractions.Interfaces;
using Penguin.SystemExtensions.Collections;
using Penguin.Web.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penguin.Web
{
    public class HttpHeaderCollection : IListCollection<HttpHeader>, IConvertible<string>, IDictionary<string, string>
    {
        private readonly CustomHttpHeaderFactory HeaderFactory = new(new List<Type>()
        {
            typeof(Connection),
            typeof(Cookie),
            typeof(ContentLength),
            typeof(IfModifiedSince)
        });

        public ICollection<string> Keys => BackingList.Select(h => h.Key).ToList();

        public ICollection<string> Values => BackingList.Select(h => h.Value).ToList();

        public override HttpHeader this[int index]
        {
            get => BackingList[index];
            set => AddOrUpdate(value);
        }

        public string this[string key]
        {
            get => Find(key)?.Value;
            set => AddOrUpdate(key, value);
        }

        public override void Add(HttpHeader item)
        {
            _ = AddOrUpdate(item);
        }

        public void Add(string key, string value)
        {
            _ = AddOrUpdate(key, value);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            _ = AddOrUpdate(item.Key, item.Value);
        }

        public bool AddOrUpdate(string key, string value)
        {
            HttpHeader httpHeader = Find(key);

            if (httpHeader is null)
            {
                httpHeader = HeaderFactory.GetHeader(key, value);
                BackingList.Add(httpHeader);
                return false;
            }
            else
            {
                httpHeader.Value = value;
                return true;
            }
        }

        public bool AddOrUpdate(HttpHeader header)
        {
            return header is null ? throw new ArgumentNullException(nameof(header)) : AddOrUpdate(header.Key, header.Value);
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            string v = this[item.Key];

            return v == item.Value;
        }

        public bool ContainsKey(string key)
        {
            return BackingList.Any(h => string.Equals(h.Key, key, StringComparison.OrdinalIgnoreCase));
        }

        string IConvertible<string>.Convert()
        {
            return ToString();
        }

        void IConvertible<string>.Convert(string fromT)
        {
            string source = fromT.Trim();
            //Convert from old format
            if (source.StartsWith("[", StringComparison.OrdinalIgnoreCase) && source.EndsWith("]", StringComparison.OrdinalIgnoreCase))
            {
                source = source.Trim('[').Trim(']');

                if (string.IsNullOrWhiteSpace(source))
                {
                    return;
                }

                source += ','; //Comma as temp solution for now to trick it into adding the last item

                int bracket = 0;
                StringBuilder sb = new();

                for (int i = 0; i < source.Length; i++)
                {
                    char c = source[i];

                    switch (c)
                    {
                        case '{':
                            bracket++;
                            break;

                        case '}':
                            bracket--;
                            break;

                        case ',':
                            switch (bracket)
                            {
                                case 0:
                                    string header = sb.ToString();
                                    _ = sb.Clear();

                                    List<string> vals = header.SplitQuotedString().Select(v => v.Trim()).ToList();

                                    string Key = null;
                                    string Value = null;

                                    Key = vals.Where(v => v.StartsWith("Key", StringComparison.OrdinalIgnoreCase)).Single().From(":").Trim();
                                    Value = vals.Where(v => v.StartsWith("Value", StringComparison.OrdinalIgnoreCase)).Single().From(":");

                                    int vLength = 0;

                                    List<char> trim = new()
                                    {
                                        '\r',
                                        '\n',
                                        '[',
                                        ']',
                                        ',',
                                        ' '
                                    };

                                    do
                                    {
                                        vLength = Value.Length;

                                        foreach (char ct in trim)
                                        {
                                            Value = Value.Trim(ct);
                                        }
                                    } while (vLength != Value.Length);

                                    _ = AddOrUpdate(Key, Value);

                                    break;

                                default:
                                    _ = sb.Append(c);
                                    break;
                            }
                            break;

                        default:
                            _ = sb.Append(c);
                            break;
                    }
                }
            }
            else
            {
                foreach (string header in source.Split('\n').Select(h => h.Trim()))
                {
                    HttpHeader placeholder = new();

                    (placeholder as IConvertible<string>).Convert(header);

                    if (placeholder.Key != null)
                    {
                        _ = AddOrUpdate(placeholder);
                    }
                }
            }
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public HttpHeader Find(string key)
        {
            return BackingList.SingleOrDefault(h => h.Key == key);
        }

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
        {
            foreach (HttpHeader header in BackingList)
            {
                yield return new KeyValuePair<string, string>(header.Key, header.Value);
            }
        }

        public int IndexOf(string key)
        {
            return IndexOf(Find(key));
        }

        public override void Insert(int index, HttpHeader item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            BackingList.Insert(index, HeaderFactory.GetHeader(item.Key, item.Value));
        }

        public bool Remove(string key)
        {
            bool val = ContainsKey(key);

            BackingList = BackingList.Where(h => !string.Equals(h.Key, key, StringComparison.OrdinalIgnoreCase)).ToList();

            return val;
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            HttpHeader h = Find(item.Key);

            if (h?.Value == item.Value)
            {
                _ = BackingList.Remove(h);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return string.Join("\r\n", BackingList);
        }

        public bool TryGetValue(string key, out string value)
        {
            value = this[key];
            return value is not null;
        }
    }
}