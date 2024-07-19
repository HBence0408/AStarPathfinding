using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    public static PathRequestManager Instance;
    private Queue<PathRequest> requestQueue = new Queue<PathRequest>();
    private bool isProcessing = false;
    private PathRequest currentRequest;
    [SerializeField] PathFinderGrid grid;

    public bool Isprocessing { get => isProcessing; }

    private struct PathRequest
    {
        private Vector3 pathStart;
        private Vector3 pathEnd;
        private Action<List<Vector3>, bool> callback;

        public Vector3 PathStart { get => pathStart; }
        public Vector3 PathEnd { get => pathEnd; }
        public Action<List<Vector3>, bool> Callback { get => callback; }

        public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<List<Vector3>, bool> callback)
        {
            this.pathStart = pathStart;
            this.pathEnd = pathEnd;
            this.callback = callback;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("multiple pathrequest magers, destrtoying self");
            Destroy(this.gameObject);
        }
    }

    #region requestManager
    public void AddRequest(Vector3 pathStart, Vector3 pathEnd, Action<List<Vector3>,bool> callback)
    {
        requestQueue.Enqueue(new PathRequest(pathStart, pathEnd, callback));
        Debug.Log("added request");
        ProcessNext();
    }

    private void ProcessNext()
    {
        if (requestQueue.Count > 0 && !isProcessing)
        {
            currentRequest = requestQueue.Dequeue();
            isProcessing = true;
            Debug.Log("starting porcessing");
            StartCoroutine(FindPath(currentRequest.PathStart, currentRequest.PathEnd));
            Debug.Log("corutine started");
            //Pathfinding.Instance.StartPathFinding(currentRequest.PathStart,currentRequest.PathEnd);
        }
    }

    private void PathProcessingFinished(List<Vector3> path, bool sucess)
    {
        Debug.Log("process finished");
        currentRequest.Callback(path, sucess);
        Debug.Log("callback sent");
        isProcessing = false;
        ProcessNext();
    }
    #endregion requestManager

    #region pathfinder
    private IEnumerator FindPath(Vector3 startWorldPos, Vector3 targetWorldPos)
    {
        Debug.Log("finding path");
        Node startNode = grid.FindNode(startWorldPos);
        Node targetNode = grid.FindNode(targetWorldPos);
        List<Vector3> thePath = null;

        if (!startNode.Walkable || !targetNode.Walkable )
        {
            PathProcessingFinished(thePath, false);
            yield break;
        }

        MinBinaryHeap<Node> openSet = new MinBinaryHeap<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Insert(startNode);

        while (!openSet.IsEmty)
        {
            Node currentnode = openSet.ExctractMin();
            closedSet.Add(currentnode);

            if (currentnode == targetNode)
            {
                Debug.Log("retracing");
                thePath = RetarcePath(startNode, currentnode);
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
    #endregion pathfinder
}


