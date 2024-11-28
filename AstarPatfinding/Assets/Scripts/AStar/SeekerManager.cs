using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using System.Threading;

public class SeekerManager : MonoBehaviour
{
    static private SeekerManager instance =  null;
    private const int maxSeekers = 1000;
    [SerializeField] private GameObject seeker;
    public static SeekerManager Instance { get => instance ; private set => instance = value; }
    private ISeeker[] seekers = new ISeeker[maxSeekers];
    private SeekerData[] seekerDatas = new SeekerData[maxSeekers];
    //private bool requestNewPath = true;
    [SerializeField] private Transform target;
    private IntStack freeSpaces = new IntStack(maxSeekers);
    private List<int> free2 = new List<int>();
    [SerializeField] private int deadSeekers = 0;
    [SerializeField] private int currentSeekers = 0;
    private Quaternion rotation;
    private int iterations = 0;
    private  int Iterations
    {
        get
        {
            return iterations;
        }
       set
        {
            if (value > maxSeekers-1)
            {
                iterations = 0;
            }
            else
            {
                iterations = value;
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            rotation = this.transform.rotation;
        }
        else
        {
            Debug.LogWarning("multiple seeker managers, destroying self");
            Destroy(this);
        }

        for (int i = maxSeekers-1; i >= 0; i--)
        {
            freeSpaces.Push(i);
           // Debug.Log("pushed "+i);
        }
    }


    // ez spawner tulajdonság, külön spawner kell

    
    private void Start()
    {
        // object pooling   
        for (int i = 0; i < maxSeekers; i++)
        {
            GameObject go = Instantiate(seeker, this.transform.position, this.transform.rotation);
            go.name = ("seeker" + i).ToString();
            seekers[i] = go.GetComponent<ISeeker>();
            seekerDatas[i] = new SeekerData(go.transform, target, seekers[i]);
            seekers[i].SetTarget(target);
            seekers[i].SetActive(false);
        }
        
    }
    
    public void AddSeeker(Vector3 posisiton)
    {
        
        int seekerIndex = -10;
        for (int i = 0; i < maxSeekers; i++)
        {
            if (!seekers[i].IsActive())
            {
                seekerIndex = i;
            }
        }
        if (seekerIndex == -10)
        {
            return;
        }
        seekers[seekerIndex].SetActive(true);
        seekerDatas[seekerIndex].TeleportToPosition(posisiton);

        /*
        GameObject go = Instantiate(seeker, posisiton, rotation);
        go.name = ("seeker " + seekerIndex).ToString();
        seekers[seekerIndex] = go.GetComponent<ISeeker>();
        seekers[seekerIndex].SetTarget(target);
       // Debug.Log("spawning transfor:" + go.transform);
        seekerDatas[seekerIndex] = new SeekerData(go.transform, target, seekers[seekerIndex]);
        currentSeekers++;
        //return (seekerDatas[seekerIndex], seekers[seekerIndex]);
        */
    }

    

    /*
    public void DestroySeekerData(ISeeker seeker)
    {
        for (int i = 0; i < maxSeekers; i++)
        {
            if (seekers[i].Equals(seeker))
            {
                freeSpaces.Push(i);
                return;
            }
        }
    }
    */
    public void DestroySeeker(ISeeker seeker)
    {
        
        for (int i = 0; i < maxSeekers; i++)
        {
            if (seekers[i].Equals(seeker))
            {
                seekers[i].SetActive(false);
                /*
                seekers[i] = null;
                seekerDatas[i].DeleteScript();
                // seekerDatas[i].IsAlive = false;
                //freeSpaces.Push(i);
                deadSeekers++;
                
                return;
                */
                //UIManager.Instance.AddPoint();
                return;
            }
        }  
    }

    public void TargetPathChange()
    {
        for (int i = 0; i < maxSeekers; i++)
        {
            Debug.Log("new pazh");
            seekerDatas[i].requestPath = true;
        } 
    }

    private void Update()
    { 
        for (int i = 0; i < maxSeekers; i++)
        {
            if (seekers[i].IsActive())
            {
                seekers[i].Poll();
            }
        }
        
        for (int i = 0; i < 100; i++)
        {
            if (seekers[Iterations].IsActive())
            {
                seekerDatas[Iterations].Update();
                Debug.Log("update");
            }
            Iterations++;
        }
    }
}
