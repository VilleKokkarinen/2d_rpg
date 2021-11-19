using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType { START,GOAL,WATER,GRASS,SAND,PATH }
public class AStar : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    private List<Vector3Int> waterTiles = new List<Vector3Int>();

    private Vector3Int startPos, goalPos;

    private Node current;

    private HashSet<Node> openList;

    private HashSet<Node> closedList;

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    private Stack<Vector3> path;

    public Tilemap Tilemap { get => tilemap; }

    public static HashSet<Vector3Int> NoDiagonalTiles { get; } = new HashSet<Vector3Int>();

    public Stack<Vector3> Algorithm(Vector3 position, Vector3 goal)
    {       
        startPos = tilemap.WorldToCell(position);
        goalPos = tilemap.WorldToCell(goal);

        startPos.z = 0;
        goalPos.z = 0;

        current = GetNode(startPos);

        openList = new HashSet<Node>();

        closedList = new HashSet<Node>();

        openList.Add(current);

        foreach (KeyValuePair<Vector3Int, Node> node in allNodes)
        {
            node.Value.Parent = null;
        }

        allNodes.Clear();

        path = null;

        int count = 0;
        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbors = FindNeighbours(current.Position);

            ExamineNeighbours(neighbors, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);

            count++;

            if(count >= 5000)
            {
                break;
            }
        }

        if(path != null)
        {
            return path;
        }

        return null;
    }

    private Node GetNode(Vector3Int Position)
    {
        if (allNodes.ContainsKey(Position))
        {
            return allNodes[Position];
        }
        else
        {
            Node node = new Node(Position);
            
            allNodes.Add(Position,node);

            return node;
            
        }
    }

    private List<Node> FindNeighbours(Vector3Int parentPosition)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(y != 0 || x != 0)
                {
                    Vector3Int neighbourPosition = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);

                    if (neighbourPosition != startPos && !GameManager.Instance.Blocked.Contains(neighbourPosition))
                    {
                        Node neighbour = GetNode(neighbourPosition);
                        neighbours.Add(neighbour);
                    }                        
                }
            }
        }
        return neighbours;
    }

    private void ExamineNeighbours(List<Node> neighbours, Node currentNode)
    {
        for (int i = 0; i < neighbours.Count; i++)
        {
            Node neighbor = neighbours[i];

            if (!ConnectedDiagonally(current, neighbor))
            {
                continue;
            }

            int gScore = DetermineGScore(neighbours[i].Position, currentNode.Position);

            
            if(gScore == 14 && NoDiagonalTiles.Contains(neighbor.Position) && NoDiagonalTiles.Contains(current.Position))
            {
                continue;
            }
            

            if (openList.Contains(neighbor))
            {
                if(currentNode.G + gScore < neighbor.G)
                {
                    CalcValues(currentNode, neighbor, gScore);
                }
            }
            else if (!closedList.Contains(neighbor))
            {
                CalcValues(currentNode, neighbor, gScore);

                openList.Add(neighbor);
            }
        }
    }

    private void CalcValues(Node parent, Node neighbour, int cost)
    {
        neighbour.Parent = parent;

        neighbour.G = parent.G + cost;

        neighbour.H = ((Math.Abs((neighbour.Position.x - goalPos.x)) + Math.Abs((neighbour.Position.y - goalPos.y)))*10);

        neighbour.F = neighbour.G + neighbour.H;
    }

    private int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;

        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;

        if(Math.Abs(x-y) % 2 == 1)
        {
            gScore = 10;
        }
        else
        {
            gScore = 14;
        }

        return gScore;
    }

    private void UpdateCurrentTile(ref Node currentNode)
    {
        openList.Remove(currentNode);
        closedList.Add(current);

        if(openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
    }

    private bool ConnectedDiagonally(Node currentNode, Node neighbour)
    {
        Vector3Int direct = currentNode.Position - neighbour.Position;
        Vector3Int first = new Vector3Int(current.Position.x + (direct.x *-1), current.Position.y, current.Position.z);
        Vector3Int second = new Vector3Int(current.Position.x, current.Position.y + (direct.y * -1), current.Position.z);

        if(waterTiles.Contains(first) || waterTiles.Contains(second))
        {
            return false;
        }
        return true;

    }

    private Stack<Vector3> GeneratePath(Node current)
    {
        if(current.Position == goalPos)
        {
            Stack<Vector3> finalPath = new Stack<Vector3>();

            int count = 0;
            while(current != null)
            {
                finalPath.Push(tilemap.CellToWorld(current.Position));
                current = current.Parent;
                count++;

                if(count >= 5000)
                {
                    break;
                }
            }
            return finalPath;
        }
        return null;
    }
}
