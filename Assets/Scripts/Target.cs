using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
	public GameObject explosion;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onHit() {
		GameController gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		if(gc != null) gc.TargetOnHit(gameObject.tag);
		GameObject tmp = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
		Destroy(gameObject);
		Destroy(tmp, 1);
	}
}
