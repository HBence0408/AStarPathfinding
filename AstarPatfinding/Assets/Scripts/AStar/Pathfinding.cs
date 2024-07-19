using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    /*
    [SerializeField] PathFinderGrid grid;
    public static new Pathfinding Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("multiõle pathfinding script destroying self");
            Destroy(this.gameObject);
        }
    }

    public void StartPathFinding(Vector3 start, Vector3 end)
    {
        StartCoroutine(FindPath(start, end));  
    }

    private IEnumerator FindPath(Vector3 startWorldPos, Vector3 targetWorldPos)
    {
        Node startNode = grid.FindNode(startWorldPos);
        Node targetNode = grid.FindNode(targetWorldPos);
        List<Vector3> thePath = null;

        MinBinaryHeap<Node> openSet = new MinBinaryHeap<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Insert(startNode);

        while (!openSet.IsEmty)
        {
            Node currentnode = openSet.ExctractMin();
            closedSet.Add(currentnode);

            if (currentnode == targetNode)
            {
                //Debug.Log("retracing");
                thePath = RetarcePath(currentnode, startNode);
                break;
                //grid.PATH = RetarcePath(currentnode, startNode);
            }

            foreach (Node node in grid.FindNeighbours(currentnode))
            {
                //Debug.Log("cheching neighbours");
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
                        openSet.Insert(node);
                    }
                    else
                    {
                        openSet.UpdateObject(openSet.IndexOfObject(node));
                    }
                }
            }
        }

        yield return null;

        if (thePath != null)
        {
            PathProcessingFinished(thePath, true);
        }
        else
        {
            PathProcessingFinished(thePath, false);
        }
    }

    private List<Vector3> RetarcePath(Node start, Node end)
    {
        List<Vector3> path = new List<Vector3>();
        Node current = start;

        while (current != end)
        {
            path.Add(current.WorldPosition);
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
    */

}
