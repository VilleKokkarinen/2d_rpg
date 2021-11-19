using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform map;

    [SerializeField]
    private Texture2D[] mapData;

    [SerializeField]
    private MapElement[] mapElements;

    [SerializeField]
    private Sprite defaultTile;

    private Dictionary<Point, GameObject> waterTiles = new Dictionary<Point, GameObject>();

    private Vector3 WorldStartPosition
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap()
    {
        int height = mapData[0].height;
        int width = mapData[0].width;

        for(int i = 0; i < mapData.Length; i++) // layers of map
        {
            for (int x = 0; x < mapData[i].width; x++)
            {
                for (int y = 0; y < mapData[i].height; y++)
                {
                    Color c = mapData[i].GetPixel(x, y);

                    MapElement newElement = Array.Find(mapElements, e => e.Color == c);
                                      
                    if(newElement != null)
                    {
                        float xPos = WorldStartPosition.x + (defaultTile.bounds.size.x * x);
                        float yPos = WorldStartPosition.y + (defaultTile.bounds.size.y * y);

                        GameObject go = Instantiate(newElement.ElementPrefab);

                        go.transform.position = new Vector2(xPos,yPos);


                        if (newElement.TileTag == "Water")
                        {
                            waterTiles.Add(new Point(x, y), go);
                        }

                        if (newElement.TileTag == "Tree")
                        {
                            go.GetComponent<SpriteRenderer>().sortingOrder = height*2-y*2;
                        }

                        go.transform.parent = map;

                    }
                }
            }
        }

        CheckWater();
    }

    private string TileCheck(Point currentPoint)
    {
        string composition = string.Empty;

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x != 0 || y != 0)
                {
                    if(waterTiles.ContainsKey(new Point(currentPoint.X + x, currentPoint.Y + y)))
                    {
                        composition += "W";
                    }
                    else
                    {
                        composition += "E";
                    }
                }
            }
        }

        return composition;
    }

    private void CheckWater()
    {
        foreach(KeyValuePair<Point, GameObject> tile in waterTiles)
        {
            string composition = TileCheck(tile.Key);
        }
     
    }
}

[Serializable]
public class MapElement
{
    [SerializeField]
    private string tileTag;

    [SerializeField]
    private Color color;

    [SerializeField]
    private GameObject _elementPrefab;

    public GameObject ElementPrefab { get => _elementPrefab; }

    public Color Color { get => color; }

    public string TileTag { get => tileTag; }
}

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}