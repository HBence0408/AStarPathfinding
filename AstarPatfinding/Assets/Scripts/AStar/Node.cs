using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node: IComparable, ICloneable
{
    public Node(bool walkable, Vector3 worldPosition, int i, int j)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.i = i;
        this.j = j;
    }

    private bool walkable;
    private Vector3 worldPosition;
    private int i;
    private int j;
    private int hCost;
    private int gCost;
    private Node parent;

    public bool Walkable { get => walkable; }
    public Vector3 WorldPosition { get => worldPosition; }
    public int I { get => i; }
    public int J { get => j; }
    public int HCost { get => hCost; set => hCost = value; }
    public int GCost { get => gCost; set => gCost = value; }
    public int FCost { get => hCost + gCost; }
    public Node Parent { get => parent; set => parent = value; }

    public override bool Equals(object obj)
    {
        /*
        if (this.GetHashCode() != obj.GetHashCode())
        {
            return false;
        }
        */
        if (obj is null)
        {
            return false;
        }
        if (obj is Node)
        {
            if (this.worldPosition == (obj as Node).worldPosition)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }


    public int CompareTo(object obj)
    {
        if (obj == null || obj is not Node)
        {
            Debug.LogWarning(obj + " cannot be compared to Node");
        }

        Node n = obj as Node;
        if (this.FCost == n.FCost)
        {
            if (this.HCost > n.HCost)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        else
        {
            if (this.FCost > n.FCost)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        
    
    }
    
    public object Clone()
    {
        Node n = new Node(this.walkable, this.worldPosition, this.i, this.j);
        return n;
    }

    /*
    public override int GetHashCode()
    {
        return HashCode.Combine(HCost, FCost);
    }
    */
}
