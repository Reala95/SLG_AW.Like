using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class TerrainTile : UnityEngine.Tilemaps.Tile
{
    public Sprite[] spritesSheet;
    public bool checkDiagonalTiles;
    public Sprite[] spritesDiagonalInCase5;
    public Sprite[] spritesDiagonalInCase6;
    public Sprite[] spritesDiagonalInCase7;
    public Sprite[] spritesDiagonalInCase9;
    public Sprite[] spritesDiagonalInCase10;
    public Sprite[] spritesDiagonalInCase11;
    public Sprite[] spritesDiagonalInCase13;
    public Sprite[] spritesDiagonalInCase14;
    public Sprite[] spritesDiagonalInCase15;
    public string[] similarTile;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                Vector3Int nearGrids = new Vector3Int(position.x + i, position.y + j, 0);
                if(isSameTile(tilemap, nearGrids))
                {
                    tilemap.RefreshTile(nearGrids);
                    tilemap.RefreshTile(position);
                }
            }
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
    /*
     * Index number set up:
     * bin: 0000 -> right, left, down, up
     * e.g: when index = 5 (bin:0101) -> the girds on top and on left of this grid have the same id.
     */
        int i = 0;
        i += isSameTile(tilemap, position + new Vector3Int(0, 1, 0)) ? 1 : 0;
        i += isSameTile(tilemap, position + new Vector3Int(0, -1, 0)) ? 2 : 0;
        i += isSameTile(tilemap, position + new Vector3Int(-1, 0, 0)) ? 4 : 0;
        i += isSameTile(tilemap, position + new Vector3Int(1, 0, 0)) ? 8 : 0;
        if (checkDiagonalTiles) secondTileChange(position, tilemap, ref tileData, i);
        else tileData.sprite = spritesSheet[Mathf.Min(spritesSheet.Length - 1, i)];
    }

    private bool isSameTile(ITilemap tilemap, Vector3Int position)
    {
        if (tilemap.GetTile<TerrainTile>(position) != null)
            //return tilemap.GetTile<TerrainTile>(position).getTerrainId() == this.terrainId;
            return similarTile.Contains(tilemap.GetTile<TerrainTile>(position).getTerrainId());
        else return false;
    }

    private void secondTileChange(Vector3Int position, ITilemap tilemap, ref TileData tileData, int i)
    {
        int j = 0;
        switch (i)
        {
            case 5:
                j += !isSameTile(tilemap, position + new Vector3Int(-1, 1, 0)) ? 1 : 0;
                tileData.sprite = spritesDiagonalInCase5[j];
                break;
            case 6:
                j += !isSameTile(tilemap, position + new Vector3Int(-1, -1, 0)) ? 1 : 0;
                tileData.sprite = spritesDiagonalInCase6[j];
                break;
            case 7:
                j += !isSameTile(tilemap, position + new Vector3Int(-1, -1, 0)) ? 1 : 0;
                j += !isSameTile(tilemap, position + new Vector3Int(-1, 1, 0)) ? 2 : 0;
                tileData.sprite = spritesDiagonalInCase7[j];
                break;
            case 9:
                j += !isSameTile(tilemap, position + new Vector3Int(1, 1, 0)) ? 1 : 0;
                tileData.sprite = spritesDiagonalInCase9[j];
                break;
            case 10:
                j += !isSameTile(tilemap, position + new Vector3Int(1, -1, 0)) ? 1 : 0;
                tileData.sprite = spritesDiagonalInCase10[j];
                break;
            case 11:
                j += !isSameTile(tilemap, position + new Vector3Int(1, -1, 0)) ? 1 : 0;
                j += !isSameTile(tilemap, position + new Vector3Int(1, 1, 0)) ? 2 : 0;
                tileData.sprite = spritesDiagonalInCase11[j];
                break;
            case 13:
                j += !isSameTile(tilemap, position + new Vector3Int(1, 1, 0)) ? 1 : 0;
                j += !isSameTile(tilemap, position + new Vector3Int(-1, 1, 0)) ? 2 : 0;
                tileData.sprite = spritesDiagonalInCase13[j];
                break;
            case 14:
                j += !isSameTile(tilemap, position + new Vector3Int(1, -1, 0)) ? 1 : 0;
                j += !isSameTile(tilemap, position + new Vector3Int(-1, -1, 0)) ? 2 : 0;
                tileData.sprite = spritesDiagonalInCase14[j];
                break;
            case 15:
                j += !isSameTile(tilemap, position + new Vector3Int(-1, 1, 0)) ? 1 : 0;
                j += !isSameTile(tilemap, position + new Vector3Int(1, 1, 0)) ? 2 : 0;
                j += !isSameTile(tilemap, position + new Vector3Int(-1, -1, 0)) ? 4 : 0;
                j += !isSameTile(tilemap, position + new Vector3Int(1, -1, 0)) ? 8 : 0;
                tileData.sprite = spritesDiagonalInCase15[j];
                break;
            default:
                tileData.sprite = spritesSheet[Mathf.Min(spritesSheet.Length - 1, i)];
                break;
        }
    }

    // Terrain data used in game play
    public string terrainId;
    public int terrainBonus;

    public string getTerrainId()
    {
        return this.terrainId;
    }

    public int getTerrainBonus()
    {
        return this.terrainBonus;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/TerrainTile")]
    public static void CreateTerrainTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Tile", "NormalSet_", "Asset", "Save Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TerrainTile>(), path);
    }
#endif
}
