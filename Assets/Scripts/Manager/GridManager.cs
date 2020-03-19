using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    static public GridManager instance;
    [SerializeField] bool isCreateMode = false;
    [SerializeField] int width = 0;
    [SerializeField] int height = 0;
    [SerializeField] float tileWidth = 0f;
    [SerializeField] float tileHeight = 0f;
    [SerializeField] Tile[] tileData = null;
    List<Tile> tileDataList;
    Tile[,] tiles;
    GameObject tileHolder;

    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public float TileWidth { get => tileWidth; set => tileWidth = value; }
    public float TileHeight { get => tileHeight; set => tileHeight = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        if (tileData.Length > 0)
        {
            tileDataList = new List<Tile>(tileData);
        }
        tiles = new Tile[height, width];
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0, size = gameObjects.Length; i < size; ++i)
        {
            tiles[i / Width, i % Width] = gameObjects[i].GetComponent<Tile>();
        }
    }

    public void Start()
    {
        Debug.Log(Application.dataPath);
    }

    public void CreateGrid()
    {
        tileDataList = new List<Tile>(tileData);
        GameObject holder = GameObject.Find("TileHolder");
        if (holder != null)
        {
            DestroyImmediate(holder);
        }

        tileHolder = new GameObject("TileHolder");

        if (!isCreateMode)
        {
            IEnumerable<string> test = File.ReadLines(Application.dataPath + "/Data/TestMap.txt");
            List<string> t = new List<string>(test);
            int yIndex = 0;
            for (int i = 0; i < Height; ++i)
            {
                int xIndex = 0;
                string[] tt = t[i].Split(',');
                foreach (string ttt in tt)
                {
                    int tileID = int.Parse(ttt);
                    Tile result = tileDataList.Find((Tile tile) => { return tile.ID == tileID; });
                    if (result != null)
                    {
                        Tile temp = Instantiate(result);
                        temp.name = "Tile (" + xIndex.ToString() + ", " + yIndex.ToString() + ")";
                        temp.transform.SetParent(tileHolder.transform);
                        float posX = TileWidth * (xIndex + 0.5f);
                        float posZ = TileHeight * (yIndex + 0.5f);
                        temp.transform.position = new Vector3(posX, 0, posZ);
                        ++xIndex;
                    }
                }
                ++yIndex;
            }
        }
        else
        {
            for (int j = 0; j < Height; ++j)
            {
                for (int i = 0; i < Width; ++i)
                {
                    GameObject temp = new GameObject("TestTile");
                    temp.tag = "Tile";
                    temp.transform.SetParent(tileHolder.transform);
                    Tile t = temp.AddComponent<Tile>();
                    t.Info.tileType = TileType.PlayerWalkable;
                    float posX = TileWidth * (i + 0.5f);
                    float posZ = TileHeight * (j + 0.5f);
                    temp.transform.position = new Vector3(posX, 0, posZ);
                }
            }
        }
    }

    public void SaveTileMapData()
    {
        tiles = new Tile[height, width];
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0, size = gameObjects.Length; i < size; ++i)
        {
            tiles[i / Width, i % Width] = gameObjects[i].GetComponent<Tile>();
        }
        StringWriter stringWriter = new StringWriter();
        for (int y = 0; y < height; ++y)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int x = 0; x < width; ++x)
            {
                if (x == 0)
                {
                    builder.Append(tiles[y, x].ID.ToString());
                }
                else
                {
                    builder.Append(", " + tiles[y, x].ID.ToString());
                }
            }
            stringWriter.WriteLine(builder.ToString());
        }
        File.WriteAllText(Application.dataPath + "/Data/TestMap.txt", stringWriter.ToString());
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
        {
            return null;
        }
        return tiles[y, x];
    }

    public Tile GetTile (Vector3 position)
    {
        if (position.x < 0 || position.y < 0 || position.x >= width * tileWidth || position.y >= height * tileHeight)
        {
            return null;
        }


        return tiles[(int)(position.y / tileHeight), (int)(position.x / tileWidth)];
    }

    private void OnDrawGizmos()
    {
        // 수평선 그리기
        for (int row = 0; row < height + 1; ++row)
        {
            Debug.DrawLine(new Vector3(0, 0, tileHeight * row), new Vector3(TileWidth * Width, 0, TileHeight * row), Color.yellow);
        }

        // 수직선
        for (int col = 0; col < Width + 1; ++col)
        {
            Debug.DrawLine(new Vector3(TileWidth * col, 0, 0), new Vector3(TileWidth * col, 0, TileHeight * Height), Color.yellow);
        }
    }
}
