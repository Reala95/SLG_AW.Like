using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class TerrainTile : UnityEngine.Tilemaps.Tile
{
    public Sprite[] spritesSheet;
    public bool tileChange = false;
    

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
        if (i < spritesSheet.Length && tileChange) tileData.sprite = spritesSheet[i];
        else tileData.sprite = this.sprite;
    }

    private bool isSameTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
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
