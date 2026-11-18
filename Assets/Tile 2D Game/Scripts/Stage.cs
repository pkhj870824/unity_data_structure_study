using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject[] tileObjs;

    public int mapWidth = 20;
    public int mapHeight = 20;

    public Vector2 tileSize = new Vector2(16, 16);

    public Sprite[] islandSprites;
    
    public Sprite[] fogSprites;

    private Map map;

    public Map Map => map;

    [Range(0f, 0.9f)]
    public float erodePercent = 0.3f;
    public float erodeIterations = 2;

    [Range(0f, 0.9f)]
    public float lakePercent = 0.1f;

    [Range(0f, 0.9f)]
    public float treePercent = 0.1f;

    [Range(0f, 0.9f)]
    public float hillPercent = 0.1f;

    [Range(0f, 0.9f)]
    public float moutainPercent = 0.1f;

    [Range(0f, 0.9f)]
    public float townPercent = 0.1f;

    [Range(0f, 0.9f)]
    public float monsterPercent = 0.1f;

    public int revealRange = 5;

    private void Awake()
    {
        ResetStage();
    }

    [ContextMenu("Reset Stage")]
    public void ResetStage()
    {
        map = new Map();
        bool succeed = false;
        int maxAttemp = 10;
        for (int i = 0; i < maxAttemp && !succeed; ++i)
        {
            map.Init(mapHeight, mapWidth);
            succeed = map.CreateIsland(
                erodePercent,
                erodeIterations,
                lakePercent,
                treePercent,
                hillPercent,
                moutainPercent,
                townPercent,
                monsterPercent
            );
        }

        if (!succeed)
        {
            Debug.LogError("맵 생성 실패");
            map = null;
            return;
        }

        CreateGrid();
        CreatePlayer();
    }

    public Vector3 firstTilePos;

    public GameObject playerPrefab;
    private Player player;

    public void RevealMap(int tileId)
    {
        
    }
    public void OnTileVisited(int tileId)
    {
        map.tiles[tileId].revealTile = true;
        Debug.Log(
            $"OnTileVisited({tileId}: ({tileId / mapWidth}, {tileId % mapHeight}) / {map.tiles[tileId].autoTileId})"
        );
    }

    private void CreatePlayer()
    {
        if (player != null)
        {
            Destroy(player.gameObject);
            player = null;
        }

        var newGo = Instantiate(playerPrefab);
        player = newGo.GetComponent<Player>();
    }

    private void CreateGrid()
    {
        if (tileObjs != null)
        {
            foreach (var tile in tileObjs)
            {
                Destroy(tile.gameObject);
            }
        }
        tileObjs = new GameObject[mapWidth * mapHeight];

        Vector3 pos = firstTilePos;
        float startX = pos.x;
        for (int i = 0; i < mapHeight; ++i)
        {
            for (int j = 0; j < mapWidth; ++j)
            {
                var tileId = i * mapWidth + j;
                var tile = map.tiles[tileId];

                var newGo = Instantiate(tilePrefab, transform);
                newGo.transform.position = pos;
                pos.x += tileSize.x;

                newGo.name = $"Tile {tileId} ({i}, {j})";
                tileObjs[tileId] = newGo;

                DecorateTile(tileId);
            }
            pos.x = startX;
            pos.y -= tileSize.y;
        }
    }

    public void DecorateTile(int tileId)
    {
        var tile = map.tiles[tileId];
        var tileGo = tileObjs[tileId];
        var ren = tileGo.GetComponent<SpriteRenderer>();
        if (tile.autoTileId != (int)TileTypes.Empty)
        {
            ren.sprite = islandSprites[tile.autoTileId];
        }
        else
        {
            ren.sprite = null;
        }
    }

    public int ScreenPosTileId(Vector3 screenPos, bool clamp = true)
    {
        Camera cam = Camera.main;
        screenPos.z = Mathf.Abs(transform.position.z - cam.transform.position.z);
        return WorldPosToTileId(Camera.main.ScreenToWorldPoint(screenPos), clamp);
    }

    public int WorldPosToTileId(Vector3 worldPos, bool clamp = true)
    {
        int x = Mathf.FloorToInt((worldPos.x - firstTilePos.x) / tileSize.x + 0.5f);
        int y = Mathf.FloorToInt((firstTilePos.y - worldPos.y) / tileSize.y + 0.5f);

        if (clamp)
        {
            x = Mathf.Clamp(x, 0, mapWidth - 1);
            y = Mathf.Clamp(y, 0, mapHeight - 1);
        }
        return y * mapWidth + x;
    }

    public Vector3 GetTilePos(int y, int x)
    {
        return firstTilePos + new Vector3(x * tileSize.x, -y * tileSize.y);
    }

    public Vector3 GetTilePos(int tileId)
    {
        return GetTilePos(tileId / mapWidth, tileId % mapWidth);
    }
}
