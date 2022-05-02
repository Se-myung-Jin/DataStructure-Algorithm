using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class PriorityQueue
    {
        List<int> _heap = new List<int>();

        public void Push(int data)
        {
            _heap.Add(data);

            int now = _heap.Count - 1;

            while (now > 0)
            {
                int next = (now - 1) / 2;

                if (_heap[now] < _heap[next])
                    break;

                int temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;

                now = next;
            }
        }

        public int Pop()
        {
            int ret = _heap[0];

            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex--);

            int now = 0;
            while (true)
            {
                int left = now * 2 + 1;
                int right = now * 2 + 2;

                int next = now;
                if (left <= lastIndex && _heap[next] < _heap[left])
                    next = left;
                if (right <= lastIndex && _heap[next] < _heap[right])
                    next = right;

                if (next == now)
                    break;

                int temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;

                now = next;
            }

            return ret;
        }

        public int Count()
        {
            return _heap.Count;
        }
    }
}
