using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Timer : MonoBehaviour {

	// Use this for initialization
	public float t;
	 public Text time;
	void Start () {
		t = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;
		time.text = this.ToString();
		
	}
	public override string ToString(){
		return "Time: " + System.Math.Round(t, 2) ; 
	}  
}

