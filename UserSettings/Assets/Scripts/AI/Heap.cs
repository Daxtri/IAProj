using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T>
{
	T[] items;
	int currentItemCount;

	public Heap(int maxHeapSize)
	{
		items = new T[maxHeapSize];
	}

	public void Add(T item)
	{
		item.HeapIndex = currentItemCount;
		items[currentItemCount] = item; // add item to array
		SortUp(item);
		currentItemCount++;
	}

	public T RemoveFirst() // removes item from array
	{
		T firstItem = items[0];
		currentItemCount--;
		items[0] = items[currentItemCount]; // take item at the end 
		items[0].HeapIndex = 0; // put in first place
		SortDown(items[0]);
		return firstItem;
	}

	public void UpdateItem(T item) // found a cheaper path
	{
		SortUp(item);
	}

	public int Count
	{
		get
		{
			return currentItemCount;
		}
	}

	public bool Contains(T item)
	{
		return Equals(items[item.HeapIndex], item);
	}

	void SortDown(T item)
	{
		while (true)
		{
			// indexes of the children
			int childIndexLeft = item.HeapIndex * 2 + 1;
			int childIndexRight = item.HeapIndex * 2 + 2;
			int swapIndex = 0;

			if (childIndexLeft < currentItemCount)
			{
				swapIndex = childIndexLeft;

				if (childIndexRight < currentItemCount)
				{
					if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) // which child has higher cost
					{
						swapIndex = childIndexRight;
					}
				}

				if (item.CompareTo(items[swapIndex]) < 0) // parent has lower cost than child
				{
					Swap(item, items[swapIndex]);
				}
				else{
					return;
				}
			}
			else{
				return;
			}

		}
	}

	void SortUp(T item)
	{
		int parentIndex = (item.HeapIndex - 1) / 2; // gets the parent from the item

		while (true)
		{
			T parentItem = items[parentIndex];
			if (item.CompareTo(parentItem) > 0) // compares item to parent; has higher prio, lower t cost
			{
				Swap(item, parentItem); 
			}
			else
			{
				break;
			}

			parentIndex = (item.HeapIndex - 1) / 2;
		}
	}

	void Swap(T itemA, T itemB)
	{
		//swap values
		items[itemA.HeapIndex] = itemB;
		items[itemB.HeapIndex] = itemA;
		// swap indexes
		int itemAIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = itemAIndex;
	}
}

// each item tracks it's index
public interface IHeapItem<T> : IComparable<T>
{
	int HeapIndex
	{
		get;
		set;
	}
}