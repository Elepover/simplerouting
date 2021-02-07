using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleRouting.Routing
{
    public class RouterEnumerator<TIncoming> : IEnumerator<TIncoming>
    {
        public RouterEnumerator(IEnumerable<TIncoming> incomingObjects)
        {
            _incomingObjects = incomingObjects;
        }
        
        private readonly IEnumerable<TIncoming> _incomingObjects;
        private int _position = -1;
        
        public bool MoveNext()
        {
            _position++;
            return _position < _incomingObjects.Count();
        }

        public void Reset()
        {
            _position = -1;
        }

        public TIncoming Current
        {
            get
            {
                try
                {
                    // because we use LinkedList<T> for better insertion performance
                    // use LINQ ElementAt() extension method to get object.
                    return _incomingObjects.ElementAt(_position);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }   
        }

        object IEnumerator.Current => Current!;

        public void Dispose() { }
    }
}