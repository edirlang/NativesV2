using UnityEngine;
using System.Collections;


public class Trasportador : MonoBehaviour
{
		public GameObject llegada, efecto;
		GameObject player;
		Animator animator;
		float tiempo = 0;
		bool inciarTiempo;
		// Use this for initialization
		void Start ()
		{
				inciarTiempo = false;

		}
	
		// Update is called once per frame
		void Update ()
		{
				tiempo -= Time.deltaTime;
				if (tiempo < 0 && inciarTiempo) {
						player.transform.position = llegada.transform.position;

						if (General.misionActual [0] == "1" && General.paso_mision == 2) {
								Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
								mision.procesoMision1 (General.paso_mision);
						} else if (General.misionActual [0] == "1" && General.paso_mision == 4 && llegada.name == "PlayerJuego") {
								General.timepo = 20;
								General.timepoChia = 20.5f;
								Misiones.instanciar = true;
						} else if (General.misionActual [0] == "1" && General.paso_mision == 4 && llegada.name == "pasca") {
								Misiones mision = Camera.main.gameObject.GetComponent<Misiones> ();
								mision.procesoMision1 (General.paso_mision);
						} else if (General.misionActual [0] == "1" && General.paso_mision == 6 && llegada.name == "PlayerJuego") {
								General.timepo = 10;
								General.timepoChia = 10.5f;
								Misiones.instanciar = true;
						}

						inciarTiempo = false;
						MoverMouse.movimiento = true;
						MoverMouse.cambioCamara = false;

						efecto.SetActive(false);
				}

				if (tiempo > 0 && tiempo < 1) {
						animator.SetBool ("transportador", false);
				}
		}

		public void OnTriggerEnter (Collider colision)
		{
				if (colision.gameObject.name == Network.player.ipAddress) {
						player = colision.gameObject;
						animator = player.GetComponent<Animator> ();
						efecto.SetActive(true);
						MoverMouse.movimiento = false;
						MoverMouse.cambioCamara = true;
						tiempo = 3;
						inciarTiempo = true;
						animator.SetBool("transportador",true);
				}
		}
}
