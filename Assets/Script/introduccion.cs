using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class introduccion : MonoBehaviour {

		public GameObject lluviaPrefab, luz, rayos, pj1,pj2,pj3;
		GameObject jugador, lluvia;
		float tiempo = 20f;
		bool crearLlubia;
	// Use this for initialization
	void Start () {
				if (General.username == "") {
						SceneManager.LoadScene ("main");
				}
				crearLlubia = true;
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

	}
	
	// Update is called once per frame
	void Update () {
				
				if (GameObject.Find (Network.player.ipAddress) && crearLlubia) {
						lluvia = (GameObject)Instantiate (lluviaPrefab, transform.position, transform.rotation);
						lluvia.transform.parent = Camera.main.transform;
						lluvia.transform.localPosition = Vector3.zero;
						crearLlubia = false;
				}

				tiempo -= Time.deltaTime;

				if (tiempo < 0) {
						luz.GetComponent<Light> ().color = Color.white;
						luz.GetComponent<Light> ().intensity = 8;

						Camera.main.transform.parent = GameObject.Find ("IniciarVariables").transform;

						Destroy (lluvia);
						Misiones mision = Camera.main.gameObject.GetComponent<Misiones>();
						mision.terminoMision = true;
						Network.Destroy (GameObject.Find (Network.player.ipAddress));
						GameObject g = (GameObject)Network.Instantiate (General.personaje, GameObject.Find("PlayerJuego").transform.position, new Quaternion(), 1);
						g.transform.localScale = new Vector3 (2, 2, 2);
						g.AddComponent<BoxCollider> ();
						g.GetComponent<BoxCollider> ().size = new Vector3 (0.1f, 0.1f, 0.1f);
						g.name = Network.player.ipAddress;

						SceneManager.LoadScene ("level1");
				}
	}

		void OnGUI(){


				GUIStyle style = new GUIStyle ();
				style.alignment = TextAnchor.MiddleCenter;
				style = GUI.skin.GetStyle ("Box");
				style.fontSize = (int)(20.0f);
				GUI.Box (new Rect (Screen.width/2, 9 * Screen.height / 10, Screen.width/2, Screen.height / 10), "Fusagasugá, 2016");

				GUI.color = Color.black;
				if (tiempo > 3 && tiempo < 5) {
						luz.GetComponent<Light> ().color = Color.black;
						luz.GetComponent<Light> ().intensity = 8;
						GUI.Label (new Rect (Screen.width/2 - Screen.width / 6, Screen.height / 2 - Screen.height / 12, Screen.width / 3, Screen.height / 10), "Algo anda mal");
				} else if(tiempo > 1 && tiempo < 3){
						Destroy (rayos);
						GUI.Label (new Rect (Screen.width/2 - Screen.width / 6, Screen.height / 2 - Screen.height / 12, Screen.width / 3, Screen.height / 10), "¿Que sucede?");
				}else if(tiempo < 1){
						GUI.Label (new Rect (Screen.width/2 - Screen.width / 6, Screen.height / 2 - Screen.height / 12, Screen.width / 3, Screen.height / 10), "nooooooooooooooooo");
				}

				GUI.color = Color.white;
		}
}
