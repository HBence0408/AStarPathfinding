using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class MinBinaryHeap<T> : IEnumerable where T : IComparable
{
    private List<T> heapList = new List<T>();

    public T ReturnMin { get => (T)heapList[0]; }

    public bool IsEmty { get => heapList.Count == 0; }

    public bool Contains(T obj) => heapList.Contains(obj);

    public int IndexOfObject(T obj) => heapList.IndexOf(obj);

    private int[] FindChildrenIndex(int n)
    {
        int[] Children = new int[2];

        for (int i = 0; i < 2; i++)
        {
            Children[i] = (n * 2) + i + 1;
            if (Children[i] > heapList.Count - 1)
            {
                Children[i] = -1;
            }
        }
        return Children;
    }

    private int FindParentIndex(int n) => (n - 1) / 2;

    public void Insert(T obj)
    {
        heapList.Add((T)obj);

        int currentIndex = heapList.Count - 1;
        T current = heapList[currentIndex];

        while (current.CompareTo(heapList[FindParentIndex(currentIndex)]) < 0)
        {
            heapList[currentIndex] = heapList[FindParentIndex(currentIndex)];
            heapList[FindParentIndex(currentIndex)] = current;
            currentIndex = FindParentIndex(currentIndex);

        }

        int[] ChildrenIndex = FindChildrenIndex(currentIndex);
        for (int i = 0; i < ChildrenIndex.Length; i++)
        {
            if (ChildrenIndex[i] == -1)
            {
                continue;
            }

            if (heapList[i].CompareTo(current) > 0)
            {
                T temp = current;
                //Console.WriteLine("the chidren index " + ChildrenIndex[i]);
                heapList[currentIndex] = heapList[ChildrenIndex[i]];
                heapList[ChildrenIndex[i]] = current;
                break;
            }
        }
    }

    public T ExctractMin()
    {
        T Min = ReturnMin;

        T current = Min;
        int currentIndex = 0;

        int[] childIndexes = FindChildrenIndex(currentIndex);

        while (childIndexes[0] != -1 && childIndexes[1] != -1)
        {
            if (childIndexes[0] == -1)
            {
                heapList[currentIndex] = heapList[childIndexes[1]];
                heapList.RemoveAt(childIndexes[1]);
                break;
            }
            if (childIndexes[1] == -1)
            {
                heapList[currentIndex] = heapList[childIndexes[0]];
                heapList.RemoveAt(childIndexes[0]);
                break;
            }

            if (heapList[childIndexes[0]].CompareTo(heapList[childIndexes[1]]) < 0)
            {
                heapList[currentIndex] = heapList[childIndexes[0]];
                currentIndex = childIndexes[0];
            }
            else
            {
                heapList[currentIndex] = heapList[childIndexes[1]];
                currentIndex = childIndexes[1];
            }
            childIndexes = FindChildrenIndex(currentIndex);
        }
        heapList.RemoveAt(currentIndex);
        return Min;
    }

    public void UpdateObject(int index)
    {
        T current = heapList[index];

        while (current.CompareTo(heapList[FindParentIndex(index)]) < 0)
        {
            heapList[index] = heapList[FindParentIndex(index)];
            heapList[FindParentIndex(index)] = current;
            index = FindParentIndex(index);
        }

        int[] childIndexes = FindChildrenIndex(index);
        while (childIndexes[0] != -1 && childIndexes[1] != -1)
        {
            if (childIndexes[0] == -1)
            {
                T temp = heapList[index];
                heapList[index] = heapList[childIndexes[1]];
                heapList[childIndexes[1]] = temp;
                break;
            }
            if (childIndexes[1] == -1)
            {
                T temp = heapList[index];
                heapList[index] = heapList[childIndexes[0]];
                heapList[childIndexes[0]] = temp;
                break;
            }
            if (heapList[childIndexes[0]].CompareTo(heapList[childIndexes[1]]) < 0)
            {
                T temp = heapList[index];
                heapList[index] = heapList[childIndexes[0]];
                heapList[childIndexes[0]] = temp;
                index = childIndexes[0];
            }
            else
            {
                T temp = heapList[index];
                heapList[index] = heapList[childIndexes[1]];
                heapList[childIndexes[1]] = temp;
                index = childIndexes[1];
            }
            childIndexes = FindChildrenIndex(index);
        }
    }

    public IEnumerator GetEnumerator()
    {
        foreach (T item in heapList)
        {
            yield return item;
        }
    }
}