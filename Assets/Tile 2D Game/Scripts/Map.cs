using System.Linq;
using UnityEngine;

public enum TileTypes
{
    Empty = -1,

    // 0, 14
    Grass = 15,
    Forest = 16,
    Hills = 17,
    Mountains = 18,
    Towns= 19,
    Castle = 20,
    Monster= 21,
}

public class Map
{
    public int rows = 0;
    public int cols = 0;

    public Tile[] tiles;

    public Tile[] CoastTiles => tiles.Where(t => t.autoTileId >= 0 && t.autoTileId < (int)TileTypes.Grass).ToArray();
    public Tile[] LandTiles => tiles.Where(t => t.autoTileId >= (int)TileTypes.Grass).ToArray();
    public Tile[] ForestTiles => tiles.Where(t => t.autoTileId == (int)TileTypes.Forest).ToArray();
    public Tile[] HillsTiles => tiles.Where(t => t.autoTileId == (int)TileTypes.Hills).ToArray();
    public Tile[] MountainsTiles => tiles.Where(t => t.autoTileId == (int)TileTypes.Mountains).ToArray();
    public Tile[] TownsTiles => tiles.Where(t => t.autoTileId == (int)TileTypes.Towns).ToArray();
    public Tile[] CastleTiles => tiles.Where(t => t.autoTileId == (int)TileTypes.Castle).ToArray();
    public Tile[] MonsterTiles => tiles.Where(t => t.autoTileId == (int)TileTypes.Monster).ToArray();

    public void Init(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;

        tiles = new Tile[rows * cols];
        for (int i = 0; i < tiles.Length; ++i)
        {
            tiles[i] = new Tile { id = i };
        }

        for (int r = 0; r < rows; ++r)
        {
            for (int c = 0; c < rows; ++c)
            {
                int index = r * cols + c;
                var adjacents = tiles[index].adjacents;
                if ((r - 1) >= 0)
                {
                    adjacents[(int)Sides.Top] = tiles[index - cols];
                }
                if ((c + 1) < cols)
                {
                    adjacents[(int)Sides.Right] = tiles[index + 1];
                }
                if ((c - 1) >= 0)
                {
                    adjacents[(int)Sides.Left] = tiles[index - 1];
                }
                if ((r + 1) < rows)
                {
                    adjacents[(int)Sides.Bottom] = tiles[index + cols];
                }
            }
        }

        for (int i = 0; i < tiles.Length; ++i)
        {
            tiles[i].UpdateAutoTileId();
        }
    }

    public bool CreateIsland(float erodePercent, float erodeIterations, float lakePercent)
    {
        DecorateTiles(LandTiles, lakePercent, TileTypes.Empty);

        for (int i = 0; i < erodeIterations; ++i)
        {
            DecorateTiles(CoastTiles, erodePercent, TileTypes.Empty);
        }
        return true;
    }

    public void DecorateTiles(Tile[] tiles, float percent, TileTypes tileType)
    {
        int total = Mathf.FloorToInt(tiles.Length * percent);

        ShuffleTiles(tiles);

        for (int i = 0; i < total; ++i)
        {
            if (tileType == TileTypes.Empty)
            {
                tiles[i].ClearAdjacents();
            }
            tiles[i].autoTileId = (int)tileType;
        }
    }

    public void ShuffleTiles(Tile[] tiles)
    {
        // 피셔 예이츠 셔플 알고리즘
        for (int i = tiles.Length - 1; i > 0; --i)
        {
            int index = Random.Range(0, i + 1);
            Tile temp = tiles[i];
            tiles[i] = tiles[index];
            tiles[index] = temp;
        }
    }
}
