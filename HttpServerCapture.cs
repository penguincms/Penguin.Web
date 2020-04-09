using System.Collections;
using System.Collections.Generic;

namespace Penguin.Web
{
    public class HttpServerCapture : IList<HttpServerInteraction>
    {
        private List<HttpServerInteraction> Interactions = new List<HttpServerInteraction>();

        public int Count => ((IList<HttpServerInteraction>)this.Interactions).Count;

        public bool IsReadOnly => ((IList<HttpServerInteraction>)this.Interactions).IsReadOnly;

        public HttpServerInteraction this[int index]
        {
            get
            {
                while (index >= Interactions.Count)
                {
                    Interactions.Add(new HttpServerInteraction());
                }

                return ((IList<HttpServerInteraction>)this.Interactions)[index];
            }
            set
            {
                while (index >= Interactions.Count)
                {
                    Interactions.Add(new HttpServerInteraction());
                }
                ((IList<HttpServerInteraction>)this.Interactions)[index] = value;
            }
        }

        public void Add(HttpServerInteraction item)
        {
            ((IList<HttpServerInteraction>)this.Interactions).Add(item);
        }

        public void Clear()
        {
            ((IList<HttpServerInteraction>)this.Interactions).Clear();
        }

        public bool Contains(HttpServerInteraction item)
        {
            return ((IList<HttpServerInteraction>)this.Interactions).Contains(item);
        }

        public void CopyTo(HttpServerInteraction[] array, int arrayIndex)
        {
            ((IList<HttpServerInteraction>)this.Interactions).CopyTo(array, arrayIndex);
        }

        public IEnumerator<HttpServerInteraction> GetEnumerator()
        {
            return ((IList<HttpServerInteraction>)this.Interactions).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<HttpServerInteraction>)this.Interactions).GetEnumerator();
        }

        public int IndexOf(HttpServerInteraction item)
        {
            return ((IList<HttpServerInteraction>)this.Interactions).IndexOf(item);
        }

        public void Insert(int index, HttpServerInteraction item)
        {
            ((IList<HttpServerInteraction>)this.Interactions).Insert(index, item);
        }

        public bool Remove(HttpServerInteraction item)
        {
            return ((IList<HttpServerInteraction>)this.Interactions).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<HttpServerInteraction>)this.Interactions).RemoveAt(index);
        }
    }
}