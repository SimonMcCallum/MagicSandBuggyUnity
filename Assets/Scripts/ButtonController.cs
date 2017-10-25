using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetDummyData()
    {
        Terrain.activeTerrain.GetComponent<TerrainUpdater>().SetHeightMap(DummyData.GenerateFloatArr(0.05f));
    }
}
