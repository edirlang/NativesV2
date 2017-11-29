using UnityEngine;
using System.Collections;

public class TeleTrasportador : MonoBehaviour {

		public GameObject maquinaTiempo, trasportar;
		public float tiempo;
		bool viaje;
		GameObject player;
	// Use this for initialization
	void Start () {
				tiempo = 0;
				viaje = false;
	}
	
	// Update is called once per frame
	void Update () {
				if (tiempo > 0) {
						tiempo -= Time.deltaTime;
				}
				if (viaje) {
						player.transform.position = trasportar.transform.position;
						MoverMouse.cambioCamara = false;

						if (tiempo < 1 && tiempo > 0) {
								
								viaje = false;
								Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
								mision.procesoMision3 (General.paso_mision);
						}
				}
	}

		public void OnTriggerEnter (Collider colision)
		{
				if (colision.name == Network.player.ipAddress) {
						player = colision.gameObject;
						MoverMouse.cambioCamara = true;
						viaje = true;
						tiempo = 10;
				}
		}
}
