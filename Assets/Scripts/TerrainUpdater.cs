using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainUpdater : MonoBehaviour {
    Terrain terr; // terrain to modify 
    NetworkManager netManager;

    private float[,] targetHeightMap;
    private bool freshMap = false;

    public void SetHeightMap(float[,] map)
    {
        targetHeightMap = map;
        freshMap = true;
    }

    void Start()
    {
        terr = Terrain.activeTerrain;
        netManager = GameObject.FindObjectOfType<NetworkManager>();
    }

    void Update()
    {
        /*
        if (!networkClient.freshMap)
            return;

        SetHeightMap(networkClient.getHeightData());
        networkClient.freshMap = false;
        /*
        TODO: smooth out change between heights
        float[,] heights = terr.terrainData.GetHeights(0,0,hmWidth,hmWidth);
        */

        // set the new height
        if (freshMap == true)
        {
            //terr.terrainData.SetHeights(0, 0, targetHeightMap);
            freshMap = false;
        }
    }

    public void DebugUpdateMap()
    {
        Debug.Log("Requesting Map update");
        terr.terrainData.SetHeights(0, 0, ConvertMap(netManager.RequestHeightMap()));
        Debug.Log("Updated Map");
    }

    static private float[,] ConvertMap(byte[][] mapData)
    {
        float[,] map = new float[480, 640];

        for (int y = 0; y < 480; ++y)
        {
            for (int x = 0; x < 640; ++x)
            {
                map[y, x] = mapData[y][x] / 255f;
            }
        }

        return map;
    }

}
