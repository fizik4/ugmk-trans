using System.Collections;

namespace MyLib;

public class ClosedList<T> : IClosedList<T>
{
    private List<Node<T>> _inner;
    private Node<T> _head;
    private Node<T> _current;

    public ClosedList()
    {
        _inner = new List<Node<T>>();
    }

    public ClosedList(IEnumerable<T> values) : this()
    {
        if (values is null || !values.Any())
            return;

        var arr = values.ToArray();

        _head = new Node<T>
        {
            Value = arr[0]
        };
        _head.Next = _head;
        _head.Previous = _head;
        
        _current = _head;

        _inner.Add(_head);

        for (int i = 1; i < arr.Length; i++)
        {
            var node = new Node<T>
            {
                Value = arr[i],
                Previous = _inner.Last(),
                Next = _inner.First()
            };

            _inner.Last().Next = node;
            _inner.Add(node);
        }

        _head.Previous = _inner.Last();
    }

    public T this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public T Head
    {
        get
        {
            if (_head is null)
                throw new InvalidOperationException("Collection is empty");

            return _head.Value;
        }
    }

    public T Current
    {
        get
        {
            CheckNotEmptyOrThrow();

            return _current.Value;
        }
    }

    public T Previous
    {
        get
        {
            CheckNotEmptyOrThrow();

            if (_current.Previous is null)
                return default;

            return _current.Previous.Value;
        }
    }

    public T Next
    {
        get
        {
            CheckNotEmptyOrThrow();

            if (_current.Next is null)
                return default;

            return _current.Next.Value;
        }
    }

    public event EventHandler<T> HeadReached;

    public int Count => _inner.Count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
        var node = new Node<T> { Value = item };
        var last = _inner.LastOrDefault();

        if (last is null)
        {
            node.Previous = node;
            node.Next = node;
            _inner.Add(node);
            _head = node;
            _current = node;
            return;
        }

        node.Next = last.Next;
        last.Next = node;
        node.Previous = last;
        _head.Previous = node;
    }

    public void Clear()
    {
        _current = null;
        _head = null;
        _inner.Clear();
    }

    public bool Contains(T item)
    {
        foreach (var i in _inner)
        {
            if (Equals(i.Value, item))
                return true;
        }

        return false;

    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        var list = _inner.Select(_ => _.Value).ToList();
        list.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public int IndexOf(T item)
    {
        var index = 0;
        foreach(var i in _inner)
        {
            if (Equals(i.Value, item))
                return index;

            index++;
        }

        return -1;
    }

    public void Insert(int index, T item)
    {
        var newNode = new Node<T> { Value = item };
        _inner.Insert(index, newNode);

        if (index == 0)
        {
            newNode.Next = _head;
            newNode.Previous = _head.Previous;
            _head.Previous = newNode;
            _head = newNode;
            return;
        }

        _inner[index - 1].Next = newNode;
        newNode.Previous = _inner[index - 1];
        newNode.Next = _inner[index + 1];
        _inner[index + 1].Previous = newNode;
    }

    public void MoveBack(int step = 1)
    {
        if (step <= 0)
            throw new ArgumentException($"{nameof(step)}");

        CheckNotEmptyOrThrow();

        while (true)
        {
            _current = _current.Previous;

            if (_current == _head)
                HeadReached?.Invoke(this, _head.Value);

            if (--step <= 0)
                break;
        }
    }

    public void MoveNext(int step = 1)
    {
        if (step <= 0)
            throw new ArgumentException($"{nameof(step)}");

        CheckNotEmptyOrThrow();

        while (true)
        {
            _current = _current.Next;

            if (_current == _head)
                HeadReached?.Invoke(this, _head.Value);

            if (--step <= 0)
                break;
        }
    }

    public bool Remove(T item)
    {
        int i = 0;

        foreach(var node in _inner)
        {
            if (!Equals(node.Value, item))
            {
                i++;
                continue;
            }

            node.Previous.Next = node.Next;
            node.Next.Previous = node.Previous;

            if (Equals(node, _head))
                _head = node.Next;

            if (Equals(node, _current))
                _current = node.Next;

            _inner.Remove(node);
            return true;
        }

        return false;
    }

    public void RemoveAt(int index)
    {
        var node = _inner[index];
        Remove(node.Value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    private void CheckNotEmptyOrThrow()
    {
        if (_current is null)
            throw new InvalidOperationException("Collection is empty");
    }
}
