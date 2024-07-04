using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    [SerializeField] PathFinderGrid grid;
    private List<Node> thePath;
    public List<Node> ThePath
    {
        get => thePath;
        
    }

    public void FindPath(Vector3 startWorldPos, Vector3 targetWorldPos)
    {
        Node startNode = grid.FindNode(startWorldPos);
        Node targetNode = grid.FindNode(targetWorldPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            //Debug.Log(openSet.Count);
            Node currentnode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentnode.FCost || openSet[i].FCost == currentnode.FCost && openSet[i].HCost < currentnode.HCost)
                {
                    currentnode = openSet[i];
                }
            }

            openSet.Remove(currentnode);
            closedSet.Add(currentnode);

            if (currentnode == targetNode)
            {
                Debug.Log("retracing");
                thePath = RetarcePath(currentnode, startNode);
                grid.PATH = RetarcePath(currentnode, startNode);
            }

            foreach (Node node in grid.FindNeighbours(currentnode))
            {
                Debug.Log("cheching neighbours");
                if (!node.Walkable || closedSet.Contains(node))
                {
                    continue;
                }

                int newMovementCost = currentnode.GCost + GetGridDistance(node, currentnode);
                if (node.GCost > newMovementCost || !openSet.Contains(node))
                {
                    node.GCost = newMovementCost;
                    node.HCost = GetGridDistance(node, targetNode);
                    node.Parent = currentnode;

                    if (!openSet.Contains(node))
                    {
                        openSet.Add(node);
                    }
                }
            }

            

        }
        

    }

    private List<Node> RetarcePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = start;

        while (current != end)
        {
            path.Add(current);
            current = current.Parent;
        }
        path.Reverse();

        return path;
    }

    private int GetGridDistance(Node nodeA, Node nodeB)
    {
        int i = Mathf.Abs(nodeA.I - nodeB.I);
        int j = Mathf.Abs(nodeA.J - nodeB.J);

        if (i > j)
        {
            return 14 * j + 10 * (i - j);
        }
        else
        {
            return 14 * i + 10 * (j - i);
        }
    }

}
