using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

public struct SeekerData
{
    private Transform unit;
    private Transform target;
    private ISeeker unitScript;
    public bool requestPath;

    public SeekerData(Transform u, Transform t, ISeeker sc)
    {
        unit = u;
        target = t;
        unitScript = sc;
        requestPath = true;
    }

    public void Update()
    {
        if (requestPath)
        {
            PathRequest request = new PathRequest(unit.position, target.position, unitScript.OnPathFound);
            Task pathfiniding = Task.Run(() => PathRequestManager.Instance.FindPath(request));
            requestPath = false;
        }
    }
}
