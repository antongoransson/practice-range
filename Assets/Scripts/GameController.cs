using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public GameObject enemyTarget;
    public GameObject friendlyTarget;
    GameObject player;

    public Transform[] spawnPoints;
    public bool[] spawned;
    public Vector3[] sizes;
    public int spawnDistance;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player");
        spawned = new bool[spawnPoints.Length];
        for (int i = 0; i < spawnPoints.Length; i++)
            spawned[i] = false;

    }

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < spawnPoints.Length; i++) {

            if (Vector3.Distance (player.transform.position, spawnPoints[i].position) < spawnDistance && !spawned[i]) {

                spawned[i] = true;
                for (int j = 0; j < 5; j++)

                {
                    Vector3 size = sizes[i];
                    Vector3 pos = spawnPoints[i].position + new Vector3 (Random.Range (-size.x / 2, size.x / 2), 0, Random.Range (-size.z / 2, size.z / 2));
                    Instantiate (enemyTarget, pos, Quaternion.identity);
                }
                Vector3 size1 = sizes[i];
                Vector3 pos1 = spawnPoints[i].position + new Vector3 (Random.Range (-size1.x / 2, size1.x / 2), 0, Random.Range (-size1.z / 2, size1.z / 2));
                Instantiate (friendlyTarget, pos1, Quaternion.identity);
            }
        }
    }
    void OnDrawGizmosSelected () {
        Gizmos.color = new Color (1, 0, 0, 0.5f);
        for (int i = 0; i < spawnPoints.Length; i++) {
            Vector3 size = sizes[i];
            Gizmos.DrawCube (spawnPoints[i].position, size);
        }
    }
}