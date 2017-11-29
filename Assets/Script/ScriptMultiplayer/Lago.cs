using UnityEngine;
using System.Collections;

public class Lago : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.Translate (0 , Random.Range(-5,5), Random.Range(-5,5));
	}

	// Update is called once per frame
	void Update () {
			
	}
}
