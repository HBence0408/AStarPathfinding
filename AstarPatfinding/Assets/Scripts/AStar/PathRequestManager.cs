using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;

public class PathRequestManager : MonoBehaviour
{
    public static PathRequestManager Instance;
    [SerializeField] PathFinderGrid grid;

   

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
        //requestQueue.Enqueue(new PathRequest(pathStart, pathEnd, callback));
        //Debug.Log("added request");
        Debug.Log("new path request");
        Profiler.BeginSample("task");
        //Task pathfiniding = Task.Run(() => FindPath(new PathRequest(pathStart, pathEnd, callback)));
        FindPath(new PathRequest(pathStart, pathEnd, callback));
        Profiler.EndSample();
    }
    public void AddRequest(PathRequest pathRequest)
    {
        //requestQueue.Enqueue(new PathRequest(pathStart, pathEnd, callback));
        //Debug.Log("added request");
        Task pathfiniding = Task.Run(() => FindPath(pathRequest));

    }
    /*
    private void ProcessNext()
    {
        if (requestQueue.Count > 0 && !isProcessing)
        {
            currentRequest = requestQueue.Dequeue();
            isProcessing = true;
            Debug.Log("starting porcessing");
            //StartCoroutine(FindPath(currentRequest.PathStart, currentRequest.PathEnd));
            Debug.Log("corutine started");
            //Pathfinding.Instance.StartPathFinding(currentRequest.PathStart,currentRequest.PathEnd);
        }
    }
    */
    private void PathProcessingFinished(List<Vector3> path, bool sucess, PathRequest pathRequest)
    {
        //Debug.Log("process finished");
        pathRequest.Callback(path, sucess);
        //Debug.Log("callback sent");
 

    }
    #endregion requestManager

    #region pathfinder
    public void FindPath(PathRequest pathRequest)
    {
        Profiler.BeginSample("pathfinding");
        //Debug.Log("finding path");
        Node startNode = grid.FindNode(pathRequest.PathStart);
        Node targetNode = grid.FindNode(pathRequest.PathEnd);
        List<Vector3> thePath = null;
       //Debug.Log("start node, target node , path created");
        if (!startNode.Walkable || !targetNode.Walkable )
        {
            PathProcessingFinished(thePath, false,pathRequest);
            Profiler.EndSample();
            return;
        }

        MinBinaryHeap<Node> openSet = new MinBinaryHeap<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Insert(startNode);
       // Debug.Log("while start");
        while (!openSet.IsEmty)
        {
            //Debug.Log("in while");
            Node currentnode = openSet.ExctractMin();
            //Debug.Log(currentnode.I + " " + currentnode.J+ " " + targetNode.I + " " + targetNode.J);
            closedSet.Add(currentnode);

            if (currentnode.Equals( targetNode))
            {
                //Debug.Log("retracing");
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
                if (closedSet.Contains(node))
                {
                    //Debug.Log("closed");
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
    #endregion pathfinder
}


