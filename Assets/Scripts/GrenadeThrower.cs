using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour {
	public float throwForce = 40f;
	public GameObject grenadePrefab;
	public Camera cam;
	// Use this for initialization
	
	
	// Update is called once per frame
	void Update () {
		// Right mosue button
		if(Input.GetMouseButtonDown(1)) {
			ThrowGrenade();
		}
	}

	void ThrowGrenade() {
		GameObject grenade = Instantiate(grenadePrefab, cam.transform.position + new Vector3(0,1,0), cam.transform.rotation);
		Rigidbody rb = grenade.GetComponent<Rigidbody>();
		rb.AddForce(cam.transform.forward * throwForce, ForceMode.VelocityChange);
	}

}
