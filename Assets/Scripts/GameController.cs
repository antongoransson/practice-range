using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject capsule;

    public Vector3 center;
    public Vector3 size;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < 10; i++)
        {
            Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0, Random.Range(-size.z / 2, size.z / 2));
            GameObject.Instantiate(capsule, pos, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(center, size);
    }
}
