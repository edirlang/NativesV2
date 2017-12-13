using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class animate : MonoBehaviour {
	public Sprite[] frames;
	public double framesPorSegundo = 10.0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int index = (int) (Time.time * framesPorSegundo);
		index = index % frames.Length;
		this.GetComponent<Image>().sprite = frames[index];
	}
}
