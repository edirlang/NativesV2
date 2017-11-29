using UnityEngine;
using System.Collections;

public class SoldadosEspañoles : MonoBehaviour {

		public GameObject target, destruccionChoza, casa;
		public bool hablar;
		bool llego;
		public string mensaje;
		private Animator animator;
		float tiempo;
		Transform posicioninicial;
		// Use this for initialization
		void Start () {
				llego = false;
				animator = GetComponent<Animator> ();
				posicioninicial = target.transform;
				tiempo = -1;
		}

		// Update is called once per frame
		void Update () {

				if (General.paso_mision == 0) {
						DestruirChoza ();
				} else {
						casa.SetActive (true);
						if (GameObject.Find ("ChozaCompleta")) {
								Destroy (GameObject.Find ("ChozaCompleta"));
						}
				}
		}

		void OnGUI(){
				GUIStyle style = new GUIStyle ();
				style.alignment = TextAnchor.MiddleCenter;
				style = GUI.skin.GetStyle ("Box");
				style.fontSize = (int)(20.0f );
				if(llego && hablar)
				{
						GUI.Box(new Rect(0,3*Screen.height/4, Screen.width,Screen.height/4),mensaje);
				}
		}

		void DestruirChoza(){
				if (GameObject.Find ("ChozaLlamas")) {
						target = GameObject.Find ("ChozaLlamas");
				} else if (GameObject.Find ("PlayerJuego") && casa.active) {
						target = GameObject.Find ("PlayerJuego");

				}

				if (Vector3.Distance (target.transform.position, transform.position) > 5) {
						Quaternion rotacion = Quaternion.LookRotation (target.transform.position - transform.position);
						transform.rotation = Quaternion.Slerp (transform.rotation, rotacion, 6.0f * Time.deltaTime);
						transform.Translate (0, 0, 6.0f * Time.deltaTime);
						tiempo = 30;
						if (GameObject.Find ("Medieval_House")) {
								casa.SetActive (false);
						}
						animator.SetFloat ("speed", 1.0f);
				} else {
						animator.SetFloat ("speed", 0.0f);
						tiempo -= Time.deltaTime;
				}

				if ((tiempo > 0) && hablar) {

						if (tiempo > 28 && tiempo < 29 && (!GameObject.Find ("ChozaLlamas"))) {
								Destroy (GameObject.Find ("ChozaCompleta"));
								target = (GameObject)Instantiate (destruccionChoza, posicioninicial.position, transform.rotation);
								target.name = "ChozaLlamas";
								llego = false;
						}

						if (tiempo > 20 && tiempo < 29) {
								llego = true;
								mensaje = "Esta tierras son ahora del virey";
						}

						if (tiempo < 1 && tiempo > 0 && GameObject.Find ("ChozaLlamas")) {
								Destroy (GameObject.Find ("ChozaLlamas"));
								target = GameObject.Find ("PlayerJuego");
								hablar = false;
								llego = false;
								Misiones mision = Camera.main.gameObject.GetComponent<Misiones>();
								mision.procesoMision2(General.paso_mision);
						}
				}
		}
}
