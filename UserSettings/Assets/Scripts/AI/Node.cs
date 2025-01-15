using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>
{

	public bool walkable;
	public Vector3 worldPosition;
	public int gridX, gridY; // node keeps track of it's position
	public int movementPenalty;

	public int rCost; // real cost
	public int hCost; // heuristic
	public Node parent;
	int heapIndex;

	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _penalty)
	{
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
		movementPenalty = _penalty;
	}

	public int tCost // total cost
	{
		get
		{
			return rCost + hCost;
		}
	}

	public int HeapIndex
	{
		get
		{
			return heapIndex;
		}
		set
		{
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare)
	{
		int compare = tCost.CompareTo(nodeToCompare.tCost);
		if (compare == 0) // if real cost is the same - compare heuristics
		{
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare; // returns lowest 
	}
}