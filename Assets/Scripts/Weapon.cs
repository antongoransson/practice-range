using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {
    public float range = 100f;
    public int bulletsPerMag;

    public AudioClip shootSound;
    public AudioClip reloadSound;
    public int currentBullets;
    public ParticleSystem muzzleFlash;
    public GameObject gameController;
    private AudioSource audioSource;

    public Text currentAmmo;
    public Text reloading;

    public float fireRate = 200f;
    public float reloadTime;
    public Camera cam;

    float fireTimer;
    float reloadTimer;
    bool isReloading;
    public int bulletsFired;
    public int bulletsHit;

    // Use this for initialization
    void Start () {
        gameController = GameObject.FindGameObjectWithTag ("GameController");
        if (cam == null) { Debug.Log ("No camera for weapon"); }
        currentBullets = bulletsPerMag;
        bulletsFired = 0;
        reloading.gameObject.SetActive (false);
        audioSource = GetComponent<AudioSource> ();
        isReloading = false;
        SetAmmoText ();
    }

    // Update is called once per frame
    void Update () {
        if (gameController.GetComponent<GameController> ().gameEnded || Time.timeScale == 0) return;
        if (isReloading && reloadTimer > reloadTime) {
            currentBullets = bulletsPerMag;
            SetAmmoText ();
            reloading.gameObject.SetActive (false);
            isReloading = false;
        }
        if (!Input.GetKey (KeyCode.LeftShift) && Input.GetButton ("Fire1") && !isReloading) {
            Fire ();
        } else if (Input.GetButton ("R") && !isReloading) {
            Reload ();
        }

        if (fireTimer < fireRate) fireTimer += Time.deltaTime;
        if (reloadTimer < reloadTime) reloadTimer += Time.deltaTime;
    }



    void Fire () {
        if (fireTimer < fireRate) return;
        RaycastHit hit;

        if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, range)) {
            if (hit.transform.tag == "EnemyTarget" || hit.transform.tag == "FriendlyTarget") {
                hit.transform.gameObject.GetComponent<Target> ().OnHit ();
                
            }
             if (hit.transform.tag == "EnemyTarget") bulletsHit += 1;

        }
        audioSource.clip = shootSound;
        audioSource.Play ();
        muzzleFlash.Play ();

        currentBullets -= 1;
        bulletsFired += 1;
        if (currentBullets == 0)
            Reload ();
        SetAmmoText ();

        fireTimer = 0.0f;
    }

    void Reload () {
        audioSource.clip = reloadSound;
        audioSource.Play ();
        isReloading = true;
        reloadTimer = 0.0f;
        reloading.gameObject.SetActive (true);

    }
    void SetAmmoText () {
        currentAmmo.text = "Current Ammo: " + currentBullets.ToString () + "/" + bulletsPerMag.ToString ();
    }
}