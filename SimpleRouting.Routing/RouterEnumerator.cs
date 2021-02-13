using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SimpleRouting.Routing
{
    public class RouterEnumerator<T> : IEnumerator<T>
    {
        public RouterEnumerator(IEnumerable<T> incomingObjects)
        {
            _incomingObjects = incomingObjects;
        }
        
        private readonly IEnumerable<T> _incomingObjects;
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

        public T Current =>
            _incomingObjects.ElementAtOrDefault(_position) ?? throw new IndexOutOfRangeException();

        object IEnumerator.Current => Current!;

        public void Dispose() { }
    }
}