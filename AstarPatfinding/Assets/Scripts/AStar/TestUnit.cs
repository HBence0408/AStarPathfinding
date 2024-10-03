using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

public class TestUnit : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform target;
   // private static List<TestUnit> Units = new List<TestUnit>(); 
    private List<Vector3> path;
    [SerializeField] private bool followPath = false;
    private int pathIndex;
    public bool requestNewPath = true;
    //private Action<List<Vector3>, bool> callback;
    private Vector3 targetPos;
    private Vector3 pos;
    public SeekerData data;
    public bool WaitingForPath = false;
    //public TestUnit script { get => this; }


    private void Awake()
    {
       // Units.Add(this);
        //callback = OnPathFound;
    }

    void Start()
    {
        //PathRequestManager.Instance.AddRequest(this.transform.position, target.position, OnPathFound);
        //Vector3.MoveTowards(this.transform.position, target.position, speed);
       // PathRequestManager.Instance.AddRequest(this.transform.position, target.position, OnPathFound);
    }

    public void Poll()
    {
        //Profiler.BeginSample("requesting path");
        /*
        if (requestNewPath)
        {
            //targetPos = target.transform.position;
            //pos = this.transform.position;
           // Debug.Log("requesting");
            PathRequest requests = new PathRequest(this.transform.position, target.position, OnPathFound);
            //Task pathfiniding = Task.Run(() => PathRequestManager.Instance.FindPath(requests));
            Task pathfinding = Task.Run( () => PathRequestManager.Instance.FindPath(new PathRequest( this.transform.position, target.position, OnPathFound)));
            //PathRequestManager.Instance.FindPath(new PathRequest(this.transform.position, target.position, OnPathFound));
            requestNewPath = false;
        }
        //Profiler.EndSample();
        //PathRequestManager.Instance.AddRequest(this.transform.position, target.position, OnPathFound);
        */
        if (followPath)
        {
            FollowPath();
        } 
    }

    private void Updatdfghjke()
    {
        if (followPath)
        {
            FollowPath();
        }
    }

    /*
    public static void TargetPathChange()
    {
        foreach (TestUnit unit in Units)
        {
            unit.requestNewPath = true;
        }
    }
    */
    public void OnPathFound(List<Vector3> path, bool pathFound)
    {
        if (!pathFound)
        {
            return;
        }
        //Debug.Log("path recieved, starting corutine");
        WaitingForPath = false;
        this.path = path;
        pathIndex = 0;
        followPath = true;

       
    }

    public void SetPath()
    {

    }

    private void FollowPath()
    {


        //Debug.Log("corutine");

        if (path.Count-1 < pathIndex)
        {
            followPath = false;
            pathIndex = 0;
            Debug.Log(path.Last().x + " " + path.Last().y);
            return;
        }
        //Debug.Log(path[pathIndex].x + " " + path[pathIndex].y);
        this.transform.position = Vector2.MoveTowards(this.transform.position, path[pathIndex], speed);
        if (Vector2.Distance(this.transform.position, path[pathIndex]) < 0.1)
        {
            pathIndex++;
        }
    }
}
