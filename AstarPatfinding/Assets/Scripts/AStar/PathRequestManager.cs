using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;

public class PathRequestManager : MonoBehaviour
{
    public static PathRequestManager Instance;
    [SerializeField] PathFinderGrid grid;
    [SerializeField] private int requestProcessedIn1Frame;
    [SerializeField] private Transform target;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("multiple pathrequest managers, destrtoying self");
            Destroy(this.gameObject);
        }
    }

    private void PathProcessingFinished(List<Vector3> path, bool sucess, PathRequest pathRequest)
    {
        pathRequest.Callback(path, sucess);
    }

    public void FindPath(PathRequest pathRequest)
    {
        Profiler.BeginSample("pathfinding");
        Node startNode = grid.FindNode(pathRequest.PathStart);
        Node targetNode = grid.FindNode(pathRequest.PathEnd);
        List<Vector3> thePath = null;

        if (!startNode.Walkable || !targetNode.Walkable )
        {
            PathProcessingFinished(thePath, false,pathRequest);
            Profiler.EndSample();
            return;
        }

        MinBinaryHeap<Node> openSet = new MinBinaryHeap<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Insert(startNode);

        while (!openSet.IsEmty)
        {
            Node currentnode = openSet.ExctractMin();

            closedSet.Add(currentnode);

            if (currentnode.Equals( targetNode))
            {
                thePath = RetarcePath(startNode, currentnode);
                break;
            }

            foreach (Node node in grid.FindNeighbours(currentnode))
            {
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

        if (thePath != null)
        {
            PathProcessingFinished(thePath, true, pathRequest);
        }
        else
        {
            PathProcessingFinished(thePath, false, pathRequest);
        }
    }

    private List<Vector3> RetarcePath(Node start, Node end)
    {
        List<Vector3> path = new List<Vector3>();
        Node current = end;

        while (current != start)
        {
            path.Add(current.WorldPosition);
            current = current.Parent;
        }

        path = SimplyfyPath(path);
        path.Reverse();
        path.Add(end.WorldPosition);
        return path;
    }

    private List<Vector3> SimplyfyPath(List<Vector3> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        float oldDir = Vector3.Angle(path[0], path[1]);
        float newDir;

        for (int i = 1; i < path.Count; i++)
        {
            newDir = Vector3.Angle(path[i - 1], path[i]);
            if (newDir != oldDir)
            {
                waypoints.Add(path[i]);
            }
            oldDir = newDir;
        }

        return waypoints;
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


