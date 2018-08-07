using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {
    public Texture2D crosshairTexture;
    public float range = 100f;
    public int bulletsPerMag;
    public float crosshairScale = 1;

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

    // Use this for initialization
    void Start () {
        gameController = GameObject.FindGameObjectWithTag ("GameController");
        if (cam == null) { Debug.Log ("No camera for weapon"); }
        currentBullets = bulletsPerMag;
        bulletsFired = 0;
        reloading.gameObject.SetActive (false);
        audioSource = GetComponent<AudioSource> ();
        isReloading = false;
        setAmmoText ();
    }

    // Update is called once per frame
    void Update () {
        if (gameController.GetComponent<GameController> ().gameEnded || Time.timeScale == 0) return;
        if (isReloading && reloadTimer > reloadTime) {
            currentBullets = bulletsPerMag;
            setAmmoText ();
            reloading.gameObject.SetActive (false);
            isReloading = false;
        }
        if (!Input.GetKey (KeyCode.LeftShift) && Input.GetButton ("Fire1") && !isReloading) {
            Fire ();
        } else if (Input.GetButton ("R")) {
            Reload ();
        }

        if (fireTimer < fireRate) fireTimer += Time.deltaTime;
        if (reloadTimer < reloadTime) reloadTimer += Time.deltaTime;
    }

    void OnGUI () {
        if (Time.timeScale != 0) {
            if (crosshairTexture != null)
                GUI.DrawTexture (
                    new Rect (
                        (Screen.width - crosshairTexture.width * crosshairScale) / 2,
                        (Screen.height - crosshairTexture.height * crosshairScale) / 2,
                        crosshairTexture.width * crosshairScale, crosshairTexture.height * crosshairScale),
                    crosshairTexture);
            else
                Debug.Log ("No crosshair texture set in the Inspector");
        }
    }

    void Fire () {
        if (fireTimer < fireRate) return;
        RaycastHit hit;

        if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, range)) {
            if (hit.transform.tag == "EnemyTarget" || hit.transform.tag == "FriendlyTarget") {
                hit.transform.gameObject.GetComponent<Target> ().onHit ();
            }

        }
        audioSource.clip = shootSound;
        audioSource.Play ();
        muzzleFlash.Play ();

        currentBullets -= 1;
        bulletsFired += 1;
        if (currentBullets == 0)
            Reload ();
        setAmmoText ();

        fireTimer = 0.0f;
    }

    void Reload () {
        audioSource.clip = reloadSound;
        audioSource.Play ();
        isReloading = true;
        reloadTimer = 0.0f;
        reloading.gameObject.SetActive (true);

    }
    void setAmmoText () {
        currentAmmo.text = "Current Ammo: " + currentBullets.ToString () + "/" + bulletsPerMag.ToString ();
    }
}