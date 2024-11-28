using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using static UnityEngine.Rendering.DebugUI;

public class TestUnit : MonoBehaviour, ISeeker
{
    [SerializeField] private float speed;
    [SerializeField] private Transform target;
    private List<Vector3> path;
    [SerializeField] private bool followPath = false;
    private int pathIndex;
    public SeekerData data;
    private bool isReady = false;
    private bool isActive;

    public bool IsActive()
    {
         return isActive;  
    }

    // public SeekerData Data { get => data; }

    private void Update()
    {
       
    }

    private void Start()
    {
        isReady = true;
    }

    public void Poll()
    {
        Debug.Log("unit poll");
        if (!isReady)
        {
            return;
        }
        if (followPath)
        {
            FollowPath();
        }
       
        
    }

    public void SetData(SeekerData data)
    {
        this.data = data;
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

        try
        {
            Vector3 pos = this.transform.position;
            Vector3 targetPos = path[pathIndex];
            double sqareDistance = (pos.x * pos.x + pos.y * pos.y + pos.z * pos.z) - (targetPos.x * targetPos.x + targetPos.y * targetPos.y + targetPos.z * targetPos.z);
            // itt elég összehasonlításhoz csak a távolság négyzete is ezért a gyökvonást le hagyva annak a számítását le lehet spórolni és jobb lesz a teljesítmény
            this.transform.position = Vector2.MoveTowards(this.transform.position, path[pathIndex], speed * Time.deltaTime);
            if (!(sqareDistance > 0.01 || sqareDistance < -0.01))
            {
                pathIndex++;
            }
        }
        catch (Exception)
        {

            Debug.Log("not moving");
        }

       
    }

    public void SetActive(bool isActive)
    {
        if (isActive)
        {
            this.gameObject.SetActive(true);
            this.isActive = true;
        }
        else
        {
            this.isActive = false;
            this.gameObject.SetActive(false);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    
    private void OnDestroy()
    {
        Debug.Log("destroying self");
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //Debug.Log("tessUnit trigger entered by:" + collision);
        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            //SeekerManager.Instance.DestroySeeker();
            SetActive(false);
            UIManager.Instance.AddPoint();
            //Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            Player.GetHit();
            SetActive(false);
            //SeekerManager.Instance.DestroySeeker(data);
            //Destroy(this.gameObject);
        }

    }
}
