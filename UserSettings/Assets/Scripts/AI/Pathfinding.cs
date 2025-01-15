using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pathfinding : MonoBehaviour
{

	PathRequestManager requestManager;
	Grid grid;

	void Awake()
	{
		requestManager = GetComponent<PathRequestManager>();
		grid = GetComponent<Grid>();
	}


	public void StartFindPath(Vector3 startPos, Vector3 targetPos)
	{
		StartCoroutine(FindPath(startPos, targetPos));
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Vector3[] waypoints = new Vector3[0]; // path array
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);


		if (startNode.walkable && targetNode.walkable)
		{
			Heap<Node> openSet = new Heap<Node>(grid.MaxSize); // the set of nodes to be evaluated
			HashSet<Node> closedSet = new HashSet<Node>(); // the set of nodes evaluated
			openSet.Add(startNode); // adds start pos to the open set

			while (openSet.Count > 0)
			{
				Node currentNode = openSet.RemoveFirst(); // remove lowest cost node from open
				closedSet.Add(currentNode); // add node to closed

				if (currentNode == targetNode) // path has been found
				{
					pathSuccess = true;
					break;
				}

				// for neighbours in current node
				foreach (Node neighbour in grid.GetNeighbours(currentNode))
				{
					// if neighbour isn't traversable or is already closed
					if (!neighbour.walkable || closedSet.Contains(neighbour)) 
					{
						continue; // skip to next neighbour
					}

					// pathfinding calculations
					// real cost + heuristic 
					int newMovementCostToNeighbour = currentNode.rCost + GetDistance(currentNode, neighbour)+ neighbour.movementPenalty;
					// if new path to neighbour is shorter or neighbour not in openset
					if (newMovementCostToNeighbour < neighbour.rCost || !openSet.Contains(neighbour))
					{
						// set total cost
						neighbour.rCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance(neighbour, targetNode);
						// set parent node
						neighbour.parent = currentNode;

						// add neighbour to openset or update neighbour
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
						else
							openSet.UpdateItem(neighbour);
					}
				}
			}
		}
		yield return null;
		if (pathSuccess)
		{
			waypoints = RetracePath(startNode, targetNode); // path array
			pathSuccess = waypoints.Length > 0;
		}
		requestManager.FinishedProcessingPath(waypoints, pathSuccess); // calls finished when it finds a path

	}

	Vector3[] RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode; // starts backwards

		while (currentNode != startNode) // adds parents until start node
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints); // reverses the path
		return waypoints;

	}

	Vector3[] SimplifyPath(List<Node> path) // add waypoint when direction changes
	{
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i++)
		{
			// direction on x y axis btwn last 2 nodes
			Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
			if (directionNew != directionOld)
			{
				waypoints.Add(path[i].worldPosition); // if direction changes add pos to path array
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}

	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}


}