using System;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<TElement, TPriority>
{
    private List<(TElement Element, TPriority Priority)> heap;
    private readonly IComparer<TPriority> comparer;

    public int Count => heap.Count;

    public PriorityQueue()
    {
        heap = new List<(TElement Element, TPriority Priority)>();
        comparer = Comparer<TPriority>.Default;
    }

    public PriorityQueue(IComparer<TPriority> comparer)
    {
        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }

        heap = new List<(TElement Element, TPriority Priority)>();
        this.comparer = comparer;
    }

    public PriorityQueue(IEnumerable<(TElement Element, TPriority Priority)> items)
    {
        if (items == null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        heap = new List<(TElement Element, TPriority Priority)>(items);
        comparer = Comparer<TPriority>.Default;

        for (int i = heap.Count / 2 - 1; i >= 0; --i)
        {
            HeapifyDown(i);
        }
    }

    public PriorityQueue(
        IEnumerable<(TElement Element, TPriority Priority)> items,
        IComparer<TPriority> comparer)
    {
        if (items == null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }

        this.comparer = comparer;
        heap = new List<(TElement Element, TPriority Priority)>(items);

        for (int i = heap.Count / 2 - 1; i >= 0; --i)
        {
            HeapifyDown(i);
        }
    }

    public void Clear()
    {
        heap.Clear();
    }

    public TElement Peek()
    {
        if (heap.Count == 0)
        {
            throw new InvalidOperationException("큐가 비어 있습니다.");
        }

        return heap[0].Element;
    }

    private void Swap(int i, int j)
    {
        var temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }

    public void Enqueue(TElement element, TPriority priority)
    {
        heap.Add((element, priority));

        int index = heap.Count - 1;
        HeapifyUp(index);
    }

    public TElement Dequeue()
    {
        if (heap.Count == 0)
        {
            throw new InvalidOperationException("큐가 비어 있습니다.");
        }

        TElement result = heap[0].Element;

        int lastIndex = heap.Count - 1;

        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        if (heap.Count > 0)
        {
            HeapifyDown(0);
        }

        return result;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;

            if (comparer.Compare(
                heap[index].Priority,
                heap[parentIndex].Priority) >= 0)
            {
                break;
            }

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void HeapifyDown(int index)
    {
        int lastIndex = heap.Count - 1;

        while (true)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;
            int smallestIndex = index;

            if (leftChildIndex <= lastIndex &&
                comparer.Compare(
                    heap[leftChildIndex].Priority,
                    heap[smallestIndex].Priority) < 0)
            {
                smallestIndex = leftChildIndex;
            }

            if (rightChildIndex <= lastIndex &&
                comparer.Compare(
                    heap[rightChildIndex].Priority,
                    heap[smallestIndex].Priority) < 0)
            {
                smallestIndex = rightChildIndex;
            }

            if (index == smallestIndex)
            {
                break;
            }

            Swap(index, smallestIndex);
            index = smallestIndex;
        }
    }
}