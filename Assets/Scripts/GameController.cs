using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public GameObject enemyTarget;
    public GameObject friendlyTarget;
    GameObject player;
    Weapon weapon;

    public Transform[] spawnPoints;
    public Transform endPoint;
    public bool[] spawned;
    public Vector3[] sizes;
    public Vector3 endPointSize;
    public int spawnDistance;
    public bool gameEnded;

    private int friendliesSpawned;
    private int enemiesSpawned;

    public Text endText;

    // Use this for initialization
    void Start () {
        gameEnded = false;
        player = GameObject.FindGameObjectWithTag ("Player");
        weapon = GameObject.FindGameObjectWithTag ("Weapon").GetComponent<Weapon> ();
        enemiesSpawned = 0;
        friendliesSpawned = 0;
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
                            if (j == 5) {
                                Instantiate (friendlyTarget, pos, Quaternion.identity);
                                friendliesSpawned += 1;
                            } else {
                                Instantiate (enemyTarget, pos, Quaternion.identity);
                                enemiesSpawned += 1;
                            }
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
        string accuracy = ((100 * weapon.enemiesKilled) / (weapon.bulletsFired)).ToString ();
        string enemyStats = weapon.enemiesKilled.ToString() + "/" + enemiesSpawned.ToString();
        string friendlyStats = weapon.friendliesKilled.ToString() + "/" + friendliesSpawned.ToString();
        endText.text = "Game ended. Press R to restart.\n Your accuracy was: " + accuracy + "%" + "\nEnemies killed: " + enemyStats +"\nFriendlies killed: " + friendlyStats;
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