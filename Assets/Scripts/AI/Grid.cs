using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{

	public bool displayGridGizmos;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public TerrainType[] walkableRegions;
	public int ProxPenalty = 15;
	LayerMask walkableMask;
	Dictionary<int, int> walkableRegionsDic = new Dictionary<int, int>();
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	int penaltyMin = int.MaxValue;
	int penaltyMax = int.MinValue;

	void Awake()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach (TerrainType region in walkableRegions)
        {
			walkableMask.value |= region.terrainMask.value;
			walkableRegionsDic.Add((int)Mathf.Log(region.terrainMask.value,2),region.terrainPenalty);
        }
		CreateGrid();
	}

	public int MaxSize
	{
		get
		{
			return gridSizeX * gridSizeY;
		}
	}

	void CreateGrid()
	{
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

				int movementPenalty = 0;

				// raycast to find the layers
				// calculate the movement penalty
				Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100, walkableMask))
				{
					walkableRegionsDic.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
				}

				// penalty to walk around obstacles
                if (!walkable)
                {
					movementPenalty += ProxPenalty;
                }

				grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
			}
		}
		BlurMap(3);
	}

	// path smoothing
	void BlurMap(int blurSize)
    {
		int kernelSize = blurSize * 2 + 1; // odd number for central square
		int kernelExtents = (kernelSize - 1) / 2;// squares btwn center and edge

		int[,] horizontalPass = new int[gridSizeX, gridSizeY];
		int[,] verticalPass = new int[gridSizeX, gridSizeY];

		// horizontal pass
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
				int sampleX = Mathf.Clamp(x, 0, kernelExtents);
				horizontalPass[0, y] += grid[sampleX, y].movementPenalty;
            }

            for (int x = 1; x < gridSizeX; x++) // remaining columns in the row
            {
				int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
				int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

				horizontalPass[x, y] = horizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty +
										grid[addIndex, y].movementPenalty;

			}
        }
		// vertical pass
		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = -kernelExtents; y <= kernelExtents; y++)
			{
				int sampleY = Mathf.Clamp(y, 0, kernelExtents);
				verticalPass[x, 0] += horizontalPass[x, sampleY];
			}

			for (int y = 1; y < gridSizeY; y++) // remaining columns in the row
			{
				int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
				int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

				verticalPass[x, y] = verticalPass[x, y - 1] - horizontalPass[x, removeIndex] +
										horizontalPass[x,addIndex];
				int blurred = Mathf.RoundToInt((float)verticalPass[x, y] / (kernelSize * kernelSize));
				grid[x, y].movementPenalty = blurred;

				if (blurred > penaltyMax)
					penaltyMax = blurred;
				if (blurred < penaltyMin)
					penaltyMin = blurred;
			}
		}
	}

	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)
					continue;

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
				{
					neighbours.Add(grid[checkX, checkY]);
				}
			}
		}

		return neighbours;
	}


	public Node NodeFromWorldPoint(Vector3 worldPosition)
	{
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		return grid[x, y];
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
		if (grid != null && displayGridGizmos)
		{
			foreach (Node n in grid)
			{
				Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
				Gizmos.color = (n.walkable) ?Gizmos.color : Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter /*- .1f*/));
			}
		}
	}

	[System.Serializable]
	public class TerrainType
    {
		public LayerMask terrainMask;
		public int terrainPenalty;
    }
}