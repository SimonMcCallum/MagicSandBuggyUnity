using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPosSender : MonoBehaviour {

    public float updateInterval = 0.5f;


    NetworkManager networkManager;
    float lastUpdate = 0;

	// Use this for initialization
	void Start () {
        networkManager = GameObject.FindObjectOfType<NetworkManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (lastUpdate + updateInterval < Time.time)
        {
            Vector3 pos = gameObject.transform.position;

            networkManager.sendCarPos((int)pos.x, (int)pos.y);

            lastUpdate = Time.time;
        }
	}
}
