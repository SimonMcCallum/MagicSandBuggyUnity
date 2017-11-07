using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTerrainShader : MonoBehaviour
{
    Terrain terr; // terrain to modify 
    Material mat;
    Texture2D tex;
    byte[] formatedMap;

    int mapRes;

    // Use this for initialization
    void Start () {
        terr = Terrain.activeTerrain;
        mat = terr.materialTemplate;
        mapRes = terr.terrainData.heightmapResolution;
        tex = new Texture2D(mapRes, mapRes,
            TextureFormat.Alpha8, false);
        formatedMap = new byte[mapRes * mapRes];
    }
	
	// Update is called once per frame
	void Update () {
        float [,] map = terr.terrainData.GetHeights(0, 0, mapRes, mapRes);
        for (int y = 0; y < mapRes; y++)
        {
            for (int x = 0; x < mapRes; x++)
            {
                formatedMap[y * mapRes + x] = (byte) (map[y, x]*255);
            }
        }
        tex.LoadRawTextureData(formatedMap);
        tex.Apply();
        mat.SetTexture("_HeightMap", tex);
	}
}
