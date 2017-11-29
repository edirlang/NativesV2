using UnityEngine;
using System.Collections;

public class Gonzalo : MonoBehaviour {
	public GameObject zona;
	public bool buscar = false;
	public float tiempo=0;
	// Use this for initialization
	void Start () {
		zona = GameObject.Find ("zonaLLegadaAltagracia");
	}
	
	// Update is called once per frame
	void Update () {

	}
}
