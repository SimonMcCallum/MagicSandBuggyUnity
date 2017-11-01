using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class TerrainUpdater : MonoBehaviour {
    Terrain terr; // terrain to modify 
    NetworkManager netManager;

    private float[,] targetHeightMap;
    private Thread activeThread = null;

    public bool nullHighest = false;
    public bool scaletoFullRange = false;

    public void SetTargetHeightMap(float[,] map)
    {
        targetHeightMap = map;
    }

    void Start()
    {
        terr = Terrain.activeTerrain;
        netManager = GameObject.FindObjectOfType<NetworkManager>();
    }

    void Update()
    {
        if (activeThread == null)
        {
            activeThread = new Thread(updateMapDataAsync);
            activeThread.Start();
        } else if (activeThread.Join(0)) {
            activeThread = null;
            if (targetHeightMap != null)
                terr.terrainData.SetHeights(0, 0, targetHeightMap);
        }
    }

    private void OnDisable()
    {
        if (activeThread != null)
        {
            activeThread.Abort();
        }
    }



    public void updateMapDataAsync()
        // does network request and calculation on the data
    {
        try
        {
            float[,] freshMap = ConvertMap(netManager.RequestHeightMap());
            if (freshMap != null) 
                SetTargetHeightMap(freshMap);
        } catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    private float[,] ConvertMap(byte[][] mapData)
    {
        float[,] map = new float[480, 640];

        float lowest = 1;
        float highest = 0;

        for (int y = 0; y < 480; ++y)
        {
            for (int x = 0; x < 640; ++x)
            {
                if (nullHighest && mapData[y][x] == 0)
                {
                    map[y, x] = 0;
                } else
                {
                    map[y, x] = 1 - mapData[y][x] / 255f;
                }
                lowest = Mathf.Min(lowest, map[y, x]);
                highest = Mathf.Max(highest, map[y, x]);
            }
        }

        if (scaletoFullRange)
        {
            //scaling
            Debug.Log("hi:" + highest + " lo:" + lowest);
            float diff = highest - lowest;
            for (int y = 0; y < 480; ++y)
            {
                for (int x = 0; x < 640; ++x)
                {
                    map[y, x] -= lowest;
                    map[y, x] *= 2;
                }
            }
        }


        return map;
    }


    public void DebugUpdateMap()
    {
        Debug.Log("Requesting Map update");
        terr.terrainData.SetHeights(0, 0, ConvertMap(netManager.RequestHeightMap()));
        Debug.Log("Updated Map");
    }

}
