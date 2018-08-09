using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
	public GameObject explosion;
	
	public void OnHit() {
		GameController gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		if(gc != null) gc.TargetOnHit(gameObject.tag);
		GameObject tmp = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
		Destroy(gameObject);
		Destroy(tmp, 1);
	}
}
