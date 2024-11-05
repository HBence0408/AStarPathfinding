using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Profiling;
using System.Threading.Tasks;

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
        }
        else
        {
            Debug.LogWarning("multiple seeker managers, destroying self");
            Destroy(this);
        }
    }

    private void Start()
    {
        for (int i = 0; i < maxSeekers; i++)
        {
            GameObject go = Instantiate(seeker, this.transform.position, this.transform.rotation);
            go.name = ("seeker" + i).ToString();
            seekers[i] = go.GetComponent<TestUnit>();
            seekerDatas[i] = new SeekerData(go.transform, target, seekers[i]);
        }
    }

    public void TargetPathChange()
    {
        for (int i = 0; i < maxSeekers; i++)
        {
            seekerDatas[i].requestPath = true;
        } 
    }

    private void Update()
    { 
        for (int i = 0; i < maxSeekers; i++)
        {
            seekers[i].Poll();
        }
        
        for (int i = 0; i < 100; i++)
        {;
            seekerDatas[Iterations].Update();
            Iterations++;
        }
    }
}
