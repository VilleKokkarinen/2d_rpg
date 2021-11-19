using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarDebugger : MonoBehaviour
{
    private static AStarDebugger instance;
    public static AStarDebugger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AStarDebugger>();
            }

            return instance;
        }
    }

    [SerializeField]
    private Grid grid;

    private Tile tile;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Color openColor, closedColor, pathColor, currentcolor, startColor, goalColor;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private GameObject debugTextPrefab;

    private List<GameObject> debugObjects = new List<GameObject>();

    public void CreateTiles(HashSet<Node> openList, HashSet<Node> closedList,Dictionary<Vector3Int, Node> allNodes , Vector3Int start, Vector3Int goal, Stack<Vector3Int> path = null)
    {
        foreach (var go in debugObjects)
        {
            Destroy(go);
        }

        foreach (Node node in openList)
        {
            ColorTile(node.Position, openColor);
        }

        foreach (Node node in closedList)
        {
            ColorTile(node.Position, closedColor);
        }

        if(path != null)
        {
            foreach (var pos in path)
            {
                if(pos != start && pos != goal)
                {
                    ColorTile(pos, pathColor);
                }
            }
        }

        ColorTile(start, startColor);
        ColorTile(goal, goalColor);

        foreach (KeyValuePair<Vector3Int, Node> node in allNodes)
        {
            if(node.Value.Parent != null)
            {
                GameObject go = Instantiate(debugTextPrefab, canvas.transform);
                go.transform.position = grid.CellToWorld(node.Key);
                debugObjects.Add(go);
            }
        }
    }

    public void ColorTile(Vector3Int position, Color color)
    {
        tilemap.SetTile(position, tile);
        tilemap.SetTileFlags(position, TileFlags.None);
        tilemap.SetColor(position, color);
    }

    private void GenerateDebugText(Node node, DebugText debugText)
    {
        Vector3Int diretion = node.Parent.Position - node.Position;
        debugText.Arrow.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(diretion.y, diretion.x) * Mathf.Rad2Deg);
    }

    public void ShowHide()
    {
        canvas.gameObject.SetActive(canvas.isActiveAndEnabled);
        Color c = tilemap.color;

        c.a = c.a != 0 ? 0 : 1;
        tilemap.color = c;
    }

    public void Reset(Dictionary<Vector3Int, Node> allNodes)
    {
        foreach (var go in debugObjects)
        {
            Destroy(go);
        }
        debugObjects.Clear();

        foreach (var pos in allNodes.Keys)
        {
            tilemap.SetTile(pos, null);
        }
    }

}
