using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainUpdater : MonoBehaviour {
    Terrain terr; // terrain to modify 

    private float[,] targetHeightMap;

    public void SetHeightMap(float[,] map)
    {
        targetHeightMap = map;
    }

    void Start()
    {
        terr = Terrain.activeTerrain;
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
        if (targetHeightMap != null)
            terr.terrainData.SetHeights(0,0,targetHeightMap);
        

    }

}
