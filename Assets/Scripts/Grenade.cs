using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {
	public float delay = 3f;
	public float blastRadius;
	public GameObject explosionEffect;
	float countdown;
	bool hasExploded = false;


	// Use this for initialization
	void Start () {
		countdown = delay;	
	}
	
	// Update is called once per frame
	void Update () {
		countdown -= Time.deltaTime;
		if(countdown <= 0f && !hasExploded) {
			Explode();
		}

	}

	void Explode() {
		hasExploded = true;
		Instantiate(explosionEffect, transform.position, transform.rotation);

		Collider [] targetsHit = Physics.OverlapSphere(transform.position, blastRadius);

		foreach (Collider obj in targetsHit){
			Target t = obj.GetComponent<Target>(); 
			if(t != null) {
				t.onHit();
			}
		}

		Destroy(gameObject);
	}
}
