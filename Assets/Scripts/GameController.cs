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
    public bool hasStarted;

    int friendliesSpawned;
    int enemiesSpawned;
    int enemiesKilled;
    int friendliesKilled;

    public Text infoText;
    public Text enemies;
    public Text friendlies;

    public int targetsToSpawn = 6;

    // Use this for initialization
    void Start () {
        gameEnded = false;
        hasStarted = false;
        Time.timeScale = 0;
        player = GameObject.FindGameObjectWithTag ("Player");
        weapon = GameObject.FindGameObjectWithTag ("Weapon").GetComponent<Weapon> ();
        enemiesSpawned = 0;
        friendliesSpawned = 0;
        enemiesSpawned = 0;
        friendliesKilled = 0;
        infoText.text = "Use \"Left Mouse\" to shoot and \"Right Mouse\" to throw grenades\nWhen you are ready press SPACE to start";
        spawned = new bool[spawnPoints.Length];
        for (int i = 0; i < spawnPoints.Length; i++)
            spawned[i] = false;
        SetEnemiesKilledText ();
        SetFriendliesKilledText ();
    }

    // Update is called once per frame
    void Update () {
        if (Vector3.Distance (player.transform.position, endPoint.position) < 30 && !gameEnded) EndGame ();
        if (gameEnded && Input.GetButton ("R")) {
            Time.timeScale = 1;
            Scene level = SceneManager.GetActiveScene ();
            SceneManager.LoadScene (level.name);
        } else if (!hasStarted && Input.GetKeyDown (KeyCode.Space)) {
            Time.timeScale = 1;
            hasStarted = true;
            infoText.text = "";
        }
        SpawnTargets ();
    }

    private void SpawnTargets () {
        Vector3[] spawnedLocations = new Vector3[targetsToSpawn];
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
                            spawnedLocations[j] = pos;
                        }
                    }
                }
            }
        }
    }
    private bool OkSpawnPoint (Vector3 pos, Vector3[] spawnedLocations) {
        bool tooClose = false;
        foreach (Vector3 location in spawnedLocations) {
            if (Vector3.Distance (pos, location) <= 2) {
                tooClose = true;
                break;
            }
        }
        return !tooClose;
    }

    public void EndGame () {
        Time.timeScale = 0;
        gameEnded = true;
        string enemyStats = enemiesKilled.ToString () + "/" + enemiesSpawned.ToString ();
        string friendlyStats = friendliesKilled.ToString () + "/" + friendliesSpawned.ToString ();
        infoText.text = "Game ended. Press R to restart.\n" + GetAccuracy () + "\nEnemies killed: " + enemyStats + "\nFriendlies killed: " + friendlyStats;
    }

    string GetAccuracy () {
        int bulletsFired = weapon.bulletsFired;
        if (bulletsFired == 0)
            return "No shots were fired";
        string accuracy = ((100 * enemiesKilled) / (weapon.bulletsFired)).ToString ();
        return "Your accuracy was: " + accuracy + "%";
    }

    public void TargetOnHit (string tag) {
        if (tag == "EnemyTarget") {
            enemiesKilled += 1;
            SetEnemiesKilledText ();
        } else if (tag == "FriendlyTarget") {
            friendliesKilled += 1;
            SetFriendliesKilledText ();
        }
    }

    void SetEnemiesKilledText () {
        enemies.text = "Enemies killed: " + enemiesKilled.ToString ();
    }
    void SetFriendliesKilledText () {
        friendlies.text = "Friendlies killed: " + friendliesKilled.ToString ();
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