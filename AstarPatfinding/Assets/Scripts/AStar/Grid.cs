using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PathFinderGrid : MonoBehaviour
{
    private Node[,] grid;
    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float gridCellSize;
    [SerializeField] private LayerMask walkableLayerMask;
    [SerializeField] private LayerMask unWalkableLayerMask;
    [SerializeField] private Vector3 bottomLeftCell;
    [SerializeField] private bool showMeTheMatrix;

    private Vector3 CalculateBottomLeftCell()
    {
        return new Vector3(transform.position.x - gridWorldSize.x / 2 + gridCellSize / 2, transform.position.y - gridWorldSize.y / 2 + gridCellSize / 2, 0);
    }

    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new Node[Mathf.RoundToInt(gridWorldSize.x), Mathf.RoundToInt(gridWorldSize.y)];
        bottomLeftCell = CalculateBottomLeftCell();
        Vector3 currentCell = CalculateBottomLeftCell();

        for (int i = 0; i < Mathf.RoundToInt(gridWorldSize.x); i++)
        {
            Debug.Log(grid.Length);
            currentCell.y = bottomLeftCell.y;
            for (int j = 0; j < Mathf.RoundToInt(gridWorldSize.y); j++)
            {
                bool isWalkable = !Physics2D.OverlapCircle(new Vector2(currentCell.x, currentCell.y), (gridCellSize / 2)-0.1f,unWalkableLayerMask);
                grid[i, j] = new Node(isWalkable,currentCell,i,j);

                currentCell.y += gridCellSize;
            }
            currentCell.x += gridCellSize;
        }
    }

    public List<Node> FindNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int i = -1; i <= 1 ; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                try
                {
                    neighbours.Add(grid[node.I + i, node.J + j].Clone() as Node);
                }
                catch (IndexOutOfRangeException)
                {

                    continue;
                }  
            }
        }
        return neighbours;
    }

    public Node FindNode(Vector3 point)
    {
        try
        {
            return grid[Mathf.RoundToInt( (point.x - bottomLeftCell.x) / gridCellSize),Mathf.RoundToInt( ((point.y) - bottomLeftCell.y) / gridCellSize)].Clone() as Node;
        }
        catch (Exception)
        {
            Debug.LogWarning("an error has ocured finding the node at: " + point.x + " " + point.y);
            return null;
        } 
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(CalculateBottomLeftCell() + new Vector3(gridWorldSize.x * gridCellSize / 2, gridWorldSize.y * gridCellSize / 2, 0), new Vector3(gridWorldSize.x * gridCellSize, gridWorldSize.y * gridCellSize, 1));
        Gizmos.color = Color.black;
        Gizmos.DrawCube(CalculateBottomLeftCell(), new Vector3(gridCellSize, gridCellSize, gridCellSize));
        if (showMeTheMatrix)
        {
            Gizmos.DrawCube(CalculateBottomLeftCell(), new Vector3(gridCellSize,gridCellSize,gridCellSize));
            if (grid != null)
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = n.Walkable ? Color.white : Color.red;
                    Gizmos.DrawCube(n.WorldPosition, new Vector3(gridCellSize / 2, gridCellSize / 2, gridCellSize));
                }
            }
            Gizmos.color = Color.black;   
        }
    }
}
