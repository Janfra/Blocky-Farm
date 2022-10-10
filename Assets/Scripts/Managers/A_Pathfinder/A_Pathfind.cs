using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfind_Grid))]
public class A_Pathfind : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Grid containing all nodes
    /// </summary>
    [SerializeField] private Pathfind_Grid grid;

    public Transform seeker, target;

    #endregion

    private void Awake()
    {
        if (grid == null)
        {
            grid = GetComponent<Pathfind_Grid>();
        }
    }

    private void Update()
    {
        FindPath(seeker.position, target.position);
    }

    /// <summary>
    /// Finds the closest path to the target node.
    /// </summary>
    /// <param name="startPos">Starting position</param>
    /// <param name="targetPos">Desired position to get to</param>
    private void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        // Hashset lets you quickly determine whether an object is already in the set or not.
        // All elements are unique. Doesnt keep the order in which elements were added. Faster than list. No index to search, only enumerator and create to list.
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if(!neighbour.isWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }

    /// <summary>
    /// Distance in between two nodes.
    /// </summary>
    /// <param name="nodeA">Starting node</param>
    /// <param name="nodeB">Target node</param>
    /// <returns>Distance in between the nodes for the H cost</returns>
    private int GetDistance(Node nodeA, Node nodeB)
    {
        // absolute value just makes negative numbers positive, useful for checking distances.
        int disX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int disY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        // 14 is the diagonal cost in a square grid and 10 the horizontal/vertical cost. Use the lowest distance for the diagonal. For visual min 14 https://www.youtube.com/watch?v=mZfyt03LDH4
        if (disX > disY)
        {
            return 14 * disY + 10 * (disX - disY);
        }
        else
        {
            return 14 * disX + 10 * (disY - disX);
        }
    }
}
