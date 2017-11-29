using UnityEngine;
using System.Collections;

public class FinJuego : MonoBehaviour {

		public GameObject luz, creditos, lluvia, pj1, pj2, pj3;
		public AnimationClip animacion;
		float tiempo, tiempoCreditos;
		bool iniciarCreditos;

	// Use this for initialization
	void Start () {
				tiempo = 6;
				iniciarCreditos = false;
				tiempoCreditos = animacion.length;

				GameObject jugador = pj1;
				switch (General.idPersonaje) {
				case 1:
						jugador = pj1;
						break;
				case 2:
						jugador = pj2;
						break;
				case 3:
						jugador = pj3;
						break;
				}

				Camera.main.transform.parent = GameObject.Find ("IniciarVariables").transform;
				Network.Destroy (GameObject.Find (Network.player.ipAddress));
				GameObject g = (GameObject) Network.Instantiate (jugador, GameObject.Find("PlayerJuego").transform.position, new Quaternion(), 1);
				g.transform.localScale = new Vector3 (2, 2, 2);
				g.AddComponent<BoxCollider> ();
				g.GetComponent<BoxCollider> ().size = new Vector3 (0.1f, 0.1f, 0.1f);

				g.name = Network.player.ipAddress;

				GameObject.Find ("MusicaFondo").GetComponent<AudioSource> ().volume = 1;
				GameObject rain = (GameObject) Instantiate (lluvia, transform.position, transform.rotation);
				rain.transform.parent = GameObject.Find (Network.player.ipAddress).transform;
				rain.transform.position = Vector3.zero;
				creditos.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
				
				if(GameObject.Find("Chia(Clone)")){
						Destroy (GameObject.Find ("Chia(Clone)"));
				}
				tiempo -= Time.deltaTime;
				if (tiempo < 0) {
						luz.SetActive(false);
						iniciarCreditos = true;
						MoverMouse.cambioCamara = true;
						Camera.main.transform.parent = transform;
				}

				if (iniciarCreditos) {
						tiempoCreditos -= Time.deltaTime;
						creditos.SetActive (true);
				}

				if (tiempoCreditos < 0) {
						Destroy (GameObject.Find("Luz"));
						MoverMouse.cambioCamara = false;
						StartCoroutine (Camera.main.GetComponent<Conexion>().desconectarUser ());

				}
	}

		void OnGUI(){

				GUIStyle style = new GUIStyle ();
				style.alignment = TextAnchor.MiddleCenter;
				style = GUI.skin.GetStyle ("Box");
				style.fontSize = (int)(20.0f);
				GUI.Box (new Rect (Screen.width/2, 9 * Screen.height / 10, Screen.width/2, Screen.height / 10), "Fusagasuga, 2016");
		}
}
