using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

public struct SeekerData
{
    // Start is called before the first frame update
    //private Vector3 position;
    private Transform unit;
    private Transform target;
    private TestUnit unitScript;
    public bool requestPath;
    [SerializeField] private bool pathProcessing;
    //private PathRequest request;
    //private PathRequest request;

    public SeekerData(Transform u, Transform t, TestUnit sc)
    {
        unit = u;
        target = t;
        unitScript = sc;
        requestPath = true;
        pathProcessing = false;
       // request = new PathRequest(unit.position, target.position, unitScript.OnPathFound);
        // request = new PathRequest();

        /*
        position = pos;
        Debug.Log("setting position: " + position);
        */
    }

    public void callback(List<Vector3> path, bool pathFound)
    {
        pathProcessing = false;
        Debug.Log("processing finished");
        unitScript.OnPathFound(path,pathFound);
    }

    public void Update()
    {
        if (requestPath)
        {
            PathRequest request = new PathRequest(unit.position, target.position, unitScript.OnPathFound);

            Task pathfiniding = Task.Run(() => PathRequestManager.Instance.FindPath(request));
            requestPath = false;
            // megnézné hogy küldötte requestet ha igen akkor csak updatelje nem újat küld, update -> ref-el át adja a pathrequest referenciát és ha változik akkor csak meg változtatja

            /*
            if (unitScript.WaitingForPath)
            {
                Debug.Log("path processing");
                //request.PathEnd = target.position;
            }
            else
            {
                PathRequestManager.Instance.AddRequest(ref request);
                unitScript.WaitingForPath = true;
                Debug.Log("adding request");
            }
            */
            //PathRequestManager.Instance.AddRequest(request);

        }
        
        //Task pathfinding = Task.Run(() => PathRequestManager.Instance.FindPath(new PathRequest(this.transform.position, target.position, OnPathFound)));
    }

    // Update is called once per frame
}
