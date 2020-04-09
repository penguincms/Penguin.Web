using Penguin.Extensions.Strings;
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
        private readonly CustomHttpHeaderFactory HeaderFactory = new CustomHttpHeaderFactory(new List<Type>()
        {
            typeof(Connection),
            typeof(Cookie),
            typeof(ContentLength),
            typeof(IfModifiedSince)
        });

        public ICollection<string> Keys => this.BackingList.Select(h => h.Key).ToList();

        public ICollection<string> Values => this.BackingList.Select(h => h.Value).ToList();

        public override HttpHeader this[int index]
        {
            get => ((IList<HttpHeader>)this.BackingList)[index];
            set => this.AddOrUpdate(value);
        }

        public string this[string key]
        {
            get => Find(key)?.Value;
            set => AddOrUpdate(key, value);
        }

        public override void Add(HttpHeader item)
        {
            this.AddOrUpdate(item);
        }

        public void Add(string key, string value)
        {
            this.AddOrUpdate(key, value);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            AddOrUpdate(item.Key, item.Value);
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

        public bool AddOrUpdate(HttpHeader header) => AddOrUpdate(header.Key, header.Value);

        public bool Contains(KeyValuePair<string, string> item)
        {
            string v = this[item.Key];

            return v == item.Value;
        }

        public bool ContainsKey(string key)
        {
            return this.BackingList.Any(h => string.Equals(h.Key, key, StringComparison.OrdinalIgnoreCase));
        }

        string IConvertible<string>.Convert() => string.Join("\r\n", BackingList);

        void IConvertible<string>.Convert(string fromT)
        {
            string source = fromT.Trim();
            //Convert from old format
            if (source.StartsWith("[") && source.EndsWith("]"))
            {
                source = source.Trim('[').Trim(']');

                if (string.IsNullOrWhiteSpace(source))
                {
                    return;
                }

                source += ','; //Comma as temp solution for now to trick it into adding the last item

                int bracket = 0;
                StringBuilder sb = new StringBuilder();

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
                                    sb.Clear();

                                    List<string> vals = header.SplitQuotedString().Select(v => v.Trim()).ToList();

                                    string Key = null;
                                    string Value = null;

                                    Key = vals.Where(v => v.StartsWith("Key", StringComparison.OrdinalIgnoreCase)).Single().From(":").Trim();
                                    Value = vals.Where(v => v.StartsWith("Value", StringComparison.OrdinalIgnoreCase)).Single().From(":");

                                    int vLength = 0;

                                    List<char> trim = new List<char>()
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

                                    AddOrUpdate(Key, Value);

                                    break;

                                default:
                                    sb.Append(c);
                                    break;
                            }
                            break;

                        default:
                            sb.Append(c);
                            break;
                    }
                }
            }
            else
            {
                foreach (string header in source.Split('\n').Select(h => h.Trim()))
                {
                    HttpHeader placeholder = new HttpHeader();

                    (placeholder as IConvertible<string>).Convert(header);

                    if (placeholder.Key != null)
                    {
                        AddOrUpdate(placeholder);
                    }
                }
            }
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public HttpHeader Find(string key) => this.BackingList.SingleOrDefault(h => h.Key == key);

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
        {
            foreach (HttpHeader header in this.BackingList)
            {
                yield return new KeyValuePair<string, string>(header.Key, header.Value);
            }
        }

        public int IndexOf(string key)
        {
            return this.IndexOf(this.Find(key));
        }

        public override void Insert(int index, HttpHeader item)
        {
            this.BackingList.Insert(index, HeaderFactory.GetHeader(item.Key, item.Value));
        }

        public bool Remove(string key)
        {
            bool val = this.ContainsKey(key);

            this.BackingList = this.BackingList.Where(h => !string.Equals(h.Key, key, StringComparison.OrdinalIgnoreCase)).ToList();

            return val;
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            HttpHeader h = this.Find(item.Key);

            if (h?.Value == item.Value)
            {
                BackingList.Remove(h);
                return true;
            }
            return false;
        }

        public bool TryGetValue(string key, out string value)
        {
            value = this[key];
            return !(value is null);
        }
    }
}