﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {
	public float delay = 2f;
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
		if (countdown <= 0f && !hasExploded) {
			Explode ();
		}

	}

	void Explode () {
		hasExploded = true;
		GameObject explosion = Instantiate (explosionEffect, transform.position, transform.rotation);
		Collider[] targetsHit = Physics.OverlapSphere (transform.position, blastRadius);

		foreach (Collider obj in targetsHit) {
			RaycastHit hit;
			if (Physics.Linecast (transform.position, obj.transform.position, out hit)) {
				// Don't destroy target if a wall is in the way
				if (hit.transform == obj.transform || hit.transform.gameObject.tag == "FriendlyTarget"  || hit.transform.gameObject.tag == "EnemyTarget") {
					Target t = obj.GetComponent<Target> ();
					if (t != null) {
						t.OnHit ();
					}
				}
			}
		}
		Destroy (explosion, 1);
		Destroy (gameObject);
	}
}