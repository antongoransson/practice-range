using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public GameObject enemyTarget;
    public GameObject friendlyTarget;
    GameObject player;

    public Transform[] spawnPoints;
    public Transform endPoint;
    public bool[] spawned;
    public Vector3[] sizes;
    public Vector3 endPointSize;
    public int spawnDistance;
    public bool gameEnded;

    public Text endText;

    // Use this for initialization
    void Start () {
        gameEnded = false;
        player = GameObject.FindGameObjectWithTag ("Player");
        spawned = new bool[spawnPoints.Length];
        for (int i = 0; i < spawnPoints.Length; i++)
            spawned[i] = false;

    }

    // Update is called once per frame
    void Update () {
        if (Vector3.Distance (player.transform.position, endPoint.position) < 30 && !gameEnded) EndGame ();
        if (gameEnded) {
            if (Input.GetButton ("R")) {
                Time.timeScale = 1;
                Scene level = SceneManager.GetActiveScene ();
                SceneManager.LoadScene (level.name);

            }
        }
        SpawnTargets ();

    }
    private void SpawnTargets () {
        List<Vector3> spawnedLocations = new List<Vector3> ();
        for (int i = 0; i < spawnPoints.Length; i++) {
            if (Vector3.Distance (player.transform.position, spawnPoints[i].position) < spawnDistance && !spawned[i]) {
                spawned[i] = true;
                for (int j = 0; j < 6; j++) {
                    bool canSpawn = false;
                    while (!canSpawn) {
                        Vector3 size = sizes[i];
                        Vector3 pos = spawnPoints[i].position + new Vector3 (Random.Range (-size.x / 2, size.x / 2), 0, Random.Range (-size.z / 2, size.z / 2));
                        if (OkSpawnPoint (pos, spawnedLocations)) {
                            canSpawn = true;
                            if(j == 5)
                             Instantiate (friendlyTarget, pos, Quaternion.identity);
                             else
                              Instantiate (enemyTarget, pos, Quaternion.identity);
                            spawnedLocations.Add (pos);

                        }
                    }
                }
            }
        }
    }
    private bool OkSpawnPoint (Vector3 pos, List<Vector3> spawnedLocations) {
        bool tooClose = false;
        for (int i = 0; i < spawnedLocations.Count; i++) {
            if (Vector3.Distance (pos, spawnedLocations[i]) <= 1) {
                tooClose = true;
                break;

            }
        }
        return !tooClose;
    }
    public void EndGame () {
        Time.timeScale = 0;
        gameEnded = true;
        endText.text = "Game ended. Press R to restart";
    }
    void OnDrawGizmosSelected () {
        Gizmos.color = new Color (1, 0, 0, 0.5f);
        for (int i = 0; i < spawnPoints.Length; i++) {
            Vector3 size = sizes[i];
            Gizmos.DrawCube (spawnPoints[i].position, size);
        }
        Gizmos.DrawCube (endPoint.position, endPointSize);
    }
}