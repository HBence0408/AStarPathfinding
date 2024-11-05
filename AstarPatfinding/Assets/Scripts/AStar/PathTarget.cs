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

    void Update()
    {
        previousNodePos = currentNodePos;
        currentNodePos = grid.FindNode(this.transform.position);
        time = PulseSeekers(currentNodePos, previousNodePos,time);
    }
    
    private double PulseSeekers(Node currentPosition, Node previousPosition, double checkTime)
    {
        if (!currentPosition.Equals(previousPosition) && checkTime > 0.1)
        {
            checkTime = 0;
            SeekerManager.Instance.TargetPathChange();
        }
        else
        {
            checkTime += Time.deltaTime;
        }
        return checkTime;
    }
}
