using UnityEngine;
using System.Collections;

public class VerificacionVirrey : MonoBehaviour {

	// Use this for initialization
	void Start () {
				gameObject.GetComponent<MeshRenderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

		public void OnTriggerEnter (Collider colision)
		{
				if (colision.name == Network.player.ipAddress) {
						if (General.paso_mision != 5) {
								colision.gameObject.transform.position = GameObject.Find ("pasca").transform.position;
						}
				}

		}
}
