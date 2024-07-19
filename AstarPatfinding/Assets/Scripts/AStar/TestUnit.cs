using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestUnit : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform target;
    private List<Vector3> path;
    private bool followPath = false;
    private int pathIndex;

    void Start()
    {
        PathRequestManager.Instance.AddRequest(this.transform.position, target.position, OnPathFound);
        //Vector3.MoveTowards(this.transform.position, target.position, speed);
    }

    void Update()
    {
        //this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, speed);
        if (followPath)
        {
            FollowPath();
        }
        
    }

    private void OnPathFound(List<Vector3> path, bool pathFound)
    {
        if (!pathFound)
        {
            return;
        }
        Debug.Log("path recieved, starting corutine");
        this.path = path;
        followPath = true;
       
    }

    private void FollowPath()
    {


        Debug.Log("corutine");

        if (path.Count-1 < pathIndex)
        {
            followPath = false;
            pathIndex = 0;
            Debug.Log(path.Last().x + " " + path.Last().y);
            return;
        }
        Debug.Log(path[pathIndex].x + " " + path[pathIndex].y);
        this.transform.position = Vector2.MoveTowards(this.transform.position, path[pathIndex], speed);
        if (Vector2.Distance(this.transform.position, path[pathIndex]) < 0.1)
        {
            pathIndex++;
        }
        
    }
}
