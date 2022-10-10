using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfind_Grid : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Layers of the unwalkable nodes.
    /// </summary>
    [SerializeField] private LayerMask unwalkableMask;

    /// <summary>
    /// Total size of the 2D grid.
    /// </summary>
    [SerializeField] private Vector2 gridWorldSize;

    /// <summary>
    /// Size of each node in the grid.
    /// </summary>
    [SerializeField] private float nodeRadius;

    /// <summary>
    /// Grid of nodes that store the information of each position in the grid.
    /// </summary>
    Node[,] grid;


    /// <summary>
    /// Total size covered by a node.
    /// </summary>
    private float nodeDiameter;

    /// <summary>
    /// Amount of nodes in the X and Y cords/index of the 2D array/grid.
    /// </summary>
    private int gridSizeX, gridSizeY;

    /// <summary>
    /// Total size of the grid nodes
    /// </summary>
    public int MaxSize => gridSizeX * gridSizeY;

    #endregion

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;

        // To get how many nodes will fit in X or Y first divide the total size by the size of each node
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    /// <summary>
    /// Generates the grid by populating it with nodes, starting at the bottom left.
    /// </summary>
    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        // The center of the current position, then subtract half the x size to get the border on the left, then substract half the total y to get to the bottom.
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Save the current position and then check if is walkable by checking if a collision happens
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool isWalkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                grid[x, y] = new Node(isWalkable, worldPoint, x, y);
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
                if(x == 0 && y == 0)
                {
                    // Continue just skips this iteration and keeps going
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    /// <summary>
    /// Gets the node at the given position.
    /// </summary>
    /// <param name="worldPos">Position to check for a node</param>
    /// <returns>The node at the given position, or the closest possible</returns>
    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float percentX = (worldPos.x / gridWorldSize.x) + 0.5f;
        // worldPos.z because the grid only has X and Y. In this case Z is the Y equivalent.
        float percentY = (worldPos.z / gridWorldSize.y) + 0.5f;

        // Clamp in between 0-1.
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // Get the total index size and return the index with the % of where the pos is.
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    #region Gizmos

    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.isWalkable ? Color.white : Color.red);
                if(path != null)
                {
                    if(path.Contains(n))
                        Gizmos.color = Color.black;
                }
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }

    #endregion
}
