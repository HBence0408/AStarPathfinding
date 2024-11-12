using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float spawnRate;
    [SerializeField] private float spawnCounter;
    [SerializeField] private float spawnRadius;
    [SerializeField] private GameObject spawnGO; //not yet implemented
    

    private void Update()
    {
        if (spawnCounter > spawnRate)
        {
            Spawn();
            spawnCounter = 0;
           
        }
        else
        {
            spawnCounter += Time.deltaTime;
        }
    }

    private void Spawn()
    {
        Profiler.BeginSample("random point");
        //Debug.Log("spawning");
        Vector2 rndSpawn = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPoint = new Vector3(rndSpawn.x, rndSpawn.y, 0) + this.transform.position;
        Profiler.EndSample();
        //Task spawning = Task.Run(() => SeekerManager.Instance.AddSeeker(spawnPoint));
        Profiler.BeginSample("spawning");
        SeekerManager.Instance.AddSeeker(spawnPoint);
        Profiler.EndSample();
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, spawnRadius);
    }
}
