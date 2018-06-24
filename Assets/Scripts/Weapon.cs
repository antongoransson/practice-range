using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public Texture2D crosshairTexture;
    public float range = 100f;
    public int bulletsPerMag = 30;
    public int bulletsLeft;
    public float crosshairScale = 1;

    public AudioClip shootSound;
    public int currentBullets;
    public ParticleSystem muzzleFlash;
    private AudioSource audioSource;

    
    public float fireRate = 200f;
    public Camera cam;
    float fireTimer;
	// Use this for initialization
	void Start () {
        if(cam == null) { Debug.Log("No camera for weapon"); }
        currentBullets = bulletsPerMag;
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButton("Fire1"))
        {
            Fire();
        }
       
        if (fireTimer < fireRate) fireTimer += Time.deltaTime;
	}

    void OnGUI()
    {
        if (Time.timeScale != 0)
        {
            if (crosshairTexture != null)
                GUI.DrawTexture(
                    new Rect(
                    (Screen.width - crosshairTexture.width * crosshairScale) / 2,
                    (Screen.height - crosshairTexture.height * crosshairScale) / 2,
                    crosshairTexture.width * crosshairScale, crosshairTexture.height * crosshairScale),
                    crosshairTexture);
            else
                Debug.Log("No crosshair texture set in the Inspector");
        }
    }

    private void Fire()
    {
        if (fireTimer < fireRate) return;
        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position,cam.transform.forward, out hit, range))
        {
        Debug.Log(hit.transform.name + "Hit ");

        }
            audioSource.clip = shootSound;
            audioSource.Play();
            muzzleFlash.Play();

        currentBullets -= 1;

        fireTimer = 0.0f;
    }
}
