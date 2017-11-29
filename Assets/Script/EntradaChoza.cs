using UnityEngine;
using System.Collections;

public class EntradaChoza : MonoBehaviour {

		public GameObject ubicacionCamara;
		GameObject player;
		bool cambio;
	// Use this for initialization
	void Start () {
				gameObject.GetComponent<MeshRenderer> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
				if (cambio) {
						Camera.main.transform.parent = gameObject.transform;
						Camera.main.transform.position = ubicacionCamara.transform.position;
						Camera.main.transform.rotation = ubicacionCamara.transform.rotation;
				}
	}

		public void OnTriggerEnter (Collider colision)
		{
				if (colision.name == Network.player.ipAddress) {
						player = colision.gameObject;
						if (MoverMouse.cambioCamara) {
								MoverMouse.cambioCamara = false;
								cambio = false;
						} else {
								MoverMouse.cambioCamara = true;
								player.transform.Translate (2*Vector3.forward);
								cambio = true;
						}
								
				}
		}
}
