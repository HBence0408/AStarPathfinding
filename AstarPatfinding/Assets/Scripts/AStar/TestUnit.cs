using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

public class TestUnit : MonoBehaviour, ISeeker
{
    [SerializeField] private float speed;
    [SerializeField] private Transform target;
    private List<Vector3> path;
    [SerializeField] private bool followPath = false;
    private int pathIndex;
    public SeekerData data;

    public void Poll()
    {
        if (followPath)
        {
            FollowPath();
        } 
    }

    public void OnPathFound(List<Vector3> path, bool pathFound)
    {
        if (!pathFound)
        {
            return;
        }
        this.path = path;
        pathIndex = 0;
        followPath = true;
    }

    private void FollowPath()
    {
        if (path.Count <= pathIndex)
        {
            followPath = false;
            pathIndex = 0;
            Debug.Log(path.Last().x + " " + path.Last().y);
            return;
        }

        Vector3 pos = this.transform.position;
        Vector3 targetPos = path[pathIndex];
        double sqareDistance = (pos.x * pos.x + pos.y * pos.y + pos.z * pos.z) - (targetPos.x * targetPos.x + targetPos.y * targetPos.y + targetPos.z * targetPos.z);
        // itt el�g �sszehasonl�t�shoz csak a t�vols�g n�gyzete is ez�rt a gy�kvon�st le hagyva annak a sz�m�t�s�t le lehet sp�rolni �s jobb lesz a teljes�tm�ny
        this.transform.position = Vector2.MoveTowards(this.transform.position, path[pathIndex], speed * Time.deltaTime);
        if ( !(sqareDistance > 0.01 || sqareDistance <  -0.01) ) 
        {
            pathIndex++;
        }
    }
}
