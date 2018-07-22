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
    private AudioSource audioSource;

    public Text enemies;
    public Text friendlies;
    public Text currentAmmo;
    public Text reloading;

    public int enemiesKilled;    
    public int friendliesKilled;    
    public float fireRate = 200f;
    public float reloadTime;
    public Camera cam;

    float fireTimer;
    float reloadTimer;
    bool isReloading;

	// Use this for initialization
	void Start () {
        if(cam == null) { Debug.Log("No camera for weapon"); }
        currentBullets = bulletsPerMag;
        audioSource = GetComponent<AudioSource>();
        enemiesKilled = 0;
        isReloading = false;
        friendliesKilled = 0;
        setEnemiesKilledText();
        setFriendliesKilledText();
        setAmmoText();
	}
	
	// Update is called once per frame
	void Update () {
        if (isReloading && reloadTimer > reloadTime) {
            currentBullets = bulletsPerMag;
            setAmmoText();
            reloading.text="";
            isReloading = false;
        }
        if(Input.GetButton("Fire1") && !isReloading)
        {
            Fire();
        }
         else if (Input.GetButton("R"))
        {
            reload();
        }
       
        if (fireTimer < fireRate) fireTimer += Time.deltaTime;
        if (reloadTimer < reloadTime) reloadTimer += Time.deltaTime;
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

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
        Debug.Log(hit.transform.name + "Hit ");
            if(hit.transform.tag == "EnemyTarget" )
            {
             Destroy(hit.transform.gameObject);
             enemiesKilled +=1;
             setEnemiesKilledText();
            } else if(hit.transform.tag == "FriendlyTarget")
            {
                Destroy(hit.transform.gameObject);
                friendliesKilled +=1;
                setFriendliesKilledText();
            }

        }
            audioSource.clip = shootSound;
            audioSource.Play();
            muzzleFlash.Play();

        currentBullets -= 1;
        if(currentBullets == 0)
            reload();
        setAmmoText();

        fireTimer = 0.0f;
    }
    private void reload()   
    {    
        audioSource.clip = reloadSound;
        audioSource.Play();
        isReloading = true;
        reloadTimer = 0.0f;
        reloading.text = "Reloading...";

    }
    private void setEnemiesKilledText(){
        enemies.text = "Enemies killed: " + enemiesKilled.ToString ();
        
    }
    private void setFriendliesKilledText(){
        friendlies.text = "Friendlies killed: " + friendliesKilled.ToString ();
        
    }
    private void setAmmoText(){
        currentAmmo.text = "Current Ammo: " + currentBullets.ToString ()+ "/" +  bulletsPerMag.ToString();
        
    }
}
