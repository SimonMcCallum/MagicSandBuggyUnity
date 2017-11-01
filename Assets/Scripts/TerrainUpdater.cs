using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class TerrainUpdater : MonoBehaviour {
    Terrain terr; // terrain to modify 
    NetworkManager netManager;
    Transform player;

    private float[,] targetHeightMap;
    private Thread activeThread = null;
    //in height map coordinates
    private int playerPosX;
    private int playerPosY;

    public bool nullHighest = false;
    public bool scaletoFullRange = false;
    public String playerObjectName = "Car";

    public void SetTargetHeightMap(float[,] map)
    {
        targetHeightMap = map;
    }

    void Start()
    {
        terr = Terrain.activeTerrain;
        netManager = GameObject.FindObjectOfType<NetworkManager>();
        player = GameObject.Find(playerObjectName).transform;
        
    }

    void Update()
    {
        playerPosX = GetPlayerPositionOnTerrain()[0];
        playerPosY = GetPlayerPositionOnTerrain()[1];
        if (activeThread == null)
        {
            activeThread = new Thread(updateMapDataAsync);
            activeThread.Start();
        }
        else if (activeThread.Join(0))
        {
            activeThread = null;
            if (targetHeightMap != null)
            {
                terr.terrainData.SetHeights(0, 0, targetHeightMap);
            }
        }
        
    }

    private void OnDisable()
    {
        if (activeThread != null)
        {
            activeThread.Abort();
        }
    }



    // does network request and calculation on the data
    public void updateMapDataAsync()
    {
        try
        {
            float[,] freshMap = ConvertMap(netManager.RequestHeightMap(),nullHighest,scaletoFullRange);
            if (targetHeightMap != null)
            {
                SmoothAndSparePlayerMap(playerPosX, playerPosY, targetHeightMap, freshMap);
            }
            SetTargetHeightMap(freshMap);
        } catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    private static float[,] ConvertMap(byte[][] mapData, bool nullHighest,bool scaletoFullRange)
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
                    map[y, x] /= diff;
                    map[y, x] *= 255;
                }
            }
        }


        return map;
    }

    private static void SmoothAndSparePlayerMap(int playerX, int playerY, float[,] current, float[,] target)
    {
        int playerSpareRadius = 30;
        int smoothRadus = 5;

        //if player on map dont change height around him
        if (0 < playerX && playerX < target.GetLength(1) && 0 < playerY && playerY < target.GetLength(0))
        {
            for (int y = 0; y < target.GetLength(0); y++)
            {
                for (int x = 0; x < target.GetLength(1); x++)
                {
                    if (Math.Abs(x - playerX) < playerSpareRadius && Math.Abs(y - playerY) < playerSpareRadius)
                    {
                        target[y, x] = current[y, x];
                    }
                }
            }
        }
        
        //smooth map
        for (int y = smoothRadus; y < target.GetLength(0)-smoothRadus; y++)
        {
            for (int x = smoothRadus; x < target.GetLength(1)-smoothRadus; x++)
            {
                float sum = 0;
                for (int xoff = -smoothRadus; xoff < smoothRadus; xoff++)
                {
                    for (int yoff = -smoothRadus; yoff < smoothRadus; yoff++)
                    {
                        sum += target[y+yoff, x+xoff];
                    }
                }
                target[y,x] = sum / Mathf.Pow(smoothRadus*2,2);
            }
        }
    }

    //returns [x,y]
    private int[] GetPlayerPositionOnTerrain()
    {
        //the following variables are relative to the heightmap coordinate system
        float worldPlayerY = player.position.z;
        float worldPlayerX = player.position.x;

        float mapPlayerY = (worldPlayerY / terr.terrainData.size.z) * terr.terrainData.heightmapResolution;
        float mapPlayerX = (worldPlayerX / terr.terrainData.size.x) * terr.terrainData.heightmapResolution;

        return new int[] { (int)mapPlayerX, (int)mapPlayerY};
    }

    public void DebugUpdateMap()
    {
        Debug.Log("Requesting Map update");
        terr.terrainData.SetHeights(0, 0, ConvertMap(netManager.RequestHeightMap(),false,false));
        Debug.Log("Updated Map");
    }

}
