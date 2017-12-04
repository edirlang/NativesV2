using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour
{

		public GameObject objetoInstanciar, pj1, pj2, pj3, pj12, pj22, pj13, pj23, pj33, pj32, pjR12, pjR22, pjR32;
		public GameObject Ubicacioncamara, obj_mision;
		public Text label_username, label_mision;
		public Texture monedas, vidas;
		private string[] misiones, mision;
		private int opciones = 0;
		string porcentaje;
		WWW www, www1, www2;
		// Use this for initialization
		void Start ()
		{

				string url2 = General.hosting + "usuario";
				WWWForm form2 = new WWWForm ();
				form2.AddField ("username", General.username);
				www = new WWW (url2, form2);
				StartCoroutine (consultarUsuarioPorUsername (www));

				string url = General.hosting + "misiones";
				www1 = new WWW (url);
				StartCoroutine (consultarMisiones (www1));

				url = General.hosting + "mision";
				WWWForm form = new WWWForm ();
				form.AddField ("username", General.username);
				www2 = new WWW (url, form);
				StartCoroutine (consultarMisionActual (www2));

				MoverMouse.cambioCamara = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				float porcentajeConsultas = (www.progress + www1.progress + www2.progress) / 3;
				porcentaje = porcentajeConsultas.ToString ("00%");
				if (opciones != 1 && GameObject.FindGameObjectWithTag ("Player")) {
						GameObject jugador = GameObject.FindGameObjectWithTag ("Player");
						jugador.transform.Rotate (Vector3.up, Time.deltaTime * 30, Space.World);
				}

				if (General.misionActual [0] == "2") {
						switch (General.idPersonaje) {
						case 1: 
								General.personaje = pjR12;
								break;
						case 2:
								General.personaje = pjR22;
								break;
						case 3:
								General.personaje = pjR32;
								break;
						}
				}

		}

		void OnGUI ()
		{

				GUIStyle style = new GUIStyle ();

				style = GUI.skin.GetStyle ("label");
				style.fontSize = (int)(25.0f);
		
				style = GUI.skin.GetStyle ("button");
				style.fontSize = (int)(20.0f);

				var centeredStyle = GUI.skin.GetStyle ("Label");
				centeredStyle.alignment = TextAnchor.UpperCenter;

				switch (opciones) {
				case 0:
						pantallaNormal (style);
						break;
				case 1:
						GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "");
						GUI.Label (new Rect (Screen.width - Screen.width / 4, Screen.height - Screen.height / 6, Screen.width / 4, Screen.height / 6), "Cargando...");
						break;
				case 2:
						pantallaMisiones ();
						break;
				case 3:
						pantallaMision ();
						break;
				case 4:
						pantallaConfiguracion ();
						break;
				}
		}

		private void pantallaNormal (GUIStyle style)
		{
				GUI.Box (new Rect (Screen.width / 12, Screen.height / 24, 5 * (Screen.width / 12), 23 * (Screen.height / 24)), "");
	
				label_username.text = General.username;
				label_mision.text = General.misionActual [1];
				
				//GUI.Label (new Rect (2 * (Screen.width / 6) - Screen.width / 8, (Screen.height / 10), Screen.width / 6, Screen.height / 12), General.username);
				if (porcentaje != "100%")
						GUI.Label (new Rect (Screen.width - Screen.width / 8, 9 * (Screen.height / 10), Screen.width / 8, Screen.height / 10), porcentaje);

				style = GUI.skin.GetStyle ("label");
				style.fontSize = (int)(20.0f);
		
				//GUI.Box (new Rect (2 * (Screen.width / 6) - Screen.width / 8, 7 * (Screen.height / 40), Screen.width / 6, Screen.height / 48), "");
		
				//GUI.Label (new Rect (2 * (Screen.width / 6) - Screen.width / 8, 3 * (Screen.height / 10), Screen.width / 6, Screen.height / 12), "Misión Actual");
		
				//GUI.Label (new Rect (3 * (Screen.width / 12) - Screen.width / 8, 4 * (Screen.height / 10), Screen.width / 3, Screen.height / 3), General.misionActual [1]);
		
				//if (GUI.Button (new Rect (3 * (Screen.width / 12) - Screen.width / 8, 7 * (Screen.height / 10), Screen.width / 7, Screen.height / 12), "Misiones")) {
				//		opciones = 2;
				//}

				if (GUI.Button (new Rect (5 * (Screen.width / 12) - Screen.width / 8, 7 * (Screen.height / 10), Screen.width / 7, Screen.height / 12), "Configuracion")) {
						opciones = 4;
				}

		
		
				GUI.Box (new Rect (6 * (Screen.width / 10) - Screen.width / 16, (Screen.height / 10), Screen.width / 12, Screen.height / 12), vidas, style);
				GUI.Label (new Rect (6 * (Screen.width / 10), (Screen.height / 10), Screen.width / 10, Screen.height / 12), "x " + General.salud + "");
		
				GUI.Box (new Rect (9 * (Screen.width / 10) - Screen.width / 16, (Screen.height / 10), Screen.width / 12, Screen.height / 12), monedas, style);
				GUI.Label (new Rect (9 * (Screen.width / 10), (Screen.height / 10), Screen.width / 10, Screen.height / 12), "x " + General.monedas + "");
				if (General.misionActual [0] != "4") {
						if (porcentaje == "100%" && GUI.Button (new Rect (4 * (Screen.width / 6), 9 * (Screen.height / 10), Screen.width / 6, Screen.height / 10), "Jugar")) {
								opciones = 1;
								SceneManager.LoadScene ("lobyScena");
						}
				} else {
						if(GUI.Button (new Rect (4 * (Screen.width / 6), 9 * (Screen.height / 10), Screen.width / 6, Screen.height / 10), "Reiniciar")){
								General.paso_mision = 1;
								General.misionActual [0] = "5";
								General.salud = 3;
								General.monedas = 10;
								StartCoroutine (General.cambiarMision ());
						}
				}
		}

		public void logout(){
			opciones = 1;
			General.conectado = false;
			General.username = null;
			General.idPersonaje = 0;
			General.personaje = null;
			Destroy (GameObject.Find("IniciarVariables"));
			Debug.Log ("eliminado");
			Application.LoadLevel ("main");
		}
		
	public void play(){
		opciones = 1;
		SceneManager.LoadScene ("lobyScena");
	}

	public void pantallaMisiones ()
	{
		/*
		 * for (int i = 0; i < misiones.Length - 1; i++) {
			string[] mision_array = misiones [i].Split ('-');
			GameObject obj_mision_1 = obj_mision;

			obj_mision_1.GetComponent<Transform>().FindChild("id").gameObject.GetComponent<Text>().text = mision_array [0];
			obj_mision_1.GetComponent<Transform>().FindChild("nombre").gameObject.GetComponent<Text>().text = mision_array [0];
			obj_mision_1.GetComponent<Transform>().FindChild("button").gameObject.GetComponent<Button>().onClick = this.pantallaMision();

			obj_mision_1.transform.localPosition.x += 20;

		}
		*/
		for (int i = 0; i < misiones.Length - 1; i++) {
			string[] mision_array = misiones [i].Split ('-');

					GUI.Label (new Rect (3 * (Screen.width / 20), (i + 3) * (Screen.height / 10), Screen.width / 14, Screen.height / 12), mision_array [0]);
					GUI.Label (new Rect (4 * (Screen.width / 20), (i + 3) * (Screen.height / 10), Screen.width / 2, Screen.height / 12), mision_array [1]);
					if (GUI.Button (new Rect (8 * (Screen.width / 10), (i + 3) * (Screen.height / 10), Screen.width / 10, Screen.height / 12), "Detalles")) {
							opciones = 3;
							mision = mision_array;
					}
			}

			if (GUI.Button (new Rect (7 * (Screen.width / 10), Screen.height - Screen.height / 5, Screen.width / 6, Screen.height / 10), "Volver")) {
					opciones = 0;
			}
	}

		private void pantallaMision ()
		{
				GUIStyle style = new GUIStyle ();
				style = GUI.skin.GetStyle ("box");
				style.fontSize = (int)(25.0f);
				style = GUI.skin.GetStyle ("label");
				style.fontSize = (int)(25.0f);
				style.alignment = TextAnchor.UpperLeft;
				GUI.Box (new Rect (Screen.width / 10, Screen.height / 10, Screen.width - Screen.width / 7, Screen.height - Screen.height / 6), "Misión " + mision [0]);

				style.fontStyle = FontStyle.Bold;
				GUI.Label (new Rect (3 * (Screen.width / 20), 4 * (Screen.height / 20), Screen.width / 2, Screen.height / 12), "Nombre:");
				style.fontStyle = FontStyle.Normal;
				GUI.Label (new Rect (7 * (Screen.width / 20), 4 * (Screen.height / 20), 7 * (Screen.width / 12), Screen.height / 6), mision [1]);

				style.fontStyle = FontStyle.Bold;
				GUI.Label (new Rect (3 * (Screen.width / 20), 7 * (Screen.height / 20), Screen.width / 6, Screen.height / 12), "Requisitos:");
				style.fontStyle = FontStyle.Normal;
				GUI.Label (new Rect (7 * (Screen.width / 20), 7 * (Screen.height / 20), 7 * (Screen.width / 12), Screen.height / 3), mision [2]);


				if (GUI.Button (new Rect (7 * (Screen.width / 10), Screen.height - Screen.height / 5, Screen.width / 6, Screen.height / 10), "Volver")) {
						opciones = 2;
				}
		}

		public IEnumerator consultarUsuarioPorUsername (WWW www)
		{
				yield return www;
				if (www.error == null) {
						string[] usuario = www.text.Split ('-');
						General.salud = int.Parse (usuario [5]);
						General.monedas = int.Parse (usuario [6]);
				} else {
						Debug.Log (www.error);
				}
		}

		public IEnumerator consultarMisiones (WWW www)
		{
				yield return www;
				if (www.error == null) {
						misiones = www.text.Split ('/');
				} else {
						Debug.Log (www.error);
				}
		}

		public IEnumerator consultarMisionActual (WWW www)
		{
				yield return www;
				if (www.error == null) {
						string[] mision = www.text.Split ('*');

						General.misionActual [0] = mision [0];
						General.misionActual [1] = mision [1];
						General.misionActual [2] = mision [2];
						GameObject personaje;
			
						if (General.idPersonaje == 1) {
								if (General.misionActual [0] == "1") {
										personaje = Instantiate (pj1, objetoInstanciar.transform.position, objetoInstanciar.transform.rotation) as GameObject;
								} else if (General.misionActual [0] == "2") {
										personaje = Instantiate (pj12, objetoInstanciar.transform.position, objetoInstanciar.transform.rotation) as GameObject;
								} else {
										personaje = Instantiate (pj13, objetoInstanciar.transform.position, objetoInstanciar.transform.rotation) as GameObject;
								}
								personaje.tag = "Player";
						} else if (General.idPersonaje == 2) {
								if (General.misionActual [0] == "1") {
										personaje = Instantiate (pj2, objetoInstanciar.transform.position, objetoInstanciar.transform.rotation) as GameObject;
								} else if (General.misionActual [0] == "2") {
										personaje = Instantiate (pj22, objetoInstanciar.transform.position, objetoInstanciar.transform.rotation) as GameObject;
								}else {
										personaje = Instantiate (pj23, objetoInstanciar.transform.position, objetoInstanciar.transform.rotation) as GameObject;
								}
								personaje.tag = "Player";
						} else if (General.idPersonaje == 3) {
								if (General.misionActual [0] == "1") {
										personaje = Instantiate (pj3, objetoInstanciar.transform.position, objetoInstanciar.transform.rotation) as GameObject; 
								} else if (General.misionActual [0] == "2") {
										personaje = Instantiate (pj32, objetoInstanciar.transform.position, objetoInstanciar.transform.rotation) as GameObject;
								}else {
										personaje = Instantiate (pj33, objetoInstanciar.transform.position, objetoInstanciar.transform.rotation) as GameObject;
								}
								personaje.tag = "Player";
						}

						if (mision [10] == "0") {
								General.bono = false;
						} else {
								General.bono = true;
						}

						General.paso_mision = int.Parse (mision [9]);
						General.posicionIncial = new Vector3 (float.Parse (mision [5]), float.Parse (mision [6]), float.Parse (mision [7]));
				} else {
						Debug.Log (www.error);
				}
		}

		void pantallaConfiguracion(){
				GUIStyle style = new GUIStyle ();
				style = GUI.skin.GetStyle ("box");
				style.fontSize = (int)(25.0f);
				style = GUI.skin.GetStyle ("label");
				style.fontSize = (int)(20.0f);
				style.alignment = TextAnchor.UpperLeft;
				GUI.Box (new Rect (Screen.width / 10, Screen.height / 10, Screen.width - Screen.width / 6, Screen.height - Screen.height / 6), "Configuraciòn Grafica");

				GUI.Label (new Rect((Screen.width / 6), 3*(Screen.height / 10), Screen.width / 6, Screen.height / 10),"Calidad: ");
				if (GUI.Button (new Rect (2 * (Screen.width / 6), 3*(Screen.height / 10), Screen.width / 7, Screen.height / 10), "Baja")) {
						QualitySettings.SetQualityLevel (0);
				}

				if (GUI.Button (new Rect (3 * (Screen.width / 6), 3*(Screen.height / 10), Screen.width / 7, Screen.height / 10), "Media")) {
						QualitySettings.SetQualityLevel (3);
				}

				if (GUI.Button (new Rect (4 * (Screen.width / 6), 3*(Screen.height / 10), Screen.width / 7, Screen.height / 10), "Alta")) {
						QualitySettings.SetQualityLevel (5);
				}

				if (GUI.Button (new Rect (7 * (Screen.width / 10), Screen.height - Screen.height / 5, Screen.width / 6, Screen.height / 10), "Volver")) {
						opciones = 0;
				}				
		}
}