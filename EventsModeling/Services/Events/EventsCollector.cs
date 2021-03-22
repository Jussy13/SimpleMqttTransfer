using EventsModeling.Events;

namespace EventsModeling.Services.Events
{
    public static class EventsCollector
    {
        private static Node<IEvent> _head;

        public static int Count { get; private set; }
        public static bool IsEmpty => Count == 0;

        static EventsCollector()
        {
            _head = new Node<IEvent>(null);
        }

        public static void AddEvent(IEvent @event)
        {
            var node = new Node<IEvent>(@event);

            if (_head == null)
            {
                _head = node;
            }
            else
            {
                var cur = _head;
                Node<IEvent> prev = null;

                while (null != cur && (cur.Data.FinishedAt <= node.Data.FinishedAt))
                {
                    prev = cur;
                    cur = cur.Next;
                }

                if (null == prev)
                {
                    _head = node;
                    _head.Next = cur;
                }
                else if (null == cur)
                {
                    prev.Next = node;
                }
                else
                {
                    node.Next = cur;
                    prev.Next = node;
                }
            }

            Count++;
        }

        public static IEvent GetEvent()
        {
            var result = _head;

            if (null != result)
            {
                _head = _head.Next;
                Count--;
            }

            return result?.Data;
        }

        public static void Clear()
        {
            _head = null;
            Count = 0;
        }

        private class Node<T>
        {
            public Node(T data) => Data = data;
            public T Data { get; }
            public Node<T> Next { get; set; }
        }
    }
}
