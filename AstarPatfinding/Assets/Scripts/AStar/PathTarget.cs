using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTarget : MonoBehaviour
{
    private Node currentNodePos;
    private Node previousNodePos;
    [SerializeField] private PathFinderGrid grid;
    private double time = 0;

    void Start()
    {
        currentNodePos = grid.FindNode(this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
        previousNodePos = currentNodePos;
        currentNodePos = grid.FindNode(this.transform.position);

        if (!currentNodePos.Equals(previousNodePos) && time > 0.1)
        {
            time = 0;
            TestUnit.TargetPathChange();
        }
        else
        {
            time += Time.deltaTime;
        }
        

    }
}
