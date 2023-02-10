using System.Collections;
using System.Collections.Generic;

namespace Penguin.Web
{
    public class HttpServerCapture : IList<HttpServerInteraction>
    {
        private readonly List<HttpServerInteraction> Interactions = new();

        public int Count => ((IList<HttpServerInteraction>)Interactions).Count;

        public bool IsReadOnly => ((IList<HttpServerInteraction>)Interactions).IsReadOnly;

        public HttpServerInteraction this[int index]
        {
            get
            {
                while (index >= Interactions.Count)
                {
                    Interactions.Add(new HttpServerInteraction());
                }

                return ((IList<HttpServerInteraction>)Interactions)[index];
            }
            set
            {
                while (index >= Interactions.Count)
                {
                    Interactions.Add(new HttpServerInteraction());
                }
                ((IList<HttpServerInteraction>)Interactions)[index] = value;
            }
        }

        public void Add(HttpServerInteraction item)
        {
            ((IList<HttpServerInteraction>)Interactions).Add(item);
        }

        public void Clear()
        {
            ((IList<HttpServerInteraction>)Interactions).Clear();
        }

        public bool Contains(HttpServerInteraction item)
        {
            return ((IList<HttpServerInteraction>)Interactions).Contains(item);
        }

        public void CopyTo(HttpServerInteraction[] array, int arrayIndex)
        {
            ((IList<HttpServerInteraction>)Interactions).CopyTo(array, arrayIndex);
        }

        public IEnumerator<HttpServerInteraction> GetEnumerator()
        {
            return ((IList<HttpServerInteraction>)Interactions).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<HttpServerInteraction>)Interactions).GetEnumerator();
        }

        public int IndexOf(HttpServerInteraction item)
        {
            return ((IList<HttpServerInteraction>)Interactions).IndexOf(item);
        }

        public void Insert(int index, HttpServerInteraction item)
        {
            ((IList<HttpServerInteraction>)Interactions).Insert(index, item);
        }

        public bool Remove(HttpServerInteraction item)
        {
            return ((IList<HttpServerInteraction>)Interactions).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<HttpServerInteraction>)Interactions).RemoveAt(index);
        }
    }
}