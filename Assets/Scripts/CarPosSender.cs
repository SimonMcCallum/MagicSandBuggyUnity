using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPosSender : MonoBehaviour {

    public float updateInterval = 0.5f;

    TerrainUpdater tu;
    NetworkManager networkManager;
    float lastUpdate = 0;

	// Use this for initialization
	void Start () {
        networkManager = GameObject.FindObjectOfType<NetworkManager>();
        tu = GameObject.FindObjectOfType<TerrainUpdater>();
	}
	
	// Update is called once per frame
	void Update () {
		if (lastUpdate + updateInterval < Time.time)
        {
            int[] pos = tu.GetPlayerPositionOnTerrain();

            networkManager.sendCarPos(pos[0], pos[1]);

            lastUpdate = Time.time;
        }
	}
}
